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
using System.Windows.Shapes;

using GitSame.Models;

namespace GitSame
{
    /// <summary>
    /// Логика взаимодействия для ResultsDialog.xaml
    /// </summary>
    public partial class ResultsDialog : Window
    {
        private ResultsViewModel model;
        private Window window;
        private ApplicationContext db;
        public ResultsDialog(Window mainWindow, ApplicationContext db)
        {
            InitializeComponent();
            window = mainWindow;
        }
        public void setupModel(List<ComparsionResult> res )
        {
            var collection = new ObservableCollection<ComparsionResult>();
            foreach( var i in res)
                collection.Add(i);

            model = new ResultsViewModel { Results = collection };
            DataContext = model;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void fileClick( File fileToOpen )
        {
            if (Uri.IsWellFormedUriString(fileToOpen.Source, UriKind.Absolute))
            {
                var src = db.Sources.Where(p => p.Path == fileToOpen.Source);
                string branch = "master";
                if (src.Count() > 0)
                    if( !String.IsNullOrEmpty( src.ElementAt(0).Branch ))
                        branch = src.ElementAt(0).Branch;

                System.Diagnostics.Process.Start(fileToOpen.Source + "/blob/" +branch+"/"+ fileToOpen.Path);
            }
            else
                System.Diagnostics.Process.Start(fileToOpen.Path);
        }
        private void File1_Click(object sender, RoutedEventArgs e)
        {
            fileClick(model.SelectedItem.File1);
        }

        private void File2_Click(object sender, RoutedEventArgs e)
        {
            fileClick(model.SelectedItem.File2);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            window.Show();
        }
    }
}
