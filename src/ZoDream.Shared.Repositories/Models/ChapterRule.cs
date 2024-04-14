using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class ChapterRuleModel : ObservableObject, IChapterRule
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string matchRule = string.Empty;

        public string MatchRule
        {
            get => matchRule;
            set => SetProperty(ref matchRule, value);
        }


        private string example = string.Empty;

        public string Example
        {
            get => example;
            set => SetProperty(ref example, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => SetProperty(ref sortOrder, value);
        }

        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        private bool isChecked;

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
    }
}
