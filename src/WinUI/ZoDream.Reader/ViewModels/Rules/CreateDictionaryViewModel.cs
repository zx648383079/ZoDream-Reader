using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Controls;

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
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        public TextEditor Editor { get; private set; } = new();

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



        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand FindCommand { get; private set; }
        public ICommand FindNextCommand { get; private set; }
        public ICommand ExtractCommand { get; private set; }

        private void TapFind()
        {
            FindVisible = !FindVisible;
            Editor.Unselect();
        }

        private async void TapFindNext()
        {
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
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".txt");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            Editor.LoadFromFile(file.Path);
        }

        private void TapSave()
        {

        }

        private void TapExtract()
        {
            var text = Editor.Text;
            var data = new Dictionary<char, int>();
            foreach (var item in text)
            {
                if (item is '\t' or ' ' or '\n' or '\r')
                {
                    continue;
                }
                if (data.TryAdd(item, 1))
                {
                    continue;
                }
                data[item]++;
            }
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
