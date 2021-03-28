using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using ewoxej_gitsame.Models;

namespace ewoxej_gitsame
{
    public class SourcesViewModel : INotifyPropertyChanged
    {
        private bool useLocalDb;
        public ObservableCollection<InputSource> SourcesList { get; set; }
        public bool UseLocalDb
        {
            get { return useLocalDb; }
            set { useLocalDb = value; OnPropertyChanged("UseLocalDb"); }
        }
        public InputSource CurrentItem { get; set; }
        public bool AddToDb { get; set; }
        public SourcesViewModel()
        {
            UseLocalDb = false;
            AddToDb = true;
            SourcesList = new ObservableCollection<InputSource>();
        }
        public void RemoveItem()
        {
            SourcesList.Remove(CurrentItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
