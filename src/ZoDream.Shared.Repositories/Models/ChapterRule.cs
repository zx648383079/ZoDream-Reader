using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class ChapterRuleModel : BindableBase, IChapterRule
    {
        public int Id { get; set; }
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string matchRule = string.Empty;

        public string MatchRule
        {
            get => matchRule;
            set => Set(ref matchRule, value);
        }


        private string example = string.Empty;

        public string Example
        {
            get => example;
            set => Set(ref example, value);
        }

        private int sortOrder = 99;

        public int SortOrder {
            get => sortOrder;
            set => Set(ref sortOrder, value);
        }

        private bool isEnabled = true;

        public bool IsEnabled {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        private bool isChecked;

        public bool IsChecked
        {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
