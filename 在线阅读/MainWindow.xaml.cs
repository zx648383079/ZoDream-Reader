using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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

        private UserMethod _method;

        private void BeforeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Model.index>0)
            {
                Model.index --;
                Get();
            }
            else
            {
                ContenLb.Text = "已到最前！！";
            }
            
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Model.index<Model.Books.Count-1)
            {
                Model.index++;
                Get();
            }
            else
            {
                ContenLb.Text = "已到最后！！";
            }

            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _method = new UserMethod();
            LoadSet();
            Model.Url = ConfigurationSettings.AppSettings["Url"];
            Model.index = int.Parse(ConfigurationSettings.AppSettings["index"]);
            if (Model.Url == "") return;
            GetBook();
            Get();
        }

        private void GetBook()
        {
            if (Model.Url == "") return;
            _method.GetRegex(_method.LoadRegex());
            NameLb.Text = _method.GetList();
            BookList.ItemsSource = null;
            BookList.ItemsSource = Model.Books;
            BookList.DisplayMemberPath = "Name";
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        private void Get()
        {
            Book book = Model.Books[Model.index];
            UrlTb.Text =book.Url ;
            TitleLb.Text = book.Name;
            ContenLb.Text = _method.GetContent(book.Url);
            BookList.SelectedIndex = Model.index;
        }

        private void UrlTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key ==Key.Enter)
            {
                UrlTb.Opacity = 0;
                string url = UrlTb.Text;
                if (url.LastIndexOf("index", System.StringComparison.Ordinal) <= 0) return;
                Model.Url = url;
                Model.index = 0;
                GetBook();
                Get();
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int index = BookList.SelectedIndex;
            MenuItem item = (MenuItem) sender;
            switch (item.Header.ToString())
            {
                case "设置":
                    SetWindow set = new SetWindow();
                    if (set.ShowDialog()==true)
                    {
                        LoadSet();
                    }
                    break;
                case "下载本章":
                    if (index>=0 && index<Model.Books.Count)
                    {
                        _method.DownBook(BookList.SelectedIndex, 1);
                    }
                    break;
                case "下载后续章节":
                    if (index >= 0 && index < Model.Books.Count)
                    {
                        _method.DownBook(BookList.SelectedIndex, Model.Books.Count - index);
                    }
                    break;
                case "下载全部章节":
                    if (Model.Books.Count>0)
                    {
                        _method.DownBook(0, Model.Books.Count);
                    }
                    break;
            }
        }

        private void LoadSet()
        {
            ContenLb.FontSize = int.Parse(ConfigurationSettings.AppSettings["FontSize"]);
            ContenLb.FontFamily = new FontFamily(ConfigurationSettings.AppSettings["Font"]);
            ContenLb.Foreground = new SolidColorBrush(_method.ToColor(ConfigurationSettings.AppSettings["FontColor"])); ;
            ContenScroll.Background = new SolidColorBrush(_method.ToColor(ConfigurationSettings.AppSettings["Background"]));
            
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UrlTb.Opacity = 1;
            UrlTb.Focus();
        }

        private void UrlTb_LostFocus(object sender, RoutedEventArgs e)
        {
            UrlTb.Opacity = 0;
        }

        private void BookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = BookList.SelectedIndex;
            if (index < 0 || index >= Model.Books.Count) return;
            Model.index = index;
            Get();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //修改App.Config文件，需要删除vshost.exe.config后才能生效
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Url"].Value = Model.Url;
            config.AppSettings.Settings["index"].Value = Model.index.ToString(CultureInfo.InvariantCulture);
            config.Save();
        }


    }
}
