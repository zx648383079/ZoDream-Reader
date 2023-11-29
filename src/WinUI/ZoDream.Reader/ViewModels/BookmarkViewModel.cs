using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class BookmarkViewModel: BindableBase
    {

        public BookmarkViewModel()
        {
            ExportCommand = new RelayCommand(TapExport);
        }

        private ObservableCollection<BookmarkEntity> items = new();

        public ObservableCollection<BookmarkEntity> Items {
            get => items;
            set => Set(ref items, value);
        }


        public ICommand ExportCommand { get; private set; }

        private void TapExport(object? _)
        {

        }

    }
}
