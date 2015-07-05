/*
 * 修改程序的字体背景颜色
 */

using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using 在线阅读.Class;

namespace 在线阅读.View
{
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : Page
    {
        public SystemPage()
        {
            InitializeComponent();
        }

        private UserMethod _method;
        

        private void FontColorTb_LostFocus(object sender, RoutedEventArgs e)
        {
            ColorRe.Fill = new SolidColorBrush(_method.ToColor(FontColorTb.Text));
        }

        private void BackgroundTb_LostFocus(object sender, RoutedEventArgs e)
        {
            BackgroundRe.Fill = new SolidColorBrush(_method.ToColor(BackgroundTb.Text));
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //添加字体
            foreach (FontFamily _f in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary _fontDic = _f.FamilyNames;
                if (_fontDic.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    string _fontName = null;
                    if (_fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out _fontName))
                    {
                        FontComboBox.Items.Add(_fontName);
                    }
                }
            }


            //FontComboBox.Text = ConfigurationSettings.AppSettings["Font"];
            //FontSizeTb.Text = ConfigurationSettings.AppSettings["FontSize"];
            //FontColorTb.Text = ConfigurationSettings.AppSettings["FontColor"];
            //BackgroundTb.Text = ConfigurationSettings.AppSettings["Background"];

            _method = new UserMethod();


            string[] body = _method.LoadBody();

            FontSizeTb.Text =body[1];
            FontComboBox.Text = body[0];
            FontColorTb.Text =body[2];
            BackgroundTb.Text = body[3];

            ColorRe.Fill = new SolidColorBrush(_method.ToColor(FontColorTb.Text));
            BackgroundRe.Fill = new SolidColorBrush(_method.ToColor(BackgroundTb.Text));


        }
    }
}
