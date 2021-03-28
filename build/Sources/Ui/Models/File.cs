using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ewoxej_gitsame.Models
{
    public class File : INotifyPropertyChanged
	{
		private string hash;
		private string metainfo;
		private int is_checked;
		public int Id { get; set; }
		public long CreationDate { get; set; }
		public string Source { get; set; }
		public string Path { get; set; }
		public string Hash { get { return hash; } set { hash = value; OnPropertyChanged("Hash"); } }
		public string Metainfo { get { return metainfo; } set { metainfo = value; OnPropertyChanged("Metainfo"); } }
		public int IsChecked { get { return is_checked; } set { is_checked = value; OnPropertyChanged("IsChecked"); } }

		[NotMapped]
		public bool IsCheckedBool { get { return (is_checked == 1); } set { IsChecked = (value)?1:0; } }

		[NotMapped]
		public string DateTimeStr 
		{ 
			get { return DateTime.FromFileTimeUtc(CreationDate).ToString(); }
			set { CreationDate = DateTime.Parse(value).ToFileTime(); } 
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
