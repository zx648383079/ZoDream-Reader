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
using ZoDream.Reader.Models;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// ReadView.xaml 的交互逻辑
    /// </summary>
    public partial class ReadView : Window
    {
        public ReadView(BookItem book)
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Book = book;
        }

        public ReadViewModel ViewModel = new ReadViewModel();
        private bool isBooted = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight;
            ViewModel.Tokenizer.ColumnCount = 2;
            ViewModel.Tokenizer.Refresh();
            PageRender.FontSize = ViewModel.Tokenizer.FontSize;
            PageRender.Flush();
            PageRender.Draw(ViewModel.Tokenizer.Get());
            isBooted = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Dispose();
        }

        private void PageRender_OnPrevious(object sender)
        {
            var items = ViewModel.Tokenizer.GetPrevious();
            if (items.Count < 1)
            {
                // 不能向前了
                return;
            }
            PageRender.Swap(items);
        }

        private void PageRender_OnNext(object sender)
        {
            var items = ViewModel.Tokenizer.GetNext();
            if (items.Count < 1)
            {
                // 没有更多了
                return;
            }
            PageRender.Swap(items);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isBooted)
            {
                return;
            }
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight;
            ViewModel.Tokenizer.Refresh();
            PageRender.Flush();
            PageRender.Draw(ViewModel.Tokenizer.Get());
        }
    }
}
