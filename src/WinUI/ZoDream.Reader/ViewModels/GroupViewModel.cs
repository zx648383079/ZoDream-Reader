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
    public class GroupViewModel: ObservableObject
    {

        private ObservableCollection<BookGroupEntity> groupItems = [];

        public ObservableCollection<BookGroupEntity> GroupItems {
            get => groupItems;
            set => SetProperty(ref groupItems, value);
        }

    }
}
