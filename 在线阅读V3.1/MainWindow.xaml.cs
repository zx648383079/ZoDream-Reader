using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
using 在线阅读.Class;
using 在线阅读.View;

namespace 在线阅读
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private BookPage _bookPage;

        private ReadPage _readPage;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _bookPage = new BookPage();
            MainFrame.Navigate(_bookPage);
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            switch (item.Header.ToString())
            {
                case "选项":
                    SetWindow set = new SetWindow();
                    if (set.ShowDialog()==true)
                    {
                        
                    }
                    break;
                    
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UserMethod method = new UserMethod();
            method.SavaBook(Model.BookModels);
        }
    }
}
