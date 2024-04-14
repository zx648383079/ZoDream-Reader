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
    public class SearchNovelViewModel: ObservableObject
    {



        private ObservableCollection<SourceChannelEntity> channelItems = [];

        public ObservableCollection<SourceChannelEntity> ChannelItems {
            get => channelItems;
            set => SetProperty(ref channelItems, value);
        }


        private ObservableCollection<SearchHistoryEntity> wordItems = [];

        public ObservableCollection<SearchHistoryEntity> WordItems {
            get => wordItems;
            set => SetProperty(ref wordItems, value);
        }

        private ObservableCollection<NovelSearchModel> novelItems = [];

        public ObservableCollection<NovelSearchModel> NovelItems 
        {
            get => novelItems;
            set => SetProperty(ref novelItems, value);
        }


    }
}
