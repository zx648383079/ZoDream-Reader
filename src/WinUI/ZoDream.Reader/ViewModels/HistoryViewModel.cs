using CommunityToolkit.Mvvm.ComponentModel;
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
    public class HistoryViewModel: ObservableObject
    {

        public HistoryViewModel()
        {
        }

        private ObservableCollection<ReadRecordEntity> items = [];

        public ObservableCollection<ReadRecordEntity> Items {
            get => items;
            set => SetProperty(ref items, value);
        }



    }
}
