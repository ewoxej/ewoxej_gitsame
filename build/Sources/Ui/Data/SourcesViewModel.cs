using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ewoxej_gitsame
{
    class SourcesViewModel
    {
        public ObservableCollection<InputSource> SourcesList { get; set; }
        public bool UseLocalDb { get; set; }
        public InputSource CurrentItem { get; set; }
        public bool AddToDb { get; set; }
        public SourcesViewModel()
        {
            SourcesList = new ObservableCollection<InputSource>();
        }
        public void RemoveItem()
        {
            SourcesList.Remove(CurrentItem);
        }
    }
}
