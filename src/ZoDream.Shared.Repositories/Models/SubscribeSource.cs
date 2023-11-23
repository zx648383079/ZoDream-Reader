using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class SubscribeSourceModel: BindableBase, ISubscribeSource
    {
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




        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
