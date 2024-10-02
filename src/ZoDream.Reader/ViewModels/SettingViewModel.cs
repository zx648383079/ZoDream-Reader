using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.ViewModels
{
    public class SettingViewModel: ObservableObject
    {
        public SettingViewModel()
        {
            CrumbItems.Add("设置");
            Load();
        }



        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<string> crumbItems = [];

        public ObservableCollection<string> CrumbItems
        {
            get => crumbItems;
            set => SetProperty(ref crumbItems, value);
        }

        private ObservableCollection<FontItem> fontItems = [];

        public ObservableCollection<FontItem> FontItems
        {
            get => fontItems;
            set => SetProperty(ref fontItems, value);
        }

        private string[] animateItems = ["无", "仿真", "覆盖", "上下", "滚屏"];

        public string[] AnimateItems
        {
            get => animateItems;
            set => SetProperty(ref animateItems, value);
        }


        private int fontSize;

        public int FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }

        private string fontFamily = string.Empty;

        public string FontFamily
        {
            get => fontFamily;
            set => SetProperty(ref fontFamily, value);
        }


        private string background = "#fff";

        public string Background
        {
            get => background;
            set => SetProperty(ref background, value);
        }

        private string backgroundImage = string.Empty;

        public string BackgroundImage
        {
            get => backgroundImage;
            set => SetProperty(ref backgroundImage, value);
        }

        private string foreground = "#333";

        public string Foreground
        {
            get => foreground;
            set => SetProperty(ref foreground, value);
        }

        private bool openDark;

        public bool OpenDark
        {
            get => openDark;
            set => SetProperty(ref openDark, value);
        }

        private int columnCount;

        public int ColumnCount
        {
            get => columnCount;
            set => SetProperty(ref columnCount, value);
        }


        private int lineSpace;

        public int LineSpace
        {
            get => lineSpace;
            set => SetProperty(ref lineSpace, value);
        }


        private int letterSpace;

        public int LetterSpace
        {
            get => letterSpace;
            set => SetProperty(ref letterSpace, value);
        }


        private int padding;

        public int Padding
        {
            get => padding;
            set => SetProperty(ref padding, value);
        }

        private bool isSimple;

        public bool IsSimple
        {
            get => isSimple;
            set => SetProperty(ref isSimple, value);
        }


        private int animation;

        public int Animation
        {
            get => animation;
            set => SetProperty(ref animation, value);
        }


        private bool autoFlip;

        public bool AutoFlip
        {
            get => autoFlip;
            set => SetProperty(ref autoFlip, value);
        }

        private float flipSpace;

        public float FlipSpace
        {
            get => flipSpace;
            set => SetProperty(ref flipSpace, value);
        }


        private bool openSpeak;

        public bool OpenSpeak
        {
            get => openSpeak;
            set => SetProperty(ref openSpeak, value);
        }

        private float speakSpeed;

        public float SpeakSpeed
        {
            get => speakSpeed;
            set => SetProperty(ref speakSpeed, value);
        }


        private async void Load()
        {
            var items = await _app.Storage.GetFontsAsync();
            foreach (var item in items) 
            {
                FontItems.Add(item);
            }
            Animation = _app.Option.Animation;
            OpenDark = _app.Option.OpenDark;
            LetterSpace = _app.ReadTheme.LetterSpacing;
            LineSpace = _app.ReadTheme.LineSpacing;
            OpenSpeak = _app.Option.OpenSpeak;
            Padding = _app.ReadTheme.PaddingLeft;
            FontSize = _app.ReadTheme.FontSize;
            FontFamily = _app.ReadTheme.FontFamily;
            Foreground = _app.ReadTheme.Foreground;
            AutoFlip = _app.Option.AutoFlip;
            Background = _app.ReadTheme.Background;
            BackgroundImage = _app.ReadTheme.BackgroundImage;
            ColumnCount = _app.Option.ColumnCount;
            FlipSpace = _app.Option.FlipSpace;
            SpeakSpeed = _app.Option.SpeakSpeed;
            IsSimple = _app.Option.IsSimple;
        }

        public async Task SaveAsync()
        {
            _app.Option.Animation = Animation;
            _app.Option.OpenDark = OpenDark;
            _app.ReadTheme.LetterSpacing = LetterSpace;
            _app.ReadTheme.LineSpacing = LineSpace;
            _app.Option.OpenSpeak = OpenSpeak;
            _app.ReadTheme.PaddingLeft = Padding;
            _app.ReadTheme.PaddingRight = Padding;
            _app.ReadTheme.PaddingTop = Padding;
            _app.ReadTheme.PaddingBottom = Padding;
            _app.ReadTheme.FontSize = FontSize;
            _app.ReadTheme.FontFamily  = FontFamily;
            _app.ReadTheme.Foreground = Foreground;
            _app.Option.AutoFlip = AutoFlip;
            _app.ReadTheme.Background  = Background;
            _app.ReadTheme.BackgroundImage = BackgroundImage;
            _app.Option.ColumnCount = ColumnCount;
            _app.Option.FlipSpace = FlipSpace;
            _app.Option.SpeakSpeed = SpeakSpeed;
            _app.Option.IsSimple = IsSimple;
            await _app.Database.SaveReadThemeAsync(_app.ReadTheme);
            _app.Option.ReadTheme = _app.ReadTheme.Id;
            await _app.Database.SaveSettingAsync(_app.Option);
        }
    }
}
