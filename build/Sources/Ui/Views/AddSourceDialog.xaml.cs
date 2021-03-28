using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAPICodePack.Dialogs;

using ewoxej_gitsame.Models;

namespace ewoxej_gitsame
{
    /// <summary>
    /// Логика взаимодействия для AddSourceDialog.xaml
    /// </summary>
    public partial class AddSourceDialog : Window
    {
        internal InputSource Item { get; set; }
        public AddSourceDialog()
        {
            Item = new InputSource { Type=InputSource.EType.File};
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            bool openFolder = false;
            var dlg = new CommonOpenFileDialog();
            if (btn.Name == "bBrowseFolder")
                dlg.IsFolderPicker = true;
            if (dlg.ShowDialog(this) == CommonFileDialogResult.Ok )
            {
                Item.Path = dlg.FileName;
                var label = FindName(dlg.IsFolderPicker ? "tbFolder" : "tbFile") as TextBlock;
                label.Text = dlg.FileName;
                BtnOk.IsEnabled = true;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            if (radio.Name == "rbFile")
            {
                Item.Type = InputSource.EType.File;
                if (!String.IsNullOrEmpty(tbFile.Text))
                    BtnOk.IsEnabled = true;
                else
                    BtnOk.IsEnabled = false;
            }
            else if (radio.Name == "rbFolder")
            {
                Item.Type = InputSource.EType.Folder;
                if (!String.IsNullOrEmpty(tbFolder.Text))
                    BtnOk.IsEnabled = true;
                else
                    BtnOk.IsEnabled = false;
            }
            else if (radio.Name == "rbRepo")
            {
                Item.Type = InputSource.EType.Repository;
                if (!String.IsNullOrEmpty(textBoxRepo.Text))
                    BtnOk.IsEnabled = true;
                else
                    BtnOk.IsEnabled = false;
            }
        }
        public bool isValidItem()
        {
            return !string.IsNullOrEmpty(Item.Path);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Item.Type == InputSource.EType.Repository)
            {
                Item.Path = (FindName("textBoxRepo") as TextBox).Text;
                var url = GitApi.Manager.toApiUrl(Item.Path);
                try
                {
                    var repo = GitApi.Manager.doRequest<GitApi.Repo>(url);
                    if (String.IsNullOrEmpty(repo.name))
                        throw new ArgumentException("Empty repo name");
                }
                catch( Exception ex)
                {
                    textBoxRepo.BorderBrush = Brushes.Red;
                    return;
                }
            }
            DialogResult = true;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void textBoxRepo_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxRepo.BorderBrush = Brushes.Black;
            if (rbRepo.IsChecked != true)
                return;
            if( !String.IsNullOrEmpty( textBoxRepo.Text ))
                BtnOk.IsEnabled = true;
            else
                BtnOk.IsEnabled = false;
        }
    }
}
