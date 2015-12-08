using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 在线阅读
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTb.Text = Model.TitleRegex;
            ContentTb.Text = Model.ContentRegex;
            BeforeTb.Text = Model.BeforeRegex;
            NextTb.Text = Model.NextRegex;
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
            FontComboBox.Text = Model.Font;

            FontSizeTb.Text = Model.FontSize.ToString(CultureInfo.InvariantCulture);

            FontColorTb.Text=Model.FontColor;
            FontColorTb.Foreground = new SolidColorBrush(Model.ToColor(FontColorTb.Text));
            BackgroundTb.Background = new SolidColorBrush(Model.ToColor(BackgroundTb.Text));
            BackgroundTb.Text=Model.Background;

        }

        private void SavaBtn_Click(object sender, RoutedEventArgs e)
        {
            Model.TitleRegex = TitleTb.Text;
            Model.ContentRegex = ContentTb.Text;
            Model.BeforeRegex = BeforeTb.Text;
            Model.NextRegex = NextTb.Text;
            
            Model.Font = FontComboBox.Text;
            Model.FontSize =int.Parse(FontSizeTb.Text.Trim());

            Model.FontColor = FontColorTb.Text;
            Model.Background = BackgroundTb.Text;

            this.Close();
        }

        private void FontColorTb_LostFocus(object sender, RoutedEventArgs e)
        {
            FontColorTb.Foreground = new SolidColorBrush(Model.ToColor(FontColorTb.Text));
        }

        private void BackgroudTb_LostFocus(object sender, RoutedEventArgs e)
        {
            BackgroundTb.Background = new SolidColorBrush(Model.ToColor(BackgroundTb.Text));
        }
    }
}
