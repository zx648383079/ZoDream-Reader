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

namespace 在线阅读
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }

        private void BeforeBtn_Click(object sender, RoutedEventArgs e)
        {
            UrlTb.Text = Model.BeforeUrl;
            Get();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            UrlTb.Text = Model.NextUrl;
            Get();
        }

        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            SetWindow set = new SetWindow();
            set.ShowDialog();
            ContenLb.FontSize = Model.FontSize;
            ContenLb.FontFamily = new FontFamily(Model.Font);
            ContenLb.Foreground = new SolidColorBrush(Model.ToColor(Model.FontColor));
            ContenScroll.Background = new SolidColorBrush(Model.ToColor(Model.Background));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Model.OpenSet();
            ContenLb.FontSize = Model.FontSize;
            ContenLb.FontFamily = new FontFamily(Model.Font);
            ContenLb.Foreground = new SolidColorBrush(Model.ToColor(Model.FontColor));
            ContenScroll.Background = new SolidColorBrush(Model.ToColor(Model.Background));
            UrlTb.Text = Model.Url;
            Get();
        }

        private void Get()
        {
            if (UrlTb.Text.Trim() == "" || Model.TitleRegex == "") return;
            if (!Model.Reader(UrlTb.Text.Trim())) return;
            TitleLb.Text = Model.Title;
            ContenLb.Text = Model.Content;
            ContenScroll.ScrollToHome();
        }

        private void UrlTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key ==Key.Enter)
            {
                Get();
            }
        }

        private void ContenLb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key ==Key.Left)
            {
                ContenScroll.PageUp();
            }
            if (e.Key ==Key.Right)
            {
                ContenScroll.PageDown();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.SavaSet();
        }
    }
}
