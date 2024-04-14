using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class ExploreViewModel: ObservableObject
    {

        private ObservableCollection<SourceRuleEntity> siteItems = [];

        public ObservableCollection<SourceRuleEntity> SiteItems {
            get => siteItems;
            set => SetProperty(ref siteItems, value);
        }

        private ObservableCollection<SourceChannelEntity> channelItems = [];

        public ObservableCollection<SourceChannelEntity> ChannelItems {
            get => channelItems;
            set => SetProperty(ref channelItems, value);
        }

    }
}
