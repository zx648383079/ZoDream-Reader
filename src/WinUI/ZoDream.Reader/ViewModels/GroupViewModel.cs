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
    public class GroupViewModel: BindableBase
    {

        private ObservableCollection<BookGroupEntity> groupItems = new();

        public ObservableCollection<BookGroupEntity> GroupItems {
            get => groupItems;
            set => Set(ref groupItems, value);
        }

    }
}
