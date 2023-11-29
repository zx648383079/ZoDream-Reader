using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Models;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class SearchSourceViewModel: BindableBase
    {

        private ObservableCollection<SourceNovelModel> sourceItems = new();

        public ObservableCollection<SourceNovelModel> SourceItems 
        {
            get => sourceItems;
            set => Set(ref sourceItems, value);
        }


    }
}
