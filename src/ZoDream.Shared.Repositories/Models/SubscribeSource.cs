using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class SubscribeSourceModel: BindableBase, ISubscribeSource
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name {
            get => name;
            set => Set(ref name, value);
        }

        private string groupName = string.Empty;

        public string GroupName {
            get => groupName;
            set => Set(ref groupName, value);
        }


        private string url = string.Empty;

        public string Url {
            get => url;
            set => Set(ref url, value);
        }


        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => Set(ref sortOrder, value);
        }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
