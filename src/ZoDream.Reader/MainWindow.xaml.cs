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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Reader.Events;
using ZoDream.Reader.Pages;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Models;

namespace ZoDream.Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public MainViewModel ViewModel = App.ViewModel;

        private void BookListBox_OnAdd(object sender)
        {
            var open = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "文本文件|*.txt|所有文件|*.*",
                Title = "选择文件"
            };
            if (open.ShowDialog() != true)
            {
                return;
            }
            ViewModel.Load(open.FileNames);
        }

        private void BookListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void BookListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                ViewModel.Load(file);
            }
        }

        private void BookListBox_OnAction(object sender, BookItem item, Events.ActionEvent e)
        {
            if (e == ActionEvent.CLICK)
            {
                var page = new ReadView(item);
                page.ShowDialog();
                return;
            }
            if (e == ActionEvent.DELETE)
            {
                ViewModel.RemoveBook(item);
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
