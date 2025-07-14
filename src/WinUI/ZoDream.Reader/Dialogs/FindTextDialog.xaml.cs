using Microsoft.UI.Xaml.Controls;
using ZoDream.Reader.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Dialogs
{
    public sealed partial class FindTextDialog : ContentDialog
    {
        public FindTextDialog()
        {
            InitializeComponent();
        }

        public FindTextDialogViewModel ViewModel => (FindTextDialogViewModel)DataContext;
    }
}
