using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ZoDream.Reader.Controls
{
    [TemplatePart(Name = "PART_CloseBtn", Type = typeof(Button))]
    public sealed class PanelDialog : ContentControl
    {
        public PanelDialog()
        {
            this.DefaultStyleKey = typeof(PanelDialog);
        }



        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PanelDialog), new PropertyMetadata(false, OnOpenChanged));

        private static void OnOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PanelDialog).Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed; 
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PanelDialog), new PropertyMetadata(string.Empty));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeBtn = GetTemplateChild("PART_CloseBtn") as Button;
            if (closeBtn != null)
            {
                closeBtn.Click += CloseBtn_Click;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}
