using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ewoxej_gitsame
{
    public class Source : INotifyPropertyChanged
    {
        public Source()
        {
            name = "";
            path = "";
            branch = "";
            last_commit_hash = "";
            is_checked = 0;
            Id = 0;
            Type = 0;
        }
        private string name;
        private string path;
        private string branch;
        private string last_commit_hash;
        private int is_checked;
        public int Id { get; set; }
        public int Type { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        public string Branch
        {
            get { return branch; }
            set
            {
                branch = value;
                OnPropertyChanged("Branch");
            }
        }
        public string LastCommitHash
        {
            get { return last_commit_hash; }
            set
            {
                last_commit_hash = value;
                OnPropertyChanged("LastCommitHash");
            }
        }
        public int IsChecked
        {
            get { return is_checked; }
            set
            {
                is_checked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}