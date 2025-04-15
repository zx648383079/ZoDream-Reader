using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Controls;
using ZoDream.Shared.Interfaces;

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



        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand FindCommand { get; private set; }
        public ICommand FindNextCommand { get; private set; }
        public ICommand ExtractCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }

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

        private void TapSave()
        {

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
