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
        public RegexPage()
        {
            InitializeComponent();
        }

        public SetWindow Set { get; set; }
        private int _index;
        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem) sender;
            switch (menu.Header.ToString())
            {
                case "删除":
                    _index = RegexListView.SelectedIndex;
                    int count = Set.BookRegexs.Count;
                    if (_index>=0 && _index<count)
                    {
                        Set.BookRegexs.RemoveAt(_index);
                        Refresh();
                    }
                    break;
                case "添加":
                    ScrollGrid.Opacity = 1;
                    WebTb.Text = "";
                    ChapterTb.Text = "";
                    ContentTb.Text = "";
                    ReplaceTb.Text = "";
                    _index = -1;
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
            _index = RegexListView.SelectedIndex;
            int count = Set.BookRegexs.Count;
            if (_index < 0 || _index >= count) return;
            ScrollGrid.Opacity = 1;
            BookRegex regex = Set.BookRegexs[_index];
            WebTb.Text = regex.Web;
            ChapterTb.Text = regex.Chapter;
            ContentTb.Text = regex.Content;
        }

        private void SavaBtn_Click(object sender, RoutedEventArgs e)
        {
            UserMethod method = new UserMethod();
            string web = method.GetWeb(WebTb.Text);
            BookRegex regex = new BookRegex(web,  ChapterTb.Text, ContentTb.Text,ReplaceTb.Text);
            if (_index>=0)
            {
                
                Set.BookRegexs[_index] =regex;
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
