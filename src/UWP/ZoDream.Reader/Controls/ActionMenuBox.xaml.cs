using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Events;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ZoDream.Reader.Controls
{
    public sealed partial class ActionMenuBox : UserControl
    {
        public ActionMenuBox()
        {
            this.InitializeComponent();
        }

        public event ActionEventHandler OnAction;
        private void EditBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnAction?.Invoke(this, ActionEvent.EDIT);
            MenuBox.Visibility = Visibility.Collapsed;
        }

        private void RemoveBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnAction?.Invoke(this, ActionEvent.DELETE);
            MenuBox.Visibility = Visibility.Collapsed;
        }

        public void Show(Point position)
        {
            Canvas.SetLeft(MenuBox, position.X);
            Canvas.SetTop(MenuBox, position.Y);
            MenuBox.Visibility = Visibility.Visible;
        }
    }
}
