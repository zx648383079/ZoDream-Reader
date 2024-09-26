using CommunityToolkit.Mvvm.ComponentModel;
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

        private async void Load()
        {
            //var items = await App.ViewModel.DiskRepository.GetFontsAsync();
            //// FontItems.Clear();
            //foreach (var item in items)
            //{
            //    FontItems.Add(item);
            //}
        }

        private ObservableCollection<string> crumbItems = new();

        public ObservableCollection<string> CrumbItems
        {
            get => crumbItems;
            set => SetProperty(ref crumbItems, value);
        }

        private ObservableCollection<FontItem> fontItems = new();

        public ObservableCollection<FontItem> FontItems
        {
            get => fontItems;
            set => SetProperty(ref fontItems, value);
        }

        private string[] animateItems = new string[] {"无", "仿真", "覆盖", "上下", "滚屏"};

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

        private AppOption option = new();

        public AppOption Option
        {
            get {
                option.Animation = Animation;
                option.OpenDark = OpenDark;
                option.LetterSpace = LetterSpace;
                option.LineSpace = LineSpace;
                option.OpenSpeak = OpenSpeak;
                option.Padding = Padding;
                option.FontSize = FontSize;
                option.FontFamily = FontFamily;
                option.Foreground = Foreground;
                option.AutoFlip = AutoFlip;
                option.Background = Background;
                option.BackgroundImage = BackgroundImage;
                option.ColumnCount = ColumnCount;
                option.FlipSpace = FlipSpace;
                option.SpeakSpeed = SpeakSpeed;
                option.IsSimple = IsSimple;
                return option;
            }
            set {
                option = value;
                Animation = option.Animation;
                OpenDark = option.OpenDark;
                LetterSpace = option.LetterSpace;
                LineSpace = option.LineSpace;
                OpenSpeak = option.OpenSpeak;
                Padding = option.Padding;
                FontSize = option.FontSize;
                FontFamily = option.FontFamily;
                Foreground = option.Foreground;
                AutoFlip = option.AutoFlip;
                Background = option.Background;
                BackgroundImage = option.BackgroundImage;
                ColumnCount = option.ColumnCount;
                FlipSpace = option.FlipSpace;
                SpeakSpeed = option.SpeakSpeed;
                IsSimple = option.IsSimple;
            }
        }


    }
}
