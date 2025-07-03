using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Text;

namespace ZoDream.Reader.ViewModels
{
    public class CreateDictionaryViewModel : ObservableObject, IComparer<KeyValuePair<char, int>>
    {
        public CreateDictionaryViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            SaveCommand = new RelayCommand(TapSave);
            FindCommand = new RelayCommand(TapFind);
            FindNextCommand = new RelayCommand(TapFindNext);
            ExtractCommand = new RelayCommand(TapExtract);
            BackCommand = new RelayCommand(TapBack);
            ForwardCommand = new RelayCommand(TapForward);

            SaveDictCommand = new RelayCommand(TapSaveDict);
            OrderCommand = new RelayCommand(TapOrder);
            FindLetterCommand = new RelayCommand<WordItemViewModel>(TapFindLetter);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        public ITextEditor? Editor { get; internal set; }

        private string _findText = string.Empty;

        public string FindText {
            get => _findText;
            set => SetProperty(ref _findText, value);
        }

        private bool _findVisible;

        public bool FindVisible {
            get => _findVisible;
            set => SetProperty(ref _findVisible, value);
        }

        private ObservableCollection<WordItemViewModel> _wordItems = [];

        public ObservableCollection<WordItemViewModel> WordItems {
            get => _wordItems;
            set => SetProperty(ref _wordItems, value);
        }


        private bool _backEnabled;

        public bool BackEnabled {
            get => _backEnabled;
            set => SetProperty(ref _backEnabled, value);
        }

        private bool _forwardEnabled;

        public bool ForwardEnabled {
            get => _forwardEnabled;
            set => SetProperty(ref _forwardEnabled, value);
        }

        private Visibility _dictVisible = Visibility.Collapsed;

        public Visibility DictVisible {
            get => _dictVisible;
            set => SetProperty(ref _dictVisible, value);
        }

        private bool _isDictFilter;

        public bool IsDictFilter {
            get => _isDictFilter;
            set {
                SetProperty(ref _isDictFilter, value);
                OnPropertyChanged(nameof(FilterIcon));
            }
        }

        public string FilterIcon => IsDictFilter ? "\uED1A" : "\uE890";
        private bool _isDictOrder = true;

        public bool IsDictOrder {
            get => _isDictOrder;
            set {
                SetProperty(ref _isDictOrder, value);
                OnPropertyChanged(nameof(OrderIcon));
            }
        }

        public string OrderIcon => IsDictOrder ? "\uE74B" : "\uE74A";

        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand FindCommand { get; private set; }
        public ICommand FindNextCommand { get; private set; }
        public ICommand ExtractCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }


        public ICommand SaveDictCommand { get; private set; }
        public ICommand OrderCommand { get; private set; }

        public ICommand FindLetterCommand { get; private set; }

        private void TapFindLetter(WordItemViewModel? word)
        {
            if (Editor is null || word is null)
            {
                return;
            }
            FindVisible = true;
            FindText = word.Word;
            TapFindNext();
        }

        private void TapSaveDict()
        {

        }

        private void TapOrder()
        {
            IsDictOrder = !IsDictOrder;
        }

        private void TapBack()
        {
            if (Editor is null)
            {
                return;
            }
            Editor.GoBack();
            SyncState();
        }
        private void TapForward()
        {
            if (Editor is null)
            {
                return;
            }
            Editor.GoForward();
            SyncState();
        }

        private void SyncState()
        {
            if (Editor is null)
            {
                return;
            }
            BackEnabled = Editor.CanBack;
            ForwardEnabled = Editor.CanForward;
        }

        private void TapFind()
        {
            if (Editor is null)
            {
                return;
            }
            FindVisible = !FindVisible;
            Editor.Unselect();
        }

        private async void TapFindNext()
        {
            if (Editor is null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(FindText))
            {
                return;
            }
            if (!Editor.FindNext(FindText))
            {
                await _app.ConfirmAsync("Not Found");
            }
        }


        private async void TapOpen()
        {
            if (Editor is null)
            {
                return;
            }
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".txt");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            Editor.LoadFromFile(file.Path);
            SyncState();
        }

        private async void TapSave()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("字典文件", [".bin"]);
            picker.SuggestedFileName = "dict.bin";
            _app.InitializePicker(picker);
            var file = await picker.PickSaveFileAsync();
            if (file is null)
            {
                return;
            }
            OwnDictionary.WriteFile(await file.OpenStreamForWriteAsync(), 
                WordItems.Select(i => i.Word[0]));
        }

        private void TapExtract()
        {
            if (Editor is null)
            {
                return;
            }
            var data = Editor.Count();
            WordItems.Clear();
            foreach (var item in data.Order(this))
            {
                WordItems.Add(new(item.Key)
                {
                    Count = item.Value
                });
            }
            DictVisible = Visibility.Visible;
        }

        public int Compare(KeyValuePair<char, int> x, KeyValuePair<char, int> y)
        {
            if (x.Key > 127 && y.Key <= 127)
            {
                return 1;
            }
            if (x.Key <= 127 && y.Key > 127)
            {
                return -1;
            }
            return y.Value - x.Value;
        }
    }
}
