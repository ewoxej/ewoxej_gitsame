using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            DataContext = db;
            try
                {
                db.Sources.Load();
                db.Files.Load();
            }
            catch ( Exception e)
            {
                var test = e;
            }
            var lvSources = FindName("lvSources") as ListView;
            var lvFiles = FindName("lvFiles") as ListView;
            lvSources.ItemsSource = db.Sources.Local.ToBindingList();
            lvFiles.ItemsSource = db.Files.Local.ToBindingList();
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
    }
}
