using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// SettingPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPanel : UserControl
    {
        public SettingPanel()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.PropertyChanged += (_, e) =>
            {
                IsOptionChanged = true;
                if (e.PropertyName == nameof(ViewModel.FontSize) ||
                e.PropertyName == nameof(ViewModel.LineSpace) ||
                e.PropertyName == nameof(ViewModel.LetterSpace) ||
                e.PropertyName == nameof(ViewModel.Padding) )
                {
                    IsSizeChanged = true;
                    return;
                }
                if (e.PropertyName == nameof(ViewModel.OpenSpeak) || 
                e.PropertyName == nameof(ViewModel.AutoFlip) || 
                e.PropertyName == nameof(ViewModel.FlipSpace) ||
                e.PropertyName == nameof(ViewModel.SpeakSpeed))
                {
                    return;
                }
                IsLayerChanged = true;
            };
        }

        public SettingViewModel ViewModel = new();

        public bool IsSizeChanged { get; private set; } = false;
        public bool IsLayerChanged { get; private set; } = false;
        public bool IsOptionChanged { get; private set; } = false;

        public void Show()
        {
            Visibility = Visibility.Visible;
            IsOptionChanged = IsSizeChanged = IsLayerChanged = false;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;
        }

        private void openFontBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择文件",
                RestoreDirectory = true,
                Filter = "ttf|*.ttf|所有文件|*.*"
            };
            if (picker.ShowDialog() != true)
            {
                return;
            }
            AddFont(picker.FileName);
        }

        private async void AddFont(string font)
        {
            //var item = await App.ViewModel.DiskRepository.AddFontAsync(font);
            //if (item == null)
            //{
            //    return;
            //}
            //ViewModel.FontItems.Add(item);
            //ViewModel.FontFamily = item.FontFamily;
        }
    }
}
