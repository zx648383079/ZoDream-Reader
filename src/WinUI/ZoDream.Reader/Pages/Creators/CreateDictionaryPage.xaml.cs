using Microsoft.UI.Xaml.Controls;
using ZoDream.Reader.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Pages.Creators
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateDictionaryPage : Page
    {
        public CreateDictionaryPage()
        {
            this.InitializeComponent();
            ViewModel.Editor = Editor;
        }

        public CreateDictionaryViewModel ViewModel => (CreateDictionaryViewModel)DataContext;
    }
}
