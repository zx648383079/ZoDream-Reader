using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    public sealed class ContentPanel : ContentControl
    {
        public ContentPanel()
        {
            this.DefaultStyleKey = typeof(ContentPanel);
        }



        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), 
                typeof(ContentPanel), new PropertyMetadata(false, (d, s) => {
                    if (d is not ContentPanel panel)
                    {
                        return;
                    }
                    panel.Visibility = (bool)s.NewValue ? Visibility.Visible : Visibility.Collapsed;
                }));




        public double PanelWidth {
            get { return (double)GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelWidthProperty =
            DependencyProperty.Register("PanelWidth", typeof(double), typeof(ContentPanel), new PropertyMetadata(200.0));




        public HorizontalAlignment Placement {
            get { return (HorizontalAlignment)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(HorizontalAlignment), typeof(ContentPanel), 
                new PropertyMetadata(HorizontalAlignment.Left));



        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ContentPanel), new PropertyMetadata(string.Empty));


        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (!IsOpen)
            {
                return;
            }
            var point = e.GetCurrentPoint(this);
            if (IsClickOutside(point.Position.X))
            {
                IsOpen = false;
            }
        }

        private bool IsClickOutside(double x)
        {
            return Placement switch
            {
                HorizontalAlignment.Center => Math.Abs(x - ActualWidth) > PanelWidth,
                HorizontalAlignment.Right => ActualWidth - PanelWidth > x,
                _ => x > PanelWidth
            };
        }
    }
}
