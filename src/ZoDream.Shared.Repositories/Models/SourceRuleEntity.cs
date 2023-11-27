using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class SourceRuleModel : BindableBase, ISourceRule
    {
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string groupName = string.Empty;

        public string GroupName
        {
            get => groupName;
            set => Set(ref groupName, value);
        }


        public string BaseUri { get; set; } = string.Empty;

        public SourceType Type { get; set; }

        public string DetailUrlRule { get; set; } = string.Empty;

        public bool EnabledExplore { get; set; }

        public string ExploreUrl { get; set; } = string.Empty;
        public string ExploreMatchRule { get; set; } = string.Empty;
        public string SearchUrl { get; set; } = string.Empty;

        public string SearchMatchRule { get; set; } = string.Empty;

        public string DetailMatchRule { get; set; } = string.Empty;
        public string ContentMatchRule { get; set; } = string.Empty;


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
