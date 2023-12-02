using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ChapterRuleViewModel: BindableBase
    {
        public ChapterRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand(TapEdit);
            DeleteCommand = new RelayCommand(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            LoadAsync();
        }

        private ObservableCollection<ChapterRuleModel> ruleItems = new();

        public ObservableCollection<ChapterRuleModel> RuleItems {
            get => ruleItems;
            set => Set(ref ruleItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand ToggleCheckCommand { get; private set; }

        private void TapToggleCheck(object? _)
        {
            if (RuleItems.Count == 0)
            {
                return;
            }
            var isChecked = !RuleItems[0].IsChecked;
            foreach (var item in RuleItems)
            {
                item.IsChecked = isChecked;
            }
        }

        private void TapDelete(object? arg)
        {
            
            if (arg is null)
            {
                DeleteRule(RuleItems.Where(item => item.IsChecked).Select(item => item.Id).ToArray());
                return;
            }
            if (arg is ChapterRuleModel data)
            {
                DeleteRule(data.Id);
            }
        }

        private async void DeleteRule(params int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var app = App.GetService<AppViewModel>();
            if (!await app.ConfirmAsync($"确定删除 {items.Length} 条规则？"))
            {
                return;
            }
            await app.Database.DeleteChapterRuleAsync(items);
            for (int i = RuleItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(RuleItems[i].Id))
                {
                    RuleItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(object? arg) 
        {
            if (arg is null)
            {
                foreach (var item in RuleItems)
                {
                    if (item.IsChecked)
                    {
                        EditRule(item);
                        break;
                    }
                }
                return;
            }
            if (arg is ChapterRuleModel data)
            {
                EditRule(data);
            }
            
        }

        private async void EditRule(ChapterRuleModel data)
        {
            var app = App.GetService<AppViewModel>();
            var picker = new AddChapterRuleDialog
            {
                DataContext = data.Clone<ChapterRuleModel>()
            };
            var res = await app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.MatchRule))
            {
                return;
            }
            picker.ViewModel.CopyTo(data);
            await app.Database.SaveChapterRuleAsync(data);
        }

        private async void TapAdd(object? _)
        {
            var app = App.GetService<AppViewModel>();
            var picker = new AddChapterRuleDialog();
            var res = await app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.MatchRule) || Contains(picker.ViewModel))
            {
                return;
            }
            var item = picker.ViewModel.Clone<ChapterRuleModel>();
            RuleItems.Add(item);
            await app.Database.SaveChapterRuleAsync(item);
        }

        private async void TapImport(object? _)
        {
            var app = App.GetService<AppViewModel>();
            var dialog = new ImportDialog();
            var res = await app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            var items = await dialog.Importer.LoadChapterRuleAsync<ChapterRuleModel>(file.Path);
            foreach (var item in items)
            {
                if (Contains(item))
                {
                    continue;
                }
                RuleItems.Add(item);
                await app.Database.SaveChapterRuleAsync(item);
            }
        }

        public bool Contains(string rule)
        {
            foreach (var item in RuleItems)
            {
                if (item.MatchRule == rule)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(IChapterRule rule)
        {
            return Contains(rule.MatchRule);
        }

        public async void LoadAsync()
        {
            RuleItems.Clear();
            var app = App.GetService<AppViewModel>();
            var items = await app.Database.GetChapterRuleAsync<ChapterRuleModel>();
            foreach (var item in items)
            {
                RuleItems.Add(item);
            }
        }
    }
}
