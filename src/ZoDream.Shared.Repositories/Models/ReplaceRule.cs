using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class ReplaceRuleModel : BindableBase, IReplaceRule
    {
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
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
