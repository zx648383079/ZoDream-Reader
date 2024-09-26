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
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;

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



        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(BookListBoxItem), new PropertyMetadata(null));



        public INovel Source
        {
            get { return (INovel)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(INovel), typeof(BookListBoxItem), new PropertyMetadata(null, OnSourceChange));

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
            Command?.Execute(new ActionHanlderArgs(Source, ActionEvent.NONE));
            e.Handled = true;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Command?.Execute(new ActionHanlderArgs(Source, ActionEvent.EDIT));
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Command?.Execute(new ActionHanlderArgs(Source, ActionEvent.DELETE));
        }

        private void MainBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Command?.Execute(new ActionHanlderArgs(Source, ActionEvent.CLICK));
        }
    }
}
