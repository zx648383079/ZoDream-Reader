using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public class NovelSourceViewModel :  ObservableObject, INovelBasic
    {


        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Stream? Cover { get; set; }
        public string Author { get; set; } = string.Empty;

        public byte Rating { get; set; }

        public string Brief { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; }

        private bool _isChecked;

        public bool IsChecked {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        private bool _isEnabled;

        public bool IsEnabled {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

    }
}
