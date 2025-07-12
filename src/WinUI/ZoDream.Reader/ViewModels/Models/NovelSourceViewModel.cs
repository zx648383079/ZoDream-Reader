using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using Windows.Storage;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public class NovelSourceViewModel :  ObservableObject, INovelBasic
    {
        public IStorageItem Source { get; set; }
        public bool IsDirectory { get; set; } = false;

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Stream? Cover { get; set; }
        public string Author { get; set; } = string.Empty;

        public byte Rating { get; set; } = 0;

        public string Brief { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; } = DateTime.MinValue;

        public long Size { get; set; } = 0;
        public string Tag { get; set; } = string.Empty;

        private bool _isChecked;

        public bool IsChecked {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        private bool _isEnabled = true;

        public bool IsEnabled {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

    }
}
