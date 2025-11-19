using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public partial class ChapterMatchDialogViewModel : ObservableObject
    {
        const string DefaultRule = @"^第\s*[0-9零一二三四五六七八九十百千]{1,10}[章回节卷集幕计]?.{0,20}$";

        public ChapterMatchDialogViewModel()
        {
            _ = InitializeAsync();
        }


        private Stream? _input;
        private string _fileName = string.Empty;
        private Encoding _encoding = Encoding.UTF8;
        private string[] _selectedItems = [];


        [ObservableProperty]
        public partial string RuleText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string[] RuleItems { get; set; } = [];


        [ObservableProperty]
        public partial AsyncObservableCollection<string> Items { get; set; } = [];
        [ObservableProperty]
        public partial string OkText { get; set; } = "全部";

        [ObservableProperty]
        public partial string SelectText { get; set; } = "仅选中0章";


        [RelayCommand]
        private async Task Match(string? text)
        {
            Items.Clear();
            await LoadAsync(text!);
        }

        [RelayCommand]
        private void SelectItems(IList<object>? selectedItems)
        {
            _selectedItems = selectedItems?.Select(i => (string)i)?.ToArray() ?? [];
            SelectText = $"仅选中{selectedItems?.Count}章";
        }

        public INovelDocument Read(ContentDialogResult res)
        {
            if (res == ContentDialogResult.None)
            {
                return new RichDocument();
            }
            return Read(res == ContentDialogResult.Secondary);
        }
        public INovelDocument Read(bool onlySelected = false)
        {
            if (_input is null || (onlySelected && _selectedItems.Length == 0))
            {
                return new RichDocument();
            }
            var reader = new TxtReader(_input, _encoding, _fileName, new Regex(RuleText));
            var res = reader.Read();
            if (!onlySelected)
            {
                return res;
            }
            for (int i = res.Items.Count - 1; i >= 0; i--)
            {
                var group = res.Items[i];
                for (var j = group.Count - 1; j >= 0; j--)
                {
                    if (_selectedItems.Contains(group[j].Title))
                    {
                        continue;
                    }
                    group.RemoveAt(j);
                }
                if (group.Count == 0)
                {
                    res.Items.RemoveAt(i);
                }
            }
            return res;
        }
        public Task LoadAsync(string fileName, Stream input, Encoding encoding)
        {
            _fileName = fileName;
            _input = input;
            _encoding = encoding;
            return LoadAsync(RuleText = DefaultRule);
        }
        public Task LoadAsync(string fileName, Stream input)
        {
            return LoadAsync(fileName, input, TxtReader.GetEncoding(input));
        }
        public Task LoadAsync(Stream input, Encoding encoding)
        {
            return LoadAsync(string.Empty, input, encoding);
        }

        private async Task LoadAsync(string ruleText)
        {
            if (_input is null)
            {
                return;
            }
            _input.Seek(0, SeekOrigin.Begin);
            var reader = new TxtReader(_input, _encoding, _fileName, new Regex(ruleText));
            await reader.ReadAsync(Items);
            OkText = $"全部(共{Items.Count}章)";
        }

        private async Task InitializeAsync()
        {
            var items = await App.GetService<AppViewModel>().Database
                .GetEnabledChapterRuleAsync();
            RuleItems = [DefaultRule,
                @"(?<volume>第[0-9零一二三四五六七八九十百千]{1,10}卷.{0,20})\s*(?<section>第[0-9零一二三四五六七八九十百千]{1,10}[章集].{0,30})$", 
                ..items];
        }
    }
}
