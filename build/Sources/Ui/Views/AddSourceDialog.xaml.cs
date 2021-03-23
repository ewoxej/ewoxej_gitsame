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
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            if (radio.Name == "rbFile")
                Item.Type = InputSource.EType.File;
            else if (radio.Name == "rbFolder")
                Item.Type = InputSource.EType.Folder;
            else if (radio.Name == "rbRepo")
                Item.Type = InputSource.EType.Repository;
        }
        public bool isValidItem()
        {
            return !string.IsNullOrEmpty(Item.Path);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Item.Type == InputSource.EType.Repository)
                Item.Path = (FindName("textBoxRepo") as TextBox).Text;
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
    }
}
