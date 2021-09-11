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
using ZoDream.Reader.Models;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// BookListBoxItem.xaml 的交互逻辑
    /// </summary>
    public partial class BookListBoxItem : UserControl
    {
        public BookListBoxItem()
        {
            InitializeComponent();
        }

        public event ActionItemEventHandler? OnAction;

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


        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            MoreMenu.IsOpen = !MoreMenu.IsOpen;
            e.Handled = true;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.EDIT);
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.DELETE);
        }

        private void MainBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnAction?.Invoke(this, Source, ActionEvent.CLICK);
        }
    }
}
