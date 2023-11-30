using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class SourceRuleModel : BindableBase, ISourceRule
    {
        public int Id { get; set; }
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

        public string LoginUrl { get; set; } = string.Empty;
        public IFormInput[] LoginForm { get; set; } = [];

        public long LastUpdatedAt { get; set; }

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

        private SourceUpdateStatus status = SourceUpdateStatus.None;

        public SourceUpdateStatus Status {
            get => status;
            set => Set(ref status, value);
        }

    }
}
