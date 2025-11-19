using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ZoDream.Reader.ViewModels
{
    public class OtherViewModel: ObservableObject
    {

        public OtherViewModel()
        {
            ClearCommand = new RelayCommand(TapClear);
        }

        public ICommand ClearCommand { get; private set; }

        private async void TapClear()
        {
            var app = App.GetService<AppViewModel>();
            //var dialog = new MessageDialog("确定要删除文件");
            //app.InitializePicker(dialog);
            if (!await app.ConfirmAsync("确定要删除文件"))
            {
                return;
            }
            await app.InitializeWorkspaceAsync();
            app.ToastAsync("清除数据完成");
        }
    }
}
