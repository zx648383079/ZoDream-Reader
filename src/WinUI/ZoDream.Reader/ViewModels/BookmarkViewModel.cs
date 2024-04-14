using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class BookmarkViewModel: ObservableObject
    {

        public BookmarkViewModel()
        {
            ExportCommand = new RelayCommand(TapExport);
        }

        private ObservableCollection<BookmarkEntity> items = [];

        public ObservableCollection<BookmarkEntity> Items {
            get => items;
            set => SetProperty(ref items, value);
        }


        public ICommand ExportCommand { get; private set; }

        private void TapExport()
        {

        }

    }
}
