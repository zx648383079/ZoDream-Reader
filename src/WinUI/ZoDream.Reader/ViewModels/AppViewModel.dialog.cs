using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using WinRT.Interop;
using ZoDream.Reader.Dialogs;

namespace ZoDream.Reader.ViewModels
{
    public partial class AppViewModel
    {
        public XamlRoot BaseXamlRoot => _baseWindow!.Content.XamlRoot;

        public WindowId AppWindowId => BaseXamlRoot.ContentIslandEnvironment.AppWindowId;

        public async Task<bool> ConfirmAsync(string message, string title = "提示")
        {
            var dialog = new ConfirmDialog
            {
                Title = title,
                Content = message
            };
            return await OpenDialogAsync(dialog) == ContentDialogResult.Primary;
        }

        public void ToastAsync(string text)
        {
            var notification = new AppNotificationBuilder()
                .AddText(text)
                .BuildNotification();
            AppNotificationManager.Default.Show(notification);
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
