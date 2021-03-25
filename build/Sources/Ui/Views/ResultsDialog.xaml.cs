using ewoxej_gitsame.Sources.Ui.Models;
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

namespace ewoxej_gitsame.Sources.Ui.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultsDialog.xaml
    /// </summary>
    public partial class ResultsDialog : Window
    {
        private ResultsViewModel model;
        private Window window;
        public ResultsDialog(ObservableCollection<ComparsionResult> res, Window mainWindow)
        {
            InitializeComponent();
            model = new ResultsViewModel { Results = res };
            DataContext = model;
            window = mainWindow;
            mainWindow.Hide();
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
            if (Uri.IsWellFormedUriString( fileToOpen.Source, UriKind.Absolute))
                System.Diagnostics.Process.Start(fileToOpen.Source + "/blob/master/" + fileToOpen.Path);
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
            Close();
            window.Show();
        }
    }
}
