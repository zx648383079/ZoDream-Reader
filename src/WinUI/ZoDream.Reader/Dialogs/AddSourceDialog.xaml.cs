using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.Shared.Repositories.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Dialogs
{
    public sealed partial class AddSourceDialog : ContentDialog
    {
        public AddSourceDialog()
        {
            this.InitializeComponent();
        }

        private int Step = 0;

        private int TotalStep {
            get {
                var count = 2;
                if (ViewModel.EnabledSearch)
                {
                    count++;
                }
                if (ViewModel.EnabledExplore)
                {
                    count++;
                }
                return count;
            }
        }
        public SourceRuleModel ViewModel => (SourceRuleModel)DataContext;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Step--;
            if (Step < 0)
            {
                return;
            }
            OnChangeStep();
            args.Cancel = true;
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Step++;
            if (Step >= TotalStep)
            {
                return;
            }
            OnChangeStep();
            args.Cancel = true;
        }

        private void OnChangeStep()
        {
            switch (Step)
            {
                case 0:
                    Title = "编辑信息";
                    PrimaryButtonText = "取消";
                    SecondaryButtonText = "下一步";
                    FirstPanel.Visibility = Visibility.Visible; 
                    SecondPanel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    Title = "编辑内容规则";
                    PrimaryButtonText = "上一步";
                    SecondaryButtonText = "下一步";
                    FirstPanel.Visibility = Visibility.Collapsed;
                    SecondPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
