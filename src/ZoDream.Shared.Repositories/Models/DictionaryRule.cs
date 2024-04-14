using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class DictionaryRuleModel : ObservableObject, IDictionaryRule
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }


        private string urlRule = string.Empty;

        public string UrlRule
        {
            get => urlRule;
            set => SetProperty(ref urlRule, value);
        }

        private string showRule = string.Empty;

        public string ShowRule
        {
            get => showRule;
            set => SetProperty(ref showRule, value);
        }


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
