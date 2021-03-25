using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitSame.Analyzer;
using GitSame;

using ewoxej_gitsame.Sources.Ui.Models;
using System.Collections.ObjectModel;
using static ewoxej_gitsame.InputSource;

namespace ewoxej_gitsame
{
    class InputSourcesManager
    {
        SourcesViewModel model1;
        SourcesViewModel model2;
        ApplicationContext db;
        List<File> files1;
        List<File> files2;
        List<Source> sources1;
        List<Source> sources2;
        public ObservableCollection<ComparsionResult> Results { get; set; }
        public InputSourcesManager( SourcesViewModel model1, SourcesViewModel model2, ApplicationContext db )
        {
            this.model1 = model1;
            this.model2 = model2;
            this.db = db;
            files1 = new List<File>();
            files2 = new List<File>();
            sources1 = new List<Source>();
            sources2 = new List<Source>();
            Results = new ObservableCollection<ComparsionResult>();
        }
        private void compareFiles()
        {
            foreach( File f1 in files1 )
            {
                foreach( File f2 in files2 )
                {
                    var f1Copy = f1;
                    var f2Copy = f2;
                    var res = new ComparsionResult(ref f1Copy, ref f2Copy);
                    f1.Hash = f1Copy.Hash;
                    f2.Hash = f2Copy.Hash;
                    f1.Metainfo = f1Copy.Metainfo;
                    f2.Metainfo = f2Copy.Metainfo;
                    if (res.SimilarityRate > 50)
                        Results.Add(res);
                }
            }
        }
        public void run()
        {
            foreach( var i in model1.SourcesList )
                populateSomething(i, ref files1, ref sources1 );
            if (model2.UseLocalDb)
            {
                foreach( var i in db.Files )
                {
                    if( i.IsChecked == 1 )
                        files2.Add(i);
                }
            }
            else
            {
                foreach (var i in model2.SourcesList)
                    populateSomething(i, ref files2, ref sources2 );
            }

            compareFiles();
            if( model1.AddToDb )
                addToDb( files1, sources1);
            if ( model2.AddToDb && !model2.UseLocalDb)
                addToDb(files2, sources2);

            db.SaveChanges();
        }
        private void populateSomething( InputSource i, ref List<File> files, ref List<Source> sources )
        {
            if (i.Type == InputSource.EType.File)
                files.Add(populateFile(i.Path));
            else if (i.Type == InputSource.EType.Folder)
                files.AddRange(populateFolder(i.Path, ref sources));
            else if (i.Type == InputSource.EType.Repository)
                files.AddRange(populateRepository(i.Path, ref sources));
        }
        private File populateFile( string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            File f = new File
            {
                Path = filePath,
                CreationDate = info.CreationTime.ToFileTime(),
                Source = null,
                IsChecked = 1,
                Metainfo = getJsonPath( filePath ),
            };
            return f;
        }
        private List<File> populateFolder( string folderPath, ref List<Source> sources )
        {
            sources.Add(new Source { Type = (int)EType.Folder, IsChecked = 1, Path = folderPath, Name = Path.GetDirectoryName(folderPath) });
            List<File> lst = new List<File>();
            var files = Directory.GetFiles(folderPath);
            foreach (var i in files)
                lst.Add(populateFile(i));
            var subdirs = Directory.GetDirectories(folderPath);
            foreach (var i in subdirs)
                lst.AddRange(populateFolder(i, ref sources));
            return lst;
        }
        private List<File> populateRepository(string repoPath, ref List<Source> sources )
        {

            List<File> lst = new List<File>();
            var apiurl = GitApi.Manager.toApiUrl(repoPath);
            var repo = GitApi.Manager.doRequest<GitApi.Repo>(apiurl);
            var branch = GitApi.Manager.doRequest<GitApi.Branch>(repo.branches_url.Replace("{/branch}", "/" + repo.default_branch));
            var commit = GitApi.Manager.doRequest<GitApi.Commit>(branch.commit.commit.url);
            var tree = GitApi.Manager.getTree(commit, true);
            sources.Add(new Source { Name = repo.name, Path = repoPath, Branch = branch.name,
                IsChecked = 1, LastCommitHash = commit.sha, Type = (int)EType.Repository });
            foreach (var treeItem in tree.tree)
            {
            
                if (treeItem.type == "blob")
                {
                    string str = @"https://raw.githubusercontent.com/{0}/{1}/{2}/{3}";
                    var datestr = commit.author.date;
                    var ftime = DateTime.Parse(datestr).ToFileTime();
                    lst.Add(new File { Path = treeItem.path, CreationDate = ftime, Source = repoPath, 
                    IsChecked = 1, Metainfo = string.Format(str, repo.owner.login, repo.name, branch.name, treeItem.path) });
                }
            }
            return lst;
        }
        private void addToDb( List<File> files, List<Source> sources )
        {
            foreach( var i in files)
            {
                var query = db.Files.Where(p => p.Path == i.Path && p.Source == i.Source );
                if( query.Count() == 0 )
                    db.Files.Add(i);
            }
            foreach (var i in sources)
            {
                var query = db.Sources.Where(p => p.Path == i.Path);
                if (query.Count() == 0)
                    db.Sources.Add(i);
            }
        }
        static public string getJsonPath( string filePath )
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += "/GitSame/";

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            path += Helper.sha256_hash(filePath) + ".json";

            return path;
        }
    }
}
