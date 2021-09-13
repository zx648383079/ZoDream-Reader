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
    /// SwitchInput.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchInput : UserControl
    {
        public SwitchInput()
        {
            InitializeComponent();
        }



        public new double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(SwitchInput), new PropertyMetadata(30.0, OnSizeChanged));

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as SwitchInput;
            var size = (double)e.NewValue;
            var padding = Math.Floor((size - 2) / 5);
            tb.BorderBg.Width = size * 2 - 1;
            tb.BorderBg.Padding = new Thickness(padding);
            tb.BorderBg.Height = size;
            tb.CircleBtn.Width = tb.CircleBtn.Height = size - 2 * padding - 2;
        }

        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool), typeof(SwitchInput), new PropertyMetadata(false, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as SwitchInput;
            if ((bool)e.NewValue)
            {
                var color = new SolidColorBrush(Color.FromArgb(255, 0, 105, 186));
                tb.LabelTb.Text = tb.OnLabel;
                tb.BorderBg.BorderBrush = color;
                tb.BorderBg.Background = color;
                tb.CircleBtn.Fill = new SolidColorBrush(Colors.White);
                tb.CircleBtn.HorizontalAlignment = HorizontalAlignment.Right;
            } else
            {
                var color = new SolidColorBrush(Color.FromArgb(255, 90, 90, 90));
                tb.LabelTb.Text = tb.OffLabel;
                tb.BorderBg.BorderBrush = color;
                tb.BorderBg.Background = new SolidColorBrush(Colors.Transparent);
                tb.CircleBtn.Fill = color;
                tb.CircleBtn.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        public string OffLabel
        {
            get { return (string)GetValue(OffLabelProperty); }
            set { SetValue(OffLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffLabelProperty =
            DependencyProperty.Register("OffLabel", typeof(string), typeof(SwitchInput), new PropertyMetadata(string.Empty));



        public string OnLabel
        {
            get { return (string)GetValue(OnLabelProperty); }
            set { SetValue(OnLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnLabelProperty =
            DependencyProperty.Register("OnLabel", typeof(string), typeof(SwitchInput), new PropertyMetadata(string.Empty));

        public event ValueChangedEventHandler<bool>? ValueChanged;

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Value = !Value;
            ValueChanged?.Invoke(this, Value);
        }
    }
}
