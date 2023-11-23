using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ExploreViewModel: BindableBase
    {

        private ObservableCollection<SourceRuleEntity> siteItems = new();

        public ObservableCollection<SourceRuleEntity> SiteItems {
            get => siteItems;
            set => Set(ref siteItems, value);
        }

        private ObservableCollection<SourceChannelEntity> channelItems = new();

        public ObservableCollection<SourceChannelEntity> ChannelItems {
            get => channelItems;
            set => Set(ref channelItems, value);
        }

    }
}
