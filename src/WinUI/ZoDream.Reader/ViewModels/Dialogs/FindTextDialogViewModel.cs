using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ZoDream.Reader.Controls;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public class FindTextDialogViewModel : ObservableObject, IFormValidator
    {
        public FindTextDialogViewModel()
        {
            MatchCommand = new RelayCommand(TapMatch);
            DeleteCommand = UICommand.Delete(TapDelete);
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

        private MatchItemViewModel? _selectedItem;

        public MatchItemViewModel? SelectedItem {
            get => _selectedItem;
            set {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(JumpEnabled));
            }
        }

        public bool JumpEnabled => SelectedItem is not null;

        public bool IsValid => !string.IsNullOrEmpty(FindText);

        public ITextMatcher Matcher => FindMode > 0 ? new RegexMatcher(FindText, ReplaceText) : new TextMatcher(FindText, ReplaceText);

        public ICommand MatchCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }


        private void TapDelete()
        {
            var arg = SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.Remove(arg);
            SelectedItem = null;
        }

        private async void TapMatch()
        {
            Items.Clear();
            if (string.IsNullOrWhiteSpace(FindText) || _source is null)
            {
                return;
            }
            ITextMatcher matcher;
            try
            {
                matcher = FindMode > 0 ? new RegexMatcher(FindText) : new TextMatcher(FindText);
            }
            catch (System.Exception)
            {
                return;
            }
            Items.Start();
            await _source.FindAsync(Items, matcher);
            Items.Stop();
        }

        public void Load(IFinderSource source)
        {
            _source = source;
        }

    }
}
