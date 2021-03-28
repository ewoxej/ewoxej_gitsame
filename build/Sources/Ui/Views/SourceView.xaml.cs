using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ewoxej_gitsame
{
    /// <summary>
    /// Логика взаимодействия для SourceView.xaml
    /// </summary>
    /// 
    public partial class SourceView : UserControl
    {
        private string title;
        private bool btnDb;
        internal SourcesViewModel Model { get; set; }
        public event EventHandler ItemAdded;
        public string Title { get { return title; } set 
            {
                title = value;
                var t = FindName("lTitle") as TextBlock;
                t.Text = Title;
            } 
        }
        public bool EnableDbButton { get { return btnDb; } set 
            {
                btnDb = value;
                var buttonDb = FindName("bDatabase") as Button;
                buttonDb.Visibility = (EnableDbButton) ? Visibility.Visible : Visibility.Hidden;
            } 
        }
        public SourceView()
        {
            InitializeComponent();
            Model = new SourcesViewModel();
            DataContext = Model;
        }

        private void DeleteItem_Clicked(object sender, RoutedEventArgs e)
        {
            Model.RemoveItem();
        }
        private void AddItem_Clicked(object sender, RoutedEventArgs e)
        {
            AddSourceDialog dialog = new AddSourceDialog();
            if (dialog.ShowDialog() == true)
            {
                if (dialog.isValidItem())
                {
                    Model.SourcesList.Add(dialog.Item);
                    ItemAdded(this, e);
                }
            }
        }
        private void UseDb_Clicked(object sender, RoutedEventArgs e)
        {
            Model.UseLocalDb = !Model.UseLocalDb;
            var listBox = FindName("lbSource") as ListBox;
            var placeholder = FindName("gridLocalDb") as Grid;
            var deleteBtn = FindName("deleteBtn") as Button;
            var addBtn = FindName("addBtn") as Button;
            var cbAdd = FindName("cbAddToDb") as CheckBox;
            if( Model.UseLocalDb )
            {
                placeholder.Visibility = Visibility.Visible;
                listBox.Visibility = Visibility.Hidden;
                cbAdd.Visibility = Visibility.Collapsed;
                deleteBtn.IsEnabled = false;
                addBtn.IsEnabled = false;
            }
            else
            {
                placeholder.Visibility = Visibility.Hidden;
                listBox.Visibility = Visibility.Visible;
                cbAdd.Visibility = Visibility.Visible;
                deleteBtn.IsEnabled = true;
                addBtn.IsEnabled = true;
            }
            ItemAdded(this, e);
        }

        private void DeleteAll_Clicked(object sender, RoutedEventArgs e)
        {
            Model.SourcesList.Clear();
        }
    }
}
