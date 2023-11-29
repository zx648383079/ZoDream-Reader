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
    public class HistoryViewModel: BindableBase
    {

        public HistoryViewModel()
        {
        }

        private ObservableCollection<ReadRecordEntity> items = new();

        public ObservableCollection<ReadRecordEntity> Items {
            get => items;
            set => Set(ref items, value);
        }



    }
}
