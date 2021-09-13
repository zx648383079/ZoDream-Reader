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
            Title = book.Name;
        }

        public ReadViewModel ViewModel = new ReadViewModel();
        private bool isBooted = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight;
            ViewModel.Tokenizer.ColumnCount = 2;
            ViewModel.Tokenizer.Refresh();
            ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
            PageRender.FontSize = ViewModel.Tokenizer.FontSize;
            PageRender.Flush();
            PageRender.Draw(ViewModel.Tokenizer.Get());
            isBooted = true;
            ViewModel.Load();
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
            ViewModel.Book.Position = items[0].Begin;
            ViewModel.ReloadChapter();
            App.ViewModel.Update(ViewModel.Book);
            
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
            ViewModel.Book.Position = items[0].Begin;
            ViewModel.ReloadChapter();
            App.ViewModel.Update(ViewModel.Book);
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

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            MorePanel.Visibility = MorePanel.Visibility == Visibility.Visible ? 
                Visibility.Collapsed : Visibility.Visible;
        }

        private void ChapterListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = ChapterListBox.SelectedIndex;
            if (i < -1)
            {
                return;
            }
            MorePanel.Visibility = Visibility.Collapsed;
            var chapter = ViewModel.ChapterItems[i];
            ViewModel.ChapterTitle = chapter.Title;
            ViewModel.Tokenizer.SetPage(chapter);
            var items = ViewModel.Tokenizer.Get();
            if (items.Count < 1)
            {
                // 没有更多了
                return;
            }
            PageRender.Flush();
            PageRender.Draw(items);
            ViewModel.Book.Position = items[0].Begin;
            App.ViewModel.Update(ViewModel.Book);
        }
    }
}
