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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Events;
using ZoDream.Shared.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ZoDream.Reader.Controls
{
    public sealed partial class BookListBoxItem : UserControl
    {
        public BookListBoxItem()
        {
            this.InitializeComponent();
        }

        public event ActionItemEventHandler OnAction;

        public BookItem Source
        {
            get { return (BookItem)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(BookItem), typeof(BookListBoxItem), new PropertyMetadata(null, OnSourceChange));

        private static void OnSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BookListBoxItem).RefreshSource();
        }

        private void RefreshSource()
        {
            CoverImg.Source = Utils.Converter.ToImg(Source?.Cover);
            NameTb.Text = Source == null || string.IsNullOrWhiteSpace(Source.Name) ? "[未知]" : Source.Name;
        }


        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.EDIT);
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.DELETE);
        }

        private void MainBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.CLICK);
        }

        private void MoreBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.NONE);
            e.Handled = true;
        }
    }
}
