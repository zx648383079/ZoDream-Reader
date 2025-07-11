using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.ViewModels
{
    public class ExplorerDialogViewModel : ObservableObject
    {


        private AsyncObservableCollection<NovelSourceViewModel> _items = [];

        public AsyncObservableCollection<NovelSourceViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

    }
}
