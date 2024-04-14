using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class AppThemeModel: ObservableObject, IAppTheme
    {
        public int Id { get; set; }

        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
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
            set => SetProperty(ref isEnabled, value);
        }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
    }
}
