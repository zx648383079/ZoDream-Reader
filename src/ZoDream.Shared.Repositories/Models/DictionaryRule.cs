using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class DictionaryRuleModel : BindableBase, IDictionaryRule
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }


        private string urlRule = string.Empty;

        public string UrlRule
        {
            get => urlRule;
            set => Set(ref urlRule, value);
        }

        private string showRule = string.Empty;

        public string ShowRule
        {
            get => showRule;
            set => Set(ref showRule, value);
        }


        private bool isEnabled = true;

        public bool IsEnabled
        {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => Set(ref sortOrder, value);
        }

        private bool isChecked;

        public bool IsChecked
        {
            get => isChecked;
            set => Set(ref isChecked, value);
        }



    }
}
