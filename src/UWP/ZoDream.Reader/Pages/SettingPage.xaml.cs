using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        public SettingViewModel ViewModel = new SettingViewModel();

        public UserSetting Setting
        {
            get
            {
                var model = new UserSetting()
                {
                    LineSpace = (int)lineTb.Value,
                    LetterSpace = (int)letterTb.Value,
                    FontSize = (int)sizeTb.Value,
                    Padding = (int)paddingTb.Value,
                    IsSimple = !simpleTb.IsOn,
                    AutoFlip = flipTb.IsOn,
                    FlipSpace = flipSpaceTb.Value,
                    Animation = animateListBox.SelectedIndex,
                    OpenSpeek = speekTb.IsOn,
                    SpeekSpeed = speekSpeedTb.Value,
                };
                if (fontTb.SelectedItem != null)
                {
                    model.FontFamily = fontTb.SelectedItem.ToString();
                }
                return model;
            }
            set
            {
                lineTb.Value = value.LineSpace;
                letterTb.Value = value.LetterSpace;
                sizeTb.Value = value.FontSize;
                paddingTb.Value = value.Padding;
                if (!string.IsNullOrWhiteSpace(value.FontFamily))
                {
                    fontTb.SelectedItem = new FontItem(value.FontFamily);
                }
                simpleTb.IsOn = !value.IsSimple;
                flipTb.IsOn = value.AutoFlip;
                flipSpaceTb.Value = value.FlipSpace;
                animateListBox.SelectedIndex = value.Animation;
                speekTb.IsOn = value.OpenSpeek;
                speekSpeedTb.Value = value.SpeekSpeed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.ViewModel.Setting = Setting;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Setting = App.ViewModel.Setting;
        }

        private async void openFontBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".ttf");
            var file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            var font = await App.ViewModel.DiskRepository.AddFontAsync(file);
            ViewModel.FontItems.Add(font);
            fontTb.SelectedItem = font;
        }
    }
}
