using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GitSame;
using GitSame.Analyzer;
using GitSame.Analyzer.FileDescriptions;

namespace ewoxej_gitsame.Sources.Ui.Models
{
    public class ComparsionResult
    {
        public int SimilarityRate{get;set;}
        public File File1 { get; set; }
        public File File2 { get; set; }
        public string File1Date { get; set; }
        public string File2Date { get; set; }
        public ComparsionResult( ref File file1, ref File file2 )
        {
            var desc1 = getDescription(ref file1);
            var desc2 = getDescription(ref file2);
            SimilarityRate = Comparator.CompareDescriptions(desc1, desc2);
            File1 = file1;
            File2 = file2;

            File1Date = DateTime.FromFileTimeUtc(File1.CreationDate).ToString();
            File2Date = DateTime.FromFileTimeUtc(File2.CreationDate).ToString();
        }
        public BasicFileDescription getDescription( ref File file)
        {
            if (System.IO.File.Exists(file.Metainfo))
                return DescriptionGenerator.LoadDescriptionFromFile(file.Metainfo);

            string filePath = file.Path;
            if (Uri.IsWellFormedUriString(file.Metainfo, UriKind.Absolute))
            {
                WebClient client = new WebClient();
                var tempPath = Path.GetTempFileName();
                client.DownloadFile(file.Metainfo, tempPath);
                filePath = tempPath;
                file.Metainfo = InputSourcesManager.getJsonPath(file.Metainfo);
            }

            BasicFileDescription desc = DescriptionGenerator.GenerateDescriptionFromFile(filePath);
            DescriptionGenerator.WriteDescriptionToFile(desc, file.Metainfo);
            file.Hash = DescriptionGenerator.GetHash(desc);
            return desc;
        }
    }
}
