using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI;
using WinRT.Interop;
using ZoDream.Reader.Controls;
using ZoDream.Reader.Dialogs;
using ZoDream.Reader.Pages;
using ZoDream.Reader.Repositories;

namespace ZoDream.Reader.ViewModels
{
    public partial class AppViewModel
    {
        public XamlRoot BaseXamlRoot => _baseWindow!.Content.XamlRoot;

        public void InitializePicker(object target)
        {
            InitializeWithWindow.Initialize(target, _baseWindowHandle);
        }

        public async Task<bool> ConfirmAsync(string message, string title = "提示")
        {
            var dialog = new ConfirmDialog
            {
                Title = title,
                Content = message
            };
            return await OpenDialogAsync(dialog) == ContentDialogResult.Primary;
        }

        public IAsyncOperation<ContentDialogResult> OpenDialogAsync(ContentDialog target)
        {
            target.XamlRoot = BaseXamlRoot;
            return target.ShowAsync();
        }

        public async Task<bool> OpenFormAsync(ContentDialog target)
        {
            target.XamlRoot = BaseXamlRoot;
            if (target.DataContext is not IFormValidator model)
            {
                return await target.ShowAsync() == ContentDialogResult.Primary;
            }
            while (true)
            {
                var res = await target.ShowAsync();
                if (res != ContentDialogResult.Primary)
                {
                    break;
                }
                if (model.IsValid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
