using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitSame.Models
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
            Type = 0;
        }
        private string name;
        private string path;
        private string branch;
        private string last_commit_hash;
        private int is_checked;
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
        [Key]
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

        [NotMapped]
        public bool IsCheckedBool { get { return (is_checked == 1); } set { IsChecked = (value) ? 1 : 0; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}