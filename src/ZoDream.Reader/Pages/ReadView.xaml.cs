using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Repositories;

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// ReadView.xaml 的交互逻辑
    /// </summary>
    public partial class ReadView : Window
    {
        public ReadView(INovel book)
        {
            InitializeComponent();
            _ = ViewModel.LoadAsync(book);
            Title = book.Name;
        }

        public ReadViewModel ViewModel => (ReadViewModel)DataContext;
        private bool isBooted = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // BootAsync();
        }


        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //ViewModel.Tokenizer.Dispose();
            _ = ViewModel.SaveAsync();
        }


        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            var isOpen = MorePanel.Visibility != Visibility.Visible;
            MorePanel.Visibility = isOpen ? Visibility.Visible : Visibility.Collapsed;
            if (isOpen)
            {
                OptionPanel.Show();
                //OptionPanel.ViewModel.Option = App.ViewModel.Setting;
            } else
            {
                OptionPanel.Hide();
                _ = ApplyOpitonAsync();
            }
        }

        private async Task ApplyOpitonAsync()
        {
            if (OptionPanel.IsOptionChanged)
            {
                await OptionPanel.ViewModel.SaveAsync();
                await PageRender.ReloadAsync();
            }
            //var tokenizer = ViewModel.Tokenizer;
            //if (OptionPanel.IsSizeChanged)
            //{
            //    PageRender.Setting = setting;
            //    ViewModel.Tokenizer.Left = ViewModel.Tokenizer.Right =
            //        ViewModel.Tokenizer.Top = ViewModel.Tokenizer.Bottom = setting.Padding;
            //    ViewModel.Tokenizer.LetterSpace = setting.LetterSpace;
            //    ViewModel.Tokenizer.LineSpace = setting.LineSpace;
            //    ViewModel.Tokenizer.Gap = setting.Padding * 2;
            //    await tokenizer.Refresh();
            //    ViewModel.Tokenizer.SetPage(ViewModel.Book.Position);
            //    PageRender.Flush();
            //    await PageRender.SwapToAsync(tokenizer.Page);
            //    return;
            //}
            if (OptionPanel.IsLayerChanged)
            {
                // PageRender.Setting = setting;
            }
            
        }

        private async void ChapterListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = ChapterListBox.SelectedIndex;
            if (i < -1)
            {
                return;
            }
            MorePanel.Visibility = Visibility.Collapsed;
            ViewModel.GotoChapter(i);
            await PageRender.ReloadAsync();
        }

    }
}
