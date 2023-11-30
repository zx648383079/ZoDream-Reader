using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = PickerName, Type = typeof(ColorPicker))]
    [TemplatePart(Name = FlyoutName, Type = typeof(Flyout))]
    [TemplatePart(Name = MainPanelName, Type = typeof(GridView))]
    [TemplatePart(Name = SecondaryPanelName, Type = typeof(GridView))]
    [TemplatePart(Name = ToggleBtnName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ConfirmBtnName, Type = typeof(ButtonBase))]
    public sealed class ColorInput : Control
    {
        const string PickerName = "PART_ColorPicker";
        const string FlyoutName = "PART_Flyout";
        const string MainPanelName = "PART_MainPanel";
        const string SecondaryPanelName = "PART_SecondaryPanel";
        const string ToggleBtnName = "PART_ToggleBtn";
        const string ConfirmBtnName = "PART_ConfirmBtn";
        public ColorInput()
        {
            this.DefaultStyleKey = typeof(ColorInput);
        }

        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ColorInput), new PropertyMetadata(string.Empty));


        public Color Color {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorInput), 
                new PropertyMetadata(Colors.White, (d, s) => {
                    if (d is ColorInput obj)
                    {
                        obj.ColorBrush = new SolidColorBrush((Color)s.NewValue);
                    }
                }));

        public SolidColorBrush ColorBrush {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(ColorInput), new PropertyMetadata(new SolidColorBrush(Colors.White)));




        public IList<SolidColorBrush> ItemsSource {
            get { return (IList<SolidColorBrush>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList<SolidColorBrush>), typeof(ColorInput), new PropertyMetadata(new List<SolidColorBrush>() {
                new(Colors.Red),
                new(Colors.White),
                new(Colors.Pink),
                new(Colors.Purple),
                new(Colors.DeepPink),
                new(Colors.Indigo),
                new(Colors.Blue),
                new(Colors.LightBlue),
                new(Colors.Cyan),
                new(Colors.Teal),
                new(Colors.Green),
                new(Colors.LightGreen),
                new(Colors.Lime),
                new(Colors.Yellow),
                new(Colors.Orange),
                new(Colors.DarkOrange),
                new(Colors.Brown),
                new(Colors.Gray),
                new(Colors.DarkGray),
            }));




        public event EventHandler<Color>? ColorChanged;

        private ColorPicker? Picker;
        private Flyout? PickerPopup;
        private GridView? MainPanel;
        private GridView? SecondaryPanel;
        private ButtonBase? ToggleBtn;
        private ButtonBase? ConfirmBtn;
        private bool _isInPicker = false;
        

        public bool IsInPicker {
            get => _isInPicker;
            set {
                _isInPicker = value;
                if (Picker != null)
                {
                    Picker.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
                if (MainPanel != null)
                {
                    MainPanel.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                }
                if (SecondaryPanel != null)
                {
                    SecondaryPanel.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                }
                if (ToggleBtn != null)
                {
                    ToggleBtn.Content = value ? "预制颜色" : "自定义颜色";
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Picker = GetTemplateChild(PickerName) as ColorPicker;
            PickerPopup = GetTemplateChild(FlyoutName) as Flyout;
            ConfirmBtn = GetTemplateChild(ConfirmBtnName) as ButtonBase;
            ToggleBtn = GetTemplateChild(ToggleBtnName) as ButtonBase;
            MainPanel = GetTemplateChild(MainPanelName) as GridView;
            SecondaryPanel = GetTemplateChild(SecondaryPanelName) as GridView;
            if (PickerPopup != null )
            {
                PickerPopup.Opened += PickerFlyout_Opened;
            }
            if (ToggleBtn != null )
            {
                ToggleBtn.Click += ToggleBtn_Click;
            }
            if (ConfirmBtn != null)
            {
                ConfirmBtn.Click += ConfirmBtn_Click;
            }
            if (MainPanel != null)
            {
                MainPanel.SelectionChanged += MainPanel_SelectionChanged;
            }
            UpdateSecondaryBar(ItemsSource.FirstOrDefault());
        }

        private void MainPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSecondaryBar((SolidColorBrush)e.AddedItems[0]);
        }

        private void UpdateSecondaryBar(SolidColorBrush? color)
        {
            if (color is null)
            {
                return;
            }
            UpdateSecondaryBar(color.Color);
        }

        private void UpdateSecondaryBar(Color color)
        {
            if (SecondaryPanel == null)
            {
                return;
            }
            var items = new List<SolidColorBrush>
            {
                new(color)
            };
            for (int i = 5; i > 0; i--)
            {
                items.Add(new SolidColorBrush(Color.FromArgb(color.A,
                    (byte)(color.R * i / 6),
                    (byte)(color.G * i / 6),
                    (byte)(color.B * i / 6))));
            }
            SecondaryPanel.ItemsSource = items;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var color = IsInPicker ? Picker?.Color : (SecondaryPanel?.SelectedItem as SolidColorBrush)?.Color;
            if (color is null)
            {
                return;
            }
            Color = (Color)color;
            ColorBrush = new SolidColorBrush(Color);
            ColorChanged?.Invoke(this, Color);
            PickerPopup?.Hide();
        }

        private void ToggleBtn_Click(object sender, RoutedEventArgs e)
        {
            IsInPicker = !IsInPicker;
        }


        private void PickerFlyout_Opened(object? sender, object e)
        {
            IsInPicker = false;
        }
    }
}
