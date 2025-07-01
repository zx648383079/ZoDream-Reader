using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZoDream.Reader.ViewModels
{
    public class ChapterMatchDialogViewModel : ObservableObject
    {
        public ChapterMatchDialogViewModel()
        {
            MatchCommand = new RelayCommand<string>(TapMatch);
        }


        private string _fileName = string.Empty;


        private string _ruleText = string.Empty;

        public string RuleText {
            get => _ruleText;
            set => SetProperty(ref _ruleText, value);
        }


        private AsyncObservableCollection<string> _items = [];

        public AsyncObservableCollection<string> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }


        public ICommand MatchCommand { get; private set; }

        private async void TapMatch(string? text)
        {
            await LoadAsync(_fileName, text!);
        }

        public Task LoadAsync(string fileName)
        {
            _fileName = fileName;
            return LoadAsync(fileName, RuleText = @"^(正文)?[\\s]{0,6}第?[\\s]*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?[\\s\\S]{0,20}$");
        }

        public Task LoadAsync(string fileName, string ruleText)
        {

        }


    }
}
