using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoDream.Reader.ViewModels
{
    public class OnlineNovelItemViewModel : ObservableObject
    {
        private string _cover = string.Empty;

        public string Cover {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        private string _author = string.Empty;

        public string Author {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _brief = string.Empty;

        public string Brief {
            get => _brief;
            set => SetProperty(ref _brief, value);
        }


        private string[] _tagItems = [];

        public string[] TagItems {
            get => _tagItems;
            set => SetProperty(ref _tagItems, value);
        }


        private int _count;

        public int Count {
            get => _count;
            set => SetProperty(ref _count, value);
        }

    }
}
