using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Controls
{
    public sealed class IconButton : ButtonBase
    {
        public IconButton()
        {
            this.DefaultStyleKey = typeof(IconButton);
        }

        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconButton), new PropertyMetadata(string.Empty));




        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(IconButton), new PropertyMetadata(string.Empty));



        public string Meta {
            get { return (string)GetValue(MetaProperty); }
            set { SetValue(MetaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Meta.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MetaProperty =
            DependencyProperty.Register("Meta", typeof(string), typeof(IconButton), new PropertyMetadata(string.Empty));




        public double IconFontSize {
            get { return (double)GetValue(IconFontSizeProperty); }
            set { SetValue(IconFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFontSizeProperty =
            DependencyProperty.Register("IconFontSize", typeof(double), typeof(IconButton), new PropertyMetadata(40.0));



        public double MetaFontSize {
            get { return (double)GetValue(MetaFontSizeProperty); }
            set { SetValue(MetaFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MetaFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MetaFontSizeProperty =
            DependencyProperty.Register("MetaFontSize", typeof(double), typeof(IconButton), new PropertyMetadata(12.0));


    }
}
