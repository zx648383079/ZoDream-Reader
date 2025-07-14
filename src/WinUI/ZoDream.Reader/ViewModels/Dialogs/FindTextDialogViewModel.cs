using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoDream.Reader.ViewModels
{
    public class FindTextDialogViewModel : ObservableObject, IFormValidator
    {

        private string _findText = string.Empty;

        public string FindText {
            get => _findText;
            set {
                SetProperty(ref _findText, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _replaceText = string.Empty;

        public string ReplaceText {
            get => _replaceText;
            set => SetProperty(ref _replaceText, value);
        }

        public bool IsValid => !string.IsNullOrEmpty(FindText);
    }
}
