using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Plugins.Txt;

namespace ZoDream.Reader.ViewModels
{
    public class ChapterMatchDialogViewModel : ObservableObject
    {
        const string DefaultRule = @"^第\s*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?.{0,20}$";

        public ChapterMatchDialogViewModel()
        {
            MatchCommand = new RelayCommand<string>(TapMatch);
            _ = InitializeAsync();
        }

        
        private string _fileName = string.Empty;


        private string _ruleText = string.Empty;

        public string RuleText {
            get => _ruleText;
            set => SetProperty(ref _ruleText, value);
        }

        private string[] _ruleItems = [];

        public string[] RuleItems {
            get => _ruleItems;
            set => SetProperty(ref _ruleItems, value);
        }


        private AsyncObservableCollection<string> _items = [];

        public AsyncObservableCollection<string> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private string _okText = "确定";

        public string OkText {
            get => _okText;
            set => SetProperty(ref _okText, value);
        }


        public ICommand MatchCommand { get; private set; }

        private async void TapMatch(string? text)
        {
            Items.Clear();
            await LoadAsync(_fileName, text!);
        }

        public Task LoadAsync(string fileName)
        {
            _fileName = fileName;
            return LoadAsync(fileName, RuleText = DefaultRule);
        }

        public async Task LoadAsync(string fileName, string ruleText)
        {
            using var fs = File.OpenRead(fileName);
            using var reader = new TxtReader(fs, fileName, ruleText);
            await reader.ReadAsync(Items);
            OkText = $"确认(共{Items.Count}章)";
        }

        private async Task InitializeAsync()
        {
            var items = await App.GetService<AppViewModel>().Database
                .GetEnabledChapterRuleAsync();
            RuleItems = [DefaultRule, ..items];
        }
    }
}
