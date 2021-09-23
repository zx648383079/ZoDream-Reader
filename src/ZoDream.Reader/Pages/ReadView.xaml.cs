using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Models;

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
            loadingRing.Visibility = Visibility.Visible;
        }

        public ReadViewModel ViewModel = new ReadViewModel();
        private bool isBooted = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight - 20;
            ViewModel.Tokenizer.ColumnCount = 2;
            Load();
        }

        private async void Load()
        {
            await ViewModel.Tokenizer.Refresh();
            ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
            PageRender.FontSize = ViewModel.Tokenizer.FontSize;
            PageRender.Flush();
            loadingRing.Visibility = Visibility.Collapsed;
            PageRender.Draw(await ViewModel.Tokenizer.GetAsync());
            isBooted = true;
            ViewModel.Load();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Dispose();
        }

        private async void PageRender_OnPrevious(object sender)
        {
            var items = await ViewModel.Tokenizer.GetPreviousAsync();
            if (items.Count < 1)
            {
                // 不能向前了
                return;
            }
            PageRender.Swap(items);
            ViewModel.Book.Position = items[0].Begin;
            ViewModel.ReloadChapter();
            App.ViewModel.DatabaseRepository.UpdateBook(ViewModel.Book);
        }

        private async void PageRender_OnNext(object sender)
        {
            var items = await ViewModel.Tokenizer.GetNextAsync();
            if (items.Count < 1)
            {
                // 没有更多了
                return;
            }
            PageRender.Swap(items);
            ViewModel.Book.Position = items[0].Begin;
            ViewModel.ReloadChapter();
            App.ViewModel.DatabaseRepository.UpdateBook(ViewModel.Book);
        }

        private async void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isBooted)
            {
                return;
            }
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight;
            await ViewModel.Tokenizer.Refresh();
            PageRender.Flush();
            PageRender.Draw(await ViewModel.Tokenizer.GetAsync());
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            MorePanel.Visibility = MorePanel.Visibility == Visibility.Visible ? 
                Visibility.Collapsed : Visibility.Visible;
        }

        private async void ChapterListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            var items = await ViewModel.Tokenizer.GetAsync();
            if (items.Count < 1)
            {
                // 没有更多了
                return;
            }
            PageRender.Flush();
            PageRender.Draw(items);
            ViewModel.Book.Position = items[0].Begin;
            App.ViewModel.DatabaseRepository.UpdateBook(ViewModel.Book);
        }
    }
}
