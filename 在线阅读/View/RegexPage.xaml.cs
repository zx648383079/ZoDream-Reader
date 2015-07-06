using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using 在线阅读.Class;
using 在线阅读.Models;

namespace 在线阅读.View
{
    /// <summary>
    /// Interaction logic for RegexPage.xaml
    /// </summary>
    public partial class RegexPage : Page
    {
        /// <summary>
        /// 填写正则
        /// </summary>
        public RegexPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置窗体
        /// </summary>
        public SetWindow Set { get; set; }
        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem) sender;
            switch (menu.Header.ToString())
            {
                case "删除":
                    
                    int index = RegexListView.SelectedIndex;
                    int count = Set.BookRegexs.Count;
                    if (index>=0 && index<count)
                    {
                        Set.BookRegexs.RemoveAt(index);
                        Refresh();
                    }
                    break;
                case "添加":
                    ScrollGrid.Opacity = 1;
                    WebTb.Text = "";
                    ChapterTb.Text = "";
                    ContentTb.Text = "";
                    ReplaceTb.Text = "";
                    break;
            }
        }

        private void Refresh()
        {
            RegexListView.ItemsSource = null;
            RegexListView.ItemsSource = Set.BookRegexs;
        }

        private void RegexListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            int index = RegexListView.SelectedIndex;
            int count = Set.BookRegexs.Count;
            if (index < 0 || index >= count) return;
            ScrollGrid.Opacity = 1;
            BookRegex regex = Set.BookRegexs[index];
            WebTb.Text = regex.Web;
            ChapterTb.Text = regex.Chapter;
            ContentTb.Text = regex.Content;
            ReplaceTb.Text = regex.Replace;
        }

        private void SavaBtn_Click(object sender, RoutedEventArgs e)
        {
            UserMethod method = new UserMethod();
            string web = method.GetWeb(WebTb.Text);
            BookRegex regex = new BookRegex(web,  ChapterTb.Text, ContentTb.Text,ReplaceTb.Text);
            int index = -1;
            for (int i = 0; i < Set.BookRegexs.Count; i++)
            {
                BookRegex item = Set.BookRegexs[i];
                if (item.Web != web) continue;
                index = i;
                break;
            }

            if (index>=0)
            {
                Set.BookRegexs[index] = regex;
            }
            else
            {
                Set.BookRegexs.Add(regex);
            }

            Refresh();
            ScrollGrid.Opacity = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
