using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class AppThemeModel: BindableBase, IAppTheme
    {
        public int Id { get; set; }

        private string name = string.Empty;

        public string Name {
            get => name;
            set => Set(ref name, value);
        }

        public bool IsDarkTheme { get; set; }

        public string PrimaryColor { get; set; } = string.Empty;

        public string PrimaryTextColor { get; set; } = string.Empty;

        public string AccentTextColor { get; set; } = string.Empty;

        public string BodyColor { get; set; } = string.Empty;

        public string BodyTextColor { get; set; } = string.Empty;

        private bool isEnabled;

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
