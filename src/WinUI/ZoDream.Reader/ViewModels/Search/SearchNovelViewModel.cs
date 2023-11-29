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
    public class SearchNovelViewModel: BindableBase
    {



        private ObservableCollection<SourceChannelEntity> channelItems = new();

        public ObservableCollection<SourceChannelEntity> ChannelItems {
            get => channelItems;
            set => Set(ref channelItems, value);
        }


        private ObservableCollection<SearchHistoryEntity> wordItems = new();

        public ObservableCollection<SearchHistoryEntity> WordItems {
            get => wordItems;
            set => Set(ref wordItems, value);
        }

        private ObservableCollection<NovelSearchModel> novelItems = new();

        public ObservableCollection<NovelSearchModel> NovelItems 
        {
            get => novelItems;
            set => Set(ref novelItems, value);
        }


    }
}
