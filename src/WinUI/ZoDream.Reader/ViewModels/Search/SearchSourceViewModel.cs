using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class SearchSourceViewModel: ObservableObject
    {

        private ObservableCollection<SourceNovelModel> sourceItems = [];

        public ObservableCollection<SourceNovelModel> SourceItems 
        {
            get => sourceItems;
            set => SetProperty(ref sourceItems, value);
        }


    }
}
