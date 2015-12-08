using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using 在线阅读.Class;
using 在线阅读.Models;

namespace 在线阅读.View
{
    /// <summary>
    /// Interaction logic for SetWindow.xaml
    /// </summary>
    public partial class SetWindow : Window
    {
        public SetWindow()
        {
            InitializeComponent();
        }

        private SystemPage _systemPage;

        private RegexPage _regexPage;

        public List<BookRegex> BookRegexs;

        private UserMethod _method;


        private void SavaBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (_regexPage != null)
            {
                _method.SavaRegex(BookRegexs);
            }
            

            if (_systemPage != null)
            {
                //修改App.Config文件，需要删除vshost.exe.config后才能生效
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Font"].Value = _systemPage.FontComboBox.Text;
                config.AppSettings.Settings["FontSize"].Value = _systemPage.FontSizeTb.Text;
                config.AppSettings.Settings["FontColor"].Value = _systemPage.FontColorTb.Text;
                config.AppSettings.Settings["Background"].Value = _systemPage.BackgroundTb.Text;
                config.Save();
                result = true;
            }

            this.DialogResult = result;

        }


        private void MenuList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = MenuList.SelectedIndex;
            switch (index)
            {
                case 0:
                    _systemPage = new SystemPage();
                    PageFrame.Navigate(_systemPage);
                    break;
                case 1:
                    _regexPage = new RegexPage();
                    
                    _method = new UserMethod();
                    BookRegexs = _method.LoadRegex();
                    _regexPage.Set = this;
                    PageFrame.Navigate(_regexPage);
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _method = new UserMethod();
            BookRegexs = _method.LoadRegex();
        }
    }
}
