using Microsoft.UI.Xaml.Controls;
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
            ViewModel.Document = new TextBoxEditor(PART_TextBox);
        }

        public CreateNovelViewModel ViewModel => (CreateNovelViewModel)DataContext;

        private void FindKeyboard_Invoked(Microsoft.UI.Xaml.Input.KeyboardAccelerator sender, Microsoft.UI.Xaml.Input.KeyboardAcceleratorInvokedEventArgs args)
        {
            ViewModel.FindOpen = true;
            ViewModel.FindText = PART_TextBox.SelectedText;
        }
    }
}
