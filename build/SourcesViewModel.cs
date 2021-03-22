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
        public ObservableCollection<SourceModel> SourcesList { get; set; }

        public SourceModel CurrentItem { get; set; }
        public SourcesViewModel()
        {
            SourcesList = new ObservableCollection<SourceModel>();
        }
        public void RemoveItem()
        {
            SourcesList.Remove(CurrentItem);
        }
    }
}
