using Microsoft.UI.Xaml.Controls;
using ZoDream.Shared.Repositories.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Dialogs
{
    public sealed partial class AddDictionaryDialog : ContentDialog
    {
        public AddDictionaryDialog()
        {
            this.InitializeComponent();
        }

        public DictionaryRuleModel ViewModel => (DictionaryRuleModel)DataContext;
    }
}
