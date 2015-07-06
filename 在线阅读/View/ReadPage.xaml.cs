using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using 在线阅读.Class;
using 在线阅读.Models;

namespace 在线阅读.View
{
    /// <summary>
    /// Interaction logic for ReadPage.xaml
    /// </summary>
    public partial class ReadPage : Page
    {
        /// <summary>
        /// 阅读
        /// </summary>
        public ReadPage()
        {
            InitializeComponent();
        }

        private UserMethod _method;

        private void TitleTb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UrlTb.Opacity = 1;
            UrlTb.Focus();
        }

        private void UrlTb_LostFocus(object sender, RoutedEventArgs e)
        {
            UrlTb.Opacity = 0;
        }

        private void BeforeBtn_Click(object sender, RoutedEventArgs e)
        {
            GoBefore();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            GoNext();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int index = ChapterListBox.SelectedIndex;
            MenuItem item = (MenuItem)sender;
            switch (item.Header.ToString())
            {
                case "设置":
                    SetWindow set = new SetWindow();
                    if (set.ShowDialog() == true)
                    {
                        LoadSet();
                    }
                    break;
                case "下载本章":
                    if (index >= 0 && index < Model.Books.Count)
                    {
                        _method.DownBook(index, 1);
                    }
                    break;
                case "下载后续章节":
                    if (index >= 0 && index < Model.Books.Count)
                    {
                        _method.DownBook(index, Model.Books.Count - index);
                    }
                    break;
                case "下载全部章节":
                    if (Model.Books.Count > 0)
                    {
                        _method.DownBook(0, Model.Books.Count);
                    }
                    break;
                case "复制":
                    Clipboard.SetData(DataFormats.Text, ContentTb.SelectedText);//复制内容到剪切板
                    break;
                case "隐藏列表":
                    ListGrid.Width = 0;
                    item.Header = "显示列表";
                    break;
                case "显示列表":
                    ListGrid.Width = 200;
                    item.Header = "隐藏列表";
                    break;
                case "返回":
                    if (NavigationService != null) NavigationService.GoBack();
                    break;
            }
        }

        private void ContentTb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left)
            {
                GoBefore();
            }
            if (e.Key == Key.W || e.Key == Key.Up)
            {
                ContentTb.LineDown();
                ContentTb.PageUp();
            }
            if (e.Key == Key.D || e.Key == Key.Right)
            {
                GoNext();
            }
            if (e.Key == Key.S || e.Key == Key.Down)
            {
                ContentTb.LineUp();         //换页时留一行
                ContentTb.PageDown();
            }
        }

        private void UrlTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            UrlTb.Opacity = 0;
            string url = UrlTb.Text;
            if (url.LastIndexOf("index", System.StringComparison.Ordinal) <= 0 && url[url.Length - 1] != '/')
            {
                string tem = url.Substring(0, url.LastIndexOf("/", System.StringComparison.Ordinal) + 1);
                url = tem;
            }
            Model.BookModels[0].FileName = url;
            Model.BookModels[0].Index = 0;
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _method = new UserMethod();
            Model.Kind = _method.GetKind(Model.BookModels[0].FileName);
            if (Model.Kind)
            {
                UrlTb.Text = Model.BookModels[0].FileName;
            }
            _method.GetRegex(_method.LoadRegex());
            LoadSet();
            LoadChapter();
            LoadContent();
        }

        private void ChapterListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Model.BookModels[0].Index = ChapterListBox.SelectedIndex;
            LoadContent();
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        private void LoadSet()
        {
            string[] body = _method.LoadBody();

            ContentTb.FontSize = int.Parse(body[1]);
            ContentTb.FontFamily = new FontFamily(body[0]);
            ContentTb.Foreground = new SolidColorBrush(_method.ToColor(body[2])); ;
            ContentTb.Background = new SolidColorBrush(_method.ToColor(body[3]));

        }

        /// <summary>
        /// 上一章
        /// </summary>
        private void GoBefore()
        {
            if (Model.BookModels[0].Index > 0)
            {
                Model.BookModels[0].Index--;
                LoadContent();
            }
            else
            {
                ContentTb.Text = "已到最前！！";
            }
            ContentTb.ScrollToHome();
        }

        /// <summary>
        /// 下一章
        /// </summary>
        private void GoNext()
        {
            if (Model.BookModels[0].Index < Model.Books.Count - 1)
            {
                Model.BookModels[0].Index++;
                LoadContent();
            }
            else
            {
                ContentTb.Text = "已到最后！！";
            }
            ContentTb.ScrollToHome();
        }
        /// <summary>
        /// 加载内容
        /// </summary>
        private void LoadContent()
        {
            if (Model.Books.Count <= Model.BookModels[0].Index)
            {
                return;
            }
            Book book = Model.Books[Model.BookModels[0].Index];
            TitleTb.Text = book.Name;
            if (!Model.Kind)
            {
                UrlTb.Text = book.Url;
            }
            ContentTb.Text = _method.GetContent();
            ChapterListBox.SelectedIndex = Model.BookModels[0].Index;
           
        }
        
        /// <summary>
        /// 加载目录
        /// </summary>
        private void LoadChapter()
        {
            if (Model.BookModels[0].FileName == "") return;
            _method.GetChapter();
            NameLb.Text = Model.BookModels[0].Name;
            ChapterListBox.ItemsSource = null;
            ChapterListBox.ItemsSource = Model.Books;
            ChapterListBox.DisplayMemberPath = "Name";
        }



        /*
         *  public delegate void NextPrimeDelegate();//定义委托
            public void CheckNextNumber()//执行这个算法，并且更新文本框内容
            {.................
            }
            private void startStopButton_Click(object sender, RoutedEventArgs e)
            {
                startStopButton/this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,new NextPrimeDelegate(CheckNextNumber));
            }
         */


    }
}
