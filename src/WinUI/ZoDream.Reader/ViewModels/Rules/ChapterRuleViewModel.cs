using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class ChapterRuleViewModel: ObservableObject
    {
        public ChapterRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand<ChapterRuleModel>(TapEdit);
            DeleteCommand = new RelayCommand<ChapterRuleModel>(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand<ChapterRuleModel>(TapToggle);
            SortCommand = new RelayCommand<ChapterRuleModel>(TapSort);
            SortBottomCommand = new RelayCommand<ChapterRuleModel>(TapSortBottom);
            SortTopCommand = new RelayCommand<ChapterRuleModel>(TapSortTop);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<ChapterRuleModel> ruleItems = [];

        public ObservableCollection<ChapterRuleModel> RuleItems {
            get => ruleItems;
            set => SetProperty(ref ruleItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand ToggleCheckCommand { get; private set; }
        public ICommand ToggleCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand SortBottomCommand { get; private set; }
        public ICommand SortTopCommand { get; private set; }

        private void TapSort(ChapterRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            SaveSort();
        }

        private void TapSortTop(ChapterRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            RuleItems.MoveToFirst(RuleItems.IndexOf(arg));
            SaveSort();
        }

        private void TapSortBottom(ChapterRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            RuleItems.MoveToLast(RuleItems.IndexOf(arg));
            SaveSort();
        }

        private async void SaveSort()
        {
            await _app.Database.SortChapterRuleAsync(RuleItems);
        }

        private void TapToggle(ChapterRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            _app.Database.ToggleChapterRuleAsync(arg.IsEnabled, arg.Id);
        }

        private void TapToggleCheck()
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

        private void TapDelete(ChapterRuleModel? arg)
        {
            
            if (arg is null)
            {
                DeleteRule(RuleItems.Where(item => item.IsChecked).Select(item => item.Id).ToArray());
                return;
            }
            DeleteRule(arg.Id);
        }

        private async void DeleteRule(params int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            if (!await _app.ConfirmAsync($"确定删除 {items.Length} 条规则？"))
            {
                return;
            }
            await _app.Database.DeleteChapterRuleAsync(items);
            for (int i = RuleItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(RuleItems[i].Id))
                {
                    RuleItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(ChapterRuleModel? arg) 
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
            EditRule(arg);
            
        }

        private async void EditRule(ChapterRuleModel data)
        {
            var picker = new AddChapterRuleDialog
            {
                DataContext = data.Clone<ChapterRuleModel>()
            };
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.MatchRule))
            {
                return;
            }
            picker.ViewModel.CopyTo(data);
            await _app.Database.SaveChapterRuleAsync(data);
        }

        private async void TapAdd()
        {
            var picker = new AddChapterRuleDialog();
            var res = await _app.OpenDialogAsync(picker);
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
            await _app.Database.SaveChapterRuleAsync(item);
        }

        private async void TapImport()
        {
            var dialog = new ImportDialog();
            var res = await _app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker(_app.AppWindowId);
            picker.FileTypeFilter.Add(".json");
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
                await _app.Database.SaveChapterRuleAsync(item);
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
            var items = await _app.Database.GetChapterRuleAsync<ChapterRuleModel>();
            foreach (var item in items)
            {
                RuleItems.Add(item);
            }
        }
    }
}
