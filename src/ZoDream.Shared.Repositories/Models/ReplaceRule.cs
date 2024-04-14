using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class ReplaceRuleModel : ObservableObject, IReplaceRule
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string GroupName { get; set; } = string.Empty;

        public string MatchValue { get; set; } = string.Empty;
        public string ReplaceValue { get; set; } = string.Empty;

        public bool IsRegex { get; set; }
        public bool IsMatchTitle { get; set; }
        public bool IsMatchContent { get; set; } = true;

        public string IncludeMatch { get; set; } = string.Empty;
        public string ExcludeMatch { get; set; } = string.Empty;

        public int Timeout { get; set; } = 3000;

        private bool isEnabled = true;

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => SetProperty(ref sortOrder, value);
        }

        private bool isChecked;

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

    }
}
