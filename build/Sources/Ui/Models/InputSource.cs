using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ewoxej_gitsame
{
    class InputSource
    {
        public InputSource()
        {
            Type = EType.Repository;
        
        }
        public enum EType
        {
            Repository,
            File,
            Folder
        }
        private EType type;
        private string path;
        public EType Type { get { return type; } set{ type = value; IconKind = getIconKind(); } }
        public string FriendlyName { get; set; }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                if (Type == EType.Repository)
                {
                    var tokens = path.Split('/');
                    FriendlyName = tokens.Last();
                }
                else if( Type == EType.File )
                {
                    FriendlyName = System.IO.Path.GetFileName(value);
                }
                else if( Type == EType.Folder)
                {
                    FriendlyName = System.IO.Path.GetDirectoryName(value);
                }
            }
        }

        public string IconKind { get; set; }
        private string getIconKind()
        {
            switch( type )
            {
                case EType.Repository: return "GitHub";
                case EType.Folder: return "Folder";
                case EType.File: return "File";
            }
            return "";
        }

    }
}
