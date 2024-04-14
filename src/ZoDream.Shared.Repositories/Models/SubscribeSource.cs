using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class SubscribeSourceModel: ObservableObject, ISubscribeSource
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string groupName = string.Empty;

        public string GroupName {
            get => groupName;
            set => SetProperty(ref groupName, value);
        }


        private string url = string.Empty;

        public string Url {
            get => url;
            set => SetProperty(ref url, value);
        }


        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => SetProperty(ref sortOrder, value);
        }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
    }
}
