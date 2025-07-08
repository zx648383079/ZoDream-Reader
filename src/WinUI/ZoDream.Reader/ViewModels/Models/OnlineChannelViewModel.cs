using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ZoDream.Reader.ViewModels
{
    public class OnlineChannelViewModel : ObservableObject
    {

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<OnlineNovelItemViewModel> _items = [];

        public ObservableCollection<OnlineNovelItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }


    }
}
