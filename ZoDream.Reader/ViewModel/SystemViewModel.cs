using System;
using System.Drawing;
using GalaSoft.MvvmLight;

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
        /// <summary>
        /// Initializes a new instance of the SystemViewModel class.
        /// </summary>
        public SystemViewModel()
        {
            _loadFont();
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
    }
}