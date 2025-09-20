using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using ZoDream.Reader.Controls;
using ZoDream.Reader.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Pages.Creators
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateNovelPage : Page
    {
        public CreateNovelPage()
        {
            InitializeComponent();
            ViewModel.Document = new RichTextBoxEditor(PART_TextBox);
        }

        public CreateNovelViewModel ViewModel => (CreateNovelViewModel)DataContext;

        private void FindKeyboard_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            ViewModel.FindOpen = true;
            ViewModel.FindText = ViewModel.Document!.SelectedText;
        }

        private void TextBox_TextChanged(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OnTextChanged();
        }

        private void TextBox_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var control = (RichEditBox)sender;
            control.SelectionFlyout.Opening += ContextMenu_Opening;
            control.ContextFlyout.Opening += ContextMenu_Opening;
            control.Document.UndoLimit = 20;
        }



        private void TextBox_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var control = (RichEditBox)sender;
            control.SelectionFlyout.Opening -= ContextMenu_Opening;
            control.ContextFlyout.Opening -= ContextMenu_Opening;
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.EnterCommand.Execute(null);
                return;
            }
            //if (e.Key == Windows.System.VirtualKey.V &&
            //    (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(
            //        Windows.System.VirtualKey.Control)
            //    & Windows.UI.Core.CoreVirtualKeyStates.Down) != 0)
            //{
            //    e.Handled = true;
            //    PasteWithoutFormatting();
            //    return;
            //}
        }

        private void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            e.Handled = true;
            PasteWithoutFormatting();
        }

        /// <summary>
        /// 无格式粘贴
        /// </summary>
        /// <param name="richEditBox"></param>
        /// <returns></returns>
        private async void PasteWithoutFormatting()
        {
            var clipboard = Clipboard.GetContent();
            if (clipboard.Contains(StandardDataFormats.Text))
            {
                var plainText = await clipboard.GetTextAsync();
                ViewModel.Document?.Paste(plainText);
            }
        }

        private void ContextMenu_Opening(object? sender, object e)
        {
            if (sender is not CommandBarFlyout myFlyout || myFlyout.Target != PART_TextBox)
            {
                return;
            }
            myFlyout.PrimaryCommands.Clear();
            myFlyout.PrimaryCommands.Add(CreateCommand("添加引号", "\uE9B2", ViewModel.QuoteCommand));
            myFlyout.PrimaryCommands.Add(CreateCommand("换行", "\uE751", ViewModel.EnterCommand));
            myFlyout.PrimaryCommands.Add(CreateCommand("拆分章节", "\uE736", ViewModel.SplitCommand));
            myFlyout.PrimaryCommands.Add(CreateCommand("查找字符", "\uE721", ViewModel.FindCommand));
        }

        private AppBarButton CreateCommand(string label, string icon, ICommand command)
        {
            return new AppBarButton()
            {
                Icon = new FontIcon()
                {
                    Glyph = icon
                },
                Label = label,
                Command = command,
            };
        }

      
    }
}
