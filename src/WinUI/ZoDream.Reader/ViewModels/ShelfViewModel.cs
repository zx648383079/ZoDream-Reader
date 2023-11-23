﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ShelfViewModel: BindableBase
    {
        public ShelfViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            for (int i = 0; i < 10; i++)
            {
                NovelItems.Add(new BookEntity() { Name = $"小说{i}" });
            }
        }

        private ObservableCollection<BookEntity> novelItems = new();

        public ObservableCollection<BookEntity> NovelItems {
            get => novelItems;
            set => Set(ref novelItems, value);
        }



        public ICommand AddCommand { get; private set; }

        private async void TapAdd(object _)
        {
            var picker = new AddNovelDialog();
            await App.GetService<AppViewModel>().OpenDialogAsync(picker);
        }


    }
}
