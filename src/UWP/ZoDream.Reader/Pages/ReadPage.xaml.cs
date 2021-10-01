using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Controls;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ReadPage : Page
    {
        public ReadPage()
        {
            this.InitializeComponent();
        }

        public ReadViewModel ViewModel = new ReadViewModel();
        private bool isBooted = false;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Tokenizer.Dispose();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadingBtn.Visibility = Visibility.Visible;
            App.ViewModel.ListenNavigate();
            ViewModel.Book = e.Parameter as BookItem;
            ApplicationView.GetForCurrentView().Title = ViewModel.Book.Name;
            var setting = App.ViewModel.Setting;
            PageRender.FontSize = ViewModel.Tokenizer.FontSize = setting.FontSize;
            PageRender.FontFamily = new FontFamily(setting.FontFamily);
            ViewModel.Tokenizer.Left = ViewModel.Tokenizer.Right =
                ViewModel.Tokenizer.Top = ViewModel.Tokenizer.Bottom = setting.Padding;
            ViewModel.Tokenizer.LetterSpace = setting.LetterSpace;
            ViewModel.Tokenizer.LineSpace = setting.LineSpace;
            ViewModel.Tokenizer.Gap = setting.Padding * 2;
            BootAsync();
        }

        private async void BootAsync()
        {
            if (!isBooted && PageRender.ActualWidth > 0)
            {
                PageRender.Source = ViewModel;
                ViewModel.Tokenizer.ColumnCount = App.ViewModel.Setting.ColumnCount;
                await OnResizeAsync();
                ViewModel.Load();
                isBooted = true;
                LoadingBtn.Visibility = Visibility.Collapsed;
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
            await PageRender.SwapTo(tokenizer.Page);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isBooted)
            {
                BootAsync();
                return;
            }
            _ = OnResizeAsync();
        }

        private async void JumpBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.Tokenizer.PageCount < 1)
            {
                return;
            }
            var dialog = new ProgressDialog();
            dialog.Value = ViewModel.Tokenizer.Page * 100 / ViewModel.Tokenizer.PageCount;
            var res = await dialog.ShowAsync();
            if (res != ContentDialogResult.Primary)
            {
                return;
            }
            ViewModel.Tokenizer.SetPageScale(dialog.Value, 100);
            await PageRender.SwapTo(ViewModel.Tokenizer.Page);
        }

        private void SettingBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.ViewModel.Navigate(typeof(SettingPage), null, false);
        }

        private void ChapterBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChapterPanel.IsOpen = !ChapterPanel.IsOpen;
        }

        private async void ChapterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ChapterListBox.SelectedIndex;
            if (i < -1)
            {
                return;
            }
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
            progressTb.Text = $"{page}/{ViewModel.Tokenizer.PageCount}";
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
