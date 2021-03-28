using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitSame.Analyzer;
using GitSame;

using System.Collections.ObjectModel;
using static GitSame.Models.InputSource;
using System.Data.Entity;

namespace GitSame
{
    public class InputSourcesManager
    {
        public enum Operation
        { 
            GetRemoteFiles,
            Analyzing,
            AccessToDb,
            Idle
        };
        SourcesViewModel model1;
        SourcesViewModel model2;
        ApplicationContext db;
        List<Models.File> files1;
        List<Models.File> files2;
        List<Models.Source> sources1;
        List<Models.Source> sources2;
        Action<Operation, int> onBusyAction;
        Action onFinishedAction;
        Action<Exception> onErrorAction;
        public bool IsCanceled { get; set; }
        public List<Models.ComparsionResult> Results { get; set; }
        public InputSourcesManager( SourcesViewModel model1, SourcesViewModel model2 )
        {
            this.model1 = model1;
            this.model2 = model2;
            this.db = new ApplicationContext();
            try
            {
                db.Sources.Load();
                db.Files.Load();
            }
            catch(Exception e)
            {
                throw e;
            }
            files1 = new List<Models.File>();
            files2 = new List<Models.File>();
            sources1 = new List<Models.Source>();
            sources2 = new List<Models.Source>();
            Results = new List<Models.ComparsionResult>();
            IsCanceled = false;
        }
        public void setCallbacks( Action<Operation,int> actionBusy, Action actionFinished, Action<Exception> actionError)
        {
            onBusyAction = actionBusy;
            onFinishedAction = actionFinished;
            onErrorAction = actionError;
        }
        private void compareFiles()
        {
            try
            {
                if (IsCanceled)
                    return;
                int totalCount = files1.Count * files2.Count;
                int processedCount = 0;
                foreach (var f1 in files1)
                {
                    foreach (var f2 in files2)
                    {
                        onBusyAction(Operation.Analyzing, (processedCount / totalCount * 100));
                        var f1Copy = f1;
                        var f2Copy = f2;
                        var res = new Models.ComparsionResult(ref f1Copy, ref f2Copy);
                        f1.Hash = f1Copy.Hash;
                        f2.Hash = f2Copy.Hash;
                        f1.Metainfo = f1Copy.Metainfo;
                        f2.Metainfo = f2Copy.Metainfo;
                        if (res.SimilarityRate > 50)
                            Results.Add(res);
                        ++processedCount;
                        if (IsCanceled)
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                onErrorAction(e);
            }
        }
        public async void run()
        {
            try
            {
                foreach (var i in model1.SourcesList)
                {
                    populateSomething(i, ref files1, ref sources1);
                    if (IsCanceled)
                        break;
                }
                if (model2.UseLocalDb)
                {
                    foreach (var i in db.Files)
                    {
                        if (i.IsCheckedBool)
                            files2.Add(i);
                        if (IsCanceled)
                            break;
                    }
                }
                else
                {
                    foreach (var i in model2.SourcesList)
                    {
                        populateSomething(i, ref files2, ref sources2);
                        if (IsCanceled)
                            break;
                    }
                }

                compareFiles();
                if (model1.AddToDb)
                    addToDb(files1, sources1);
                if (model2.AddToDb && !model2.UseLocalDb)
                    addToDb(files2, sources2);

                if (IsCanceled)
                    return;

                db.SaveChanges();
                onFinishedAction();
            }
            catch(Exception e)
            {
                onErrorAction(e);
            }
        }
        private void populateSomething(Models.InputSource i, ref List<Models.File> files, ref List<Models.Source> sources )
        {
            if (i.Type == EType.File)
                files.Add(populateFile(i.Path));
            else if (i.Type == EType.Folder)
                files.AddRange(populateFolder(i.Path, ref sources, i.Path, true));
            else if (i.Type == EType.Repository)
                files.AddRange(populateRepository(i.Path, ref sources));
        }
        static internal Models.File populateFile( string filePath, string sourcePath = null )
        {
            var info = new FileInfo(filePath);
            var f = new Models.File
            {
                Path = filePath,
                CreationDate = info.CreationTime.ToFileTime(),
                Source = sourcePath,
                IsChecked = 1,
                Metainfo = getJsonPath( filePath ),
            };
            return f;
        }
        static internal List<Models.File> populateFolder( string folderPath, ref List<Models.Source> sources,
            string rootPath, bool addToSrc = true)
        {
            if( addToSrc )
                sources.Add(new Models.Source { Type = (int)EType.Folder, IsChecked = 1, 
                    Path = folderPath, Name = Path.GetFileName(folderPath) });

            List<Models.File> lst = new List<Models.File>();
            var files = Directory.GetFiles(folderPath);
            foreach (var i in files)
            {
                if (Settings.getInstance().isInFilters(i.Replace('\\','/')))
                    continue;
                lst.Add(populateFile(i, rootPath));
            }
            var subdirs = Directory.GetDirectories(folderPath);
            foreach (var i in subdirs)
            {
                if (Settings.getInstance().isInFilters(i.Replace('\\', '/')))
                    continue;
                lst.AddRange(populateFolder(i, ref sources, rootPath, false));
            }
            return lst;
        }
        internal List<Models.File> populateRepository(string repoPath, ref List<Models.Source> sources )
        {
            List<Models.File> lst = new List<Models.File>();
            try
            {
                var apiurl = GitApi.Manager.toApiUrl(repoPath);
                var repo = GitApi.Manager.doRequest<GitApi.Repo>(apiurl);
                var branch = GitApi.Manager.doRequest<GitApi.Branch>(repo.branches_url.Replace("{/branch}", "/" + repo.default_branch));
                var commit = GitApi.Manager.doRequest<GitApi.Commit>(branch.commit.commit.url);
                var tree = GitApi.Manager.getTree(commit, true);
                sources.Add(new Models.Source
                {
                    Name = repo.name,
                    Path = repoPath,
                    Branch = branch.name,
                    IsChecked = 1,
                    LastCommitHash = commit.sha,
                    Type = (int)EType.Repository
                });
                int totalCount = tree.tree.Count;
                int procCount = 0;
                foreach (var treeItem in tree.tree)
                {
                    if (IsCanceled)
                        break;
                    if (treeItem.type == "blob")
                    {
                        if (Settings.getInstance().isInFilters(treeItem.path))
                            continue;

                        string str = @"https://raw.githubusercontent.com/{0}/{1}/{2}/{3}";
                        var datestr = commit.author.date;
                        var item = new Models.File
                        {
                            Path = treeItem.path,
                            DateTimeStr = datestr,
                            Source = repoPath,
                            IsChecked = 1,
                            Metainfo = string.Format(str, repo.owner.login, repo.name, branch.name, treeItem.path),
                        };
                        lst.Add(item);
                    }
                    procCount++;
                    onBusyAction(Operation.GetRemoteFiles, (procCount / totalCount * 100));
                }
            }
            catch( Exception e)
            {
                onErrorAction(e);
            }
            return lst;
        }
        private void addToDb( List<Models.File> files, List<Models.Source> sources )
        {
            if (IsCanceled)
                return;
            int totalCount = files.Count + sources.Count;
            int processedCount = 0;
            foreach( var i in files)
            {
                var query = db.Files.Where(p => p.Path == i.Path && p.Source == i.Source );
                if( query.Count() == 0 )
                    db.Files.Add(i);
                processedCount++;
                if (IsCanceled)
                    break;
                onBusyAction(Operation.AccessToDb, (processedCount / totalCount * 100));
            }
            foreach (var i in sources)
            {
                var query = db.Sources.Where(p => p.Path == i.Path);
                if (query.Count() == 0)
                    db.Sources.Add(i);
                processedCount++;
                if (IsCanceled)
                    break;
                onBusyAction(Operation.AccessToDb, (processedCount / totalCount * 100));
            }
        }
        static public string getJsonPath( string filePath )
        {
            return Path.Combine(Helper.JsonPath, Helper.sha256_hash(filePath) + ".json");
        }
    }
}
