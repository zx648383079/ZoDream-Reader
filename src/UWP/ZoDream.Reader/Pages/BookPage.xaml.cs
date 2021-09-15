using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Events;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BookPage : Page
    {
        public BookPage()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel = App.ViewModel;

        private void BookListBox_OnAdd(object sender)
        {
            _ = LoadFilesAsync();
        }

        private async Task LoadFilesAsync()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".txt");
            var files = await picker.PickMultipleFilesAsync();
            ViewModel.Load(files);
        }

        private void BookListBox_OnAction(object sender, BookItem item, ActionEvent e)
        {
            if (e == ActionEvent.CLICK)
            {
                ViewModel.Navigate(typeof(ReadPage), item, true);
            }
        }
    }
}
