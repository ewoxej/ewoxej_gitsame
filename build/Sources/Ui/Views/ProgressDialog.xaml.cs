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
using GitSame;

namespace GitSame
{
    /// <summary>
    /// Логика взаимодействия для ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {
        TextBlock text;
        ProgressBar progressBar;
        Action cancelAction;
        public ProgressDialog( Action onCancel)
        {
            InitializeComponent();
            cancelAction = onCancel;
            text = FindName("textCaption") as TextBlock;
            progressBar = FindName("progress") as ProgressBar;
        }

        public void ShowAsResult()
        {
            text.Text = "Совпадений нет";
            progressBar.Visibility = Visibility.Collapsed;
            MainBtn.Content = "OK";
            Show();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelAction();
            Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void OnProgressChanged( InputSourcesManager.Operation oper, int progress)
        {
            progressBar.Dispatcher.Invoke(() => progressBar.Value = progress);

            string textForLabel;
            switch( oper)
            {
                case InputSourcesManager.Operation.AccessToDb: textForLabel = "Запись данных в БД...";break;
                case InputSourcesManager.Operation.Analyzing: textForLabel = "Сравнение файлов..."; break;
                case InputSourcesManager.Operation.Idle: textForLabel = "Подождите..."; break;
                case InputSourcesManager.Operation.GetRemoteFiles: textForLabel = "Получение файлов..."; break;
                default: textForLabel = ""; break;
            }
            text.Dispatcher.Invoke(()=> text.Text = textForLabel);
        }

        public void OnError( Exception e)
        {
            text.Dispatcher.Invoke(() => { text.FontSize = 10;
                text.TextWrapping = TextWrapping.Wrap; text.Text = e.Message; });
            progressBar.Dispatcher.Invoke(() => progressBar.Value = 0);
        }
    }
}
