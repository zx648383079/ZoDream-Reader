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
using ZoDream.Reader.Events;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// ActionMenuBox.xaml 的交互逻辑
    /// </summary>
    public partial class ActionMenuBox : UserControl
    {
        public ActionMenuBox()
        {
            InitializeComponent();
        }

        public event ActionEventHandler? OnAction;


        public void Show(Point position)
        {
            Canvas.SetLeft(MenuBox, position.X);
            Canvas.SetTop(MenuBox, position.Y);
            MenuBox.Visibility = Visibility.Visible;
        }

        public void Hide()
        {

            MenuBox.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.AddMouseDownHandler(App.Current.MainWindow, UserControl_MouseDown);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Mouse.RemoveMouseDownHandler(App.Current.MainWindow, UserControl_MouseDown);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MenuBox.Visibility == Visibility.Collapsed)
            {
                return;
            }
            MenuBox.Visibility = Visibility.Collapsed;
        }

        private void EditBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnAction?.Invoke(this, ActionEvent.EDIT);
            MenuBox.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }


        private void RemoveBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnAction?.Invoke(this, ActionEvent.DELETE);
            MenuBox.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }
    }
}
