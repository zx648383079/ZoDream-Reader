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
            App.ViewModel.ListenNavigate();
            ViewModel.Book = e.Parameter as BookItem;
            ApplicationView.GetForCurrentView().Title = ViewModel.Book.Name;

            PageRender.FontSize = ViewModel.Tokenizer.FontSize;
            ViewModel.Tokenizer.ColumnCount = 2;
            BootAsync();
        }

        private async void BootAsync()
        {
            if (PageRender.ActualWidth > 0)
            {
                ViewModel.Tokenizer.Width = PageRender.ActualWidth;
                ViewModel.Tokenizer.Height = PageRender.ActualHeight;
                await ViewModel.Tokenizer.Refresh();
                ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
                PageRender.Flush();
                PageRender.Draw(await ViewModel.Tokenizer.GetAsync());
                ViewModel.Load();
                isBooted = true;
            }
        }

        private async void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isBooted)
            {
                BootAsync();
                return;
            }
            ViewModel.Tokenizer.Width = PageRender.ActualWidth;
            ViewModel.Tokenizer.Height = PageRender.ActualHeight;
            await ViewModel.Tokenizer.Refresh();
            PageRender.Flush();
            PageRender.Draw(await ViewModel.Tokenizer.GetAsync());
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
    }
}
