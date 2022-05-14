using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            loadingRing.IsActive = true;
        }

        public ReadViewModel ViewModel = new();
        private bool isBooted = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // BootAsync();
        }

        private async void BootAsync()
        {
            if (!isBooted && PageRender.ActualWidth > 0)
            {
                var setting = App.ViewModel.Setting;
                PageRender.Setting = App.ViewModel.Setting;
                ViewModel.Tokenizer.Left = ViewModel.Tokenizer.Right =
                    ViewModel.Tokenizer.Top = ViewModel.Tokenizer.Bottom = setting.Padding;
                ViewModel.Tokenizer.LetterSpace = setting.LetterSpace;
                ViewModel.Tokenizer.LineSpace = setting.LineSpace;
                ViewModel.Tokenizer.Gap = setting.Padding * 2;
                PageRender.Source = ViewModel;
                ViewModel.Tokenizer.ColumnCount = App.ViewModel.Setting.ColumnCount;
                await OnResizeAsync();
                ViewModel.Load();
                isBooted = true;
            }
        }

        private async Task OnResizeAsync()
        {
            var width = PageRender.ActualWidth;
            var height = PageRender.ActualHeight - 20;
            var tokenizer = ViewModel.Tokenizer;
            if (tokenizer.Width == width && tokenizer.Height == height)
            {
                return;
            }
            tokenizer.Width = width;
            tokenizer.Height = height;
            await tokenizer.Refresh();
            ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
            PageRender.Flush();
            await PageRender.SwapToAsync(tokenizer.Page);
            loadingRing.IsActive = false;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tokenizer.Dispose();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isBooted)
            {
                BootAsync();
                return;
            }
            _ = OnResizeAsync();
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
            await PageRender.SwapToAsync(ViewModel.Tokenizer.Page);
            // App.ViewModel.DatabaseRepository.UpdateBook(ViewModel.Book);
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
            BootAsync();
        }
    }
}
