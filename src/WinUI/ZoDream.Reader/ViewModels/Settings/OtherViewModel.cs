﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using ZoDream.Reader.Dialogs;
using ZoDream.Reader.Utils;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class OtherViewModel: BindableBase
    {

        public OtherViewModel()
        {
            ClearCommand = new RelayCommand(TapClear);
        }

        public ICommand ClearCommand { get; private set; }

        private async void TapClear(object? _)
        {
            var app = App.GetService<AppViewModel>();
            //var dialog = new MessageDialog("确定要删除文件");
            //app.InitializePicker(dialog);
            if (!await app.ConfirmAsync("确定要删除文件"))
            {
                return;
            }
            await app.InitializeWorkspaceAsync();
            Toast.Show("清除数据完成");
        }
    }
}
