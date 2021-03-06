using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GitSame.Models;
namespace GitSame
{
    class ResultsViewModel
    {
        private ComparsionResult result;
        public ObservableCollection<ComparsionResult> Results { get; set; }
        public ComparsionResult SelectedItem 
        { 
            get
            { 
                if (result == null && Results != null) 
                    result= Results.ElementAt(0);  
                return result; 
            } 
            set { result = value; } 
        }
        public ResultsViewModel()
        {
        }
    }
}
