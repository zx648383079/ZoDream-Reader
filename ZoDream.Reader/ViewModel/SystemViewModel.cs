using System;
using System.Drawing;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SystemViewModel : ViewModelBase
    {
        private NotificationMessageAction _system;
        /// <summary>
        /// Initializes a new instance of the SystemViewModel class.
        /// </summary>
        public SystemViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction>(this, "system", m =>
            {
                _system = m;
            });
            _loadFont();
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                SystemHelper.Open();
                Background = SystemHelper.Get("Background");
                FontFamily = new FontFamily(SystemHelper.Get("FontFamily", "宋体"));
                FontSize = SystemHelper.GetInt("FontSize");
                FontWeight = SystemHelper.GetInt("FontWeight");
                Foreground = SystemHelper.Get("Foreground");
                DatabaseHelper.Close();
            });
        }

        private void _loadFont()
        {
            System.Drawing.Text.InstalledFontCollection font = new System.Drawing.Text.InstalledFontCollection();
            FontFamilys = font.Families;
        }

        /// <summary>
        /// The <see cref="FontFamilys" /> property's name.
        /// </summary>
        public const string FontFamilysPropertyName = "FontFamilys";

        private Array _fontFamilys;

        /// <summary>
        /// Sets and gets the FontFamilys property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Array FontFamilys
        {
            get
            {
                return _fontFamilys;
            }
            set
            {
                Set(FontFamilysPropertyName, ref _fontFamilys, value);
            }
        }

        /// <summary>
        /// The <see cref="Background" /> property's name.
        /// </summary>
        public const string BackgroundPropertyName = "Background";

        private string _background = "#fff";

        /// <summary>
        /// Sets and gets the Background property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Background
        {
            get
            {
                return _background;
            }
            set
            {
                Set(BackgroundPropertyName, ref _background, value);
            }
        }

        /// <summary>
        /// The <see cref="FontFamily" /> property's name.
        /// </summary>
        public const string FontFamilyPropertyName = "FontFamily";

        private FontFamily _fontFamily = new FontFamily("宋体");

        /// <summary>
        /// Sets and gets the FontFamily property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                Set(FontFamilyPropertyName, ref _fontFamily, value);
            }
        }

        /// <summary>
        /// The <see cref="FontSize" /> property's name.
        /// </summary>
        public const string FontSizePropertyName = "FontSize";

        private int _fontSize = 32;

        /// <summary>
        /// Sets and gets the FontSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                Set(FontSizePropertyName, ref _fontSize, value);
            }
        }

        /// <summary>
        /// The <see cref="FontWeight" /> property's name.
        /// </summary>
        public const string FontWeightPropertyName = "FontWeight";

        private int _fontWeight = 300;

        /// <summary>
        /// Sets and gets the FontWeight property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int FontWeight
        {
            get
            {
                return _fontWeight;
            }
            set
            {
                Set(FontWeightPropertyName, ref _fontWeight, value);
            }
        }

        /// <summary>
        /// The <see cref="Foreground" /> property's name.
        /// </summary>
        public const string ForegroundPropertyName = "Foreground";

        private string _foreground = "#000";

        /// <summary>
        /// Sets and gets the Foreground property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                Set(ForegroundPropertyName, ref _foreground, value);
            }
        }

        private RelayCommand _openCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(ExecuteOpenCommand));
            }
        }

        private void ExecuteOpenCommand()
        {
            var file = LocalHelper.ChooseFile("图片|*.jpg;*.jpeg;*.png;*.bmp");
            if (string.IsNullOrEmpty(file))
            {
                return;
            }
            Background = file;
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            DatabaseHelper.Open();
            SystemHelper.Set("Background", Background);
            SystemHelper.Set("FontFamily", FontFamily.Name);
            SystemHelper.Set("FontSize", FontSize);
            SystemHelper.Set("FontWeight", FontWeight);
            SystemHelper.Set("Foreground", Foreground);
            SystemHelper.Save();
            DatabaseHelper.Close();
            _system.Execute();
        }
    }
}