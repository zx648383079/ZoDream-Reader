using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ZoDream.Reader.ViewModels
{
    public class FindTextDialogViewModel : ObservableObject, IFormValidator
    {
        public FindTextDialogViewModel()
        {
            MatchCommand = new RelayCommand(TapMatch);
        }

        private IFinderSource? _source;

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

        private int _findMode;

        public int FindMode {
            get => _findMode;
            set => SetProperty(ref _findMode, value);
        }


        private AsyncObservableCollection<MatchItemViewModel> _items = [];

        public AsyncObservableCollection<MatchItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }


        public bool IsValid => !string.IsNullOrEmpty(FindText);

        public ICommand MatchCommand { get; private set; }

        private async void TapMatch()
        {
            Items.Clear();
            if (string.IsNullOrWhiteSpace(FindText) || _source is null)
            {
                return;
            }
            Items.Start();
            if (FindMode > 0)
            {
                await _source.FindAsync(Items, new Regex(FindText));
            } else
            {
                await _source.FindAsync(Items, FindText);
            }
            Items.Stop();
        }

        public void Load(IFinderSource source)
        {
            _source = source;
        }
    }
}
