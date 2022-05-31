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
        }

        public SettingViewModel ViewModel = new();

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

        }
    }
}
