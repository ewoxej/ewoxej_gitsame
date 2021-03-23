using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitSame.Analyzer;
using GitSame;

namespace ewoxej_gitsame
{
    class InputSourcesManager
    {
        SourcesViewModel model1;
        SourcesViewModel model2;
        ApplicationContext db;
        List<File> files1;
        List<File> files2;
        public InputSourcesManager( SourcesViewModel model1, SourcesViewModel model2, ApplicationContext db )
        {
            this.model1 = model1;
            this.model2 = model2;
            this.db = db;
            files1 = new List<File>();
            files2 = new List<File>();
        }
        private void compareFiles()
        {
            //compare and push into comparsion results. Make ComparsionResults model that show such a thing
        }
        public void run()
        {
            foreach( var i in model1.SourcesList )
            {
                if (i.Type == InputSource.EType.File)
                    files1.Add(populateFile(i));
                //for folder and for repos and case for db
            }
            foreach (var i in model2.SourcesList)
            {
                if (i.Type == InputSource.EType.File)
                    files2.Add(populateFile(i));
            }
            //push to db
            compareFiles();
            //emit comparsionFinished()

        }
        private File populateFile( InputSource file)
        {
            FileInfo info = new FileInfo(file.Path);
            File f = new File
            {
                Path = file.Path,
                CreationDate = (int)info.CreationTime.ToFileTime(),
                Source = null,
                IsChecked = 0,
                Metainfo = getJsonPath( file.Path ),
            };
            var description = DescriptionGenerator.GenerateDescriptionFromFile(file.Path);
            DescriptionGenerator.WriteDescriptionToFile(description, f.Metainfo);
            return f;
            // Calculate description content hash
        }
        private List<File> populateFolder( InputSource folder )
        {
            List<File> lst = new List<File>();
            return lst;
        }
        private List<File> populateRepository(InputSource repo)
        {
            List<File> lst = new List<File>();
            //connect to GitApi and read files
            return lst;
        }
        private void addToDb( SourcesViewModel model )
        {

        }
        private string getJsonPath( string filePath )
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
