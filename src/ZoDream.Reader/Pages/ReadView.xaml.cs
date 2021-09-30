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
        }

        private async void Load()
        {
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight - 20;
            ViewModel.Tokenizer.ColumnCount = App.ViewModel.Setting.ColumnCount;
            await ViewModel.Tokenizer.Refresh();
            ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
            PageRender.Source = ViewModel;
            PageRender.FontSize = ViewModel.Tokenizer.FontSize;
            PageRender.Flush();
            loadingRing.Visibility = Visibility.Collapsed;
            PageRender.SwapTo(ViewModel.Tokenizer.Page);
            isBooted = true;
            ViewModel.Load();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Dispose();
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
            PageRender.SwapTo(ViewModel.Tokenizer.Page);
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

        private void PageRender_PageChanged(object sender, int page, PositionItem pagePosition)
        {
            ViewModel.Tokenizer.Page = page;
            ViewModel.Book.Position = pagePosition;
            ViewModel.ReloadChapter();
            App.ViewModel.DatabaseRepository.UpdateBook(ViewModel.Book);
        }

        private void PageRender_OnReady(object sender)
        {
            Load();
        }
    }
}
