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
                    NameTb.Text = "";
                    ChapterTb.Text = "";
                    ContentTb.Text = "";
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
            NameTb.Text = regex.Name;
            ChapterTb.Text = regex.Chapter;
            ContentTb.Text = regex.Content;
        }

        private void SavaBtn_Click(object sender, RoutedEventArgs e)
        {
            UserMethod method = new UserMethod();
            string web = method.GetWeb(WebTb.Text);
            BookRegex regex = new BookRegex(web, NameTb.Text, ChapterTb.Text, ContentTb.Text);
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
