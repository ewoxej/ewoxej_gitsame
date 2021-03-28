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
using ewoxej_gitsame.Sources.Ui.Views;

namespace ewoxej_gitsame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        InputSourcesManager manager;
        ResultsDialog resultsDialog;
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

            manager = new InputSourcesManager((FindName("source1") as SourceView).Model, (FindName("source2") as SourceView).Model );
            resultsDialog = new ResultsDialog(this, db);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseTab.IsSelected)
                db.SaveChanges();
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

        public void onProcessingFinished()
        {
            resultsDialog.Dispatcher.Invoke(() => 
            {
                if (manager.Results.Count == 0)
                {
                    ProgressDialog progressDialog = new ProgressDialog(onCanceled);
                    Hide();
                    progressDialog.ShowAsResult();
                    return;
                }
                resultsDialog.setupModel(manager.Results);
                resultsDialog.Show(); 
            });
            manager.IsCanceled = false;
        }
        public void onCanceled()
        {
            manager.IsCanceled = true; 
            Dispatcher.Invoke(() => Show());
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog(onCanceled);
            Hide();

            manager.setCallbacks(progressDialog.OnProgressChanged,
                () => { progressDialog.Dispatcher.Invoke(() => progressDialog.Close()); onProcessingFinished(); },
                progressDialog.OnError);
            progressDialog.Show();
            //manager.run();
            //progressDialog.Close(); onProcessingFinished();
            Task.Run( ()=> manager.run() );
        }

        private void CleanDb_Click(object sender, RoutedEventArgs e)
        {
            Helper.CleanData();
        }

        private void DbTab_Unselected(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
        private void DbTab_Selected(object sender, RoutedEventArgs e)
        {
            db.Sources.Load();
            db.Files.Load();
        }

        private void markAllItems(bool isChecked)
        {
            if (DbTabFiles.IsSelected)
            {
                foreach (var i in db.Files)
                    i.IsCheckedBool = isChecked;
            }
            else if (DbTabSources.IsSelected)
            {
                foreach (var i in db.Sources)
                    i.IsCheckedBool = isChecked;
            }
        }
        private void CheckAll_Click(object sender, RoutedEventArgs e)
        {
            markAllItems(true);
        }

        private void UncheckAll_Click(object sender, RoutedEventArgs e)
        {
            markAllItems(false);
        }

        private void DbDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DbTabFiles.IsSelected)
            {
                var selected = lvFiles.SelectedItem as Models.File;
                if (selected != null)
                    db.Files.Remove(selected);
            }
            else if (DbTabSources.IsSelected)
            {
                var selected = lvSources.SelectedItem as Models.Source;
                if (selected != null)
                {
                    var filesFromSrc = db.Files.Where(p => p.Source == selected.Path);
                    foreach (var i in filesFromSrc)
                        db.Files.Remove(i);

                    db.Sources.Remove(selected);
                }
            }
            db.SaveChanges();
        }

        private void source_ItemAdded(object sender, EventArgs e)
        {
            RunBtn.IsEnabled = (source1.Model.SourcesList.Count != 0 && 
                ( source2.Model.SourcesList.Count != 0 || source2.Model.UseLocalDb ));
        }

        private void OnSourceCheckChanged(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            String path = cb.CommandParameter as String;
            var filesFromSrc = db.Files.Where(p => p.Source == path);
            foreach (var i in filesFromSrc)
                i.IsCheckedBool = (bool)cb.IsChecked;
        }
    }
}
