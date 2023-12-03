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
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Dialogs
{
    public sealed partial class AddNovelDialog : ContentDialog
    {
        public AddNovelDialog()
        {
            this.InitializeComponent();
        }

        public int SelectedIndex { get; private set; }

        private void LocalBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 0;
            Hide();
        }

        private void NetBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 1;
            Hide();
        }
    }
}
