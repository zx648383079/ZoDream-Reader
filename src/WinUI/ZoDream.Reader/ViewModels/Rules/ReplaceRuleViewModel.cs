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
    public class ReplaceRuleViewModel: ObservableObject
    {
        public ReplaceRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand<ReplaceRuleModel>(TapEdit);
            DeleteCommand = new RelayCommand<ReplaceRuleModel>(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand<ReplaceRuleModel>(TapToggle);
            SortCommand = new RelayCommand<ReplaceRuleModel>(TapSort);
            SortBottomCommand = new RelayCommand<ReplaceRuleModel>(TapSortBottom);
            SortTopCommand = new RelayCommand<ReplaceRuleModel>(TapSortTop);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<ReplaceRuleModel> ruleItems = [];

        public ObservableCollection<ReplaceRuleModel> RuleItems {
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

        private void TapSort(ReplaceRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            SaveSort();
        }

        private void TapSortTop(ReplaceRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            RuleItems.MoveToFirst(RuleItems.IndexOf(arg));
            SaveSort();
        }

        private void TapSortBottom(ReplaceRuleModel? arg)
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
            await _app.Database.SortReplaceRuleAsync(RuleItems);
        }

        private void TapToggle(ReplaceRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            _app.Database.ToggleReplaceRuleAsync(arg.IsEnabled, arg.Id);
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

        private void TapDelete(ReplaceRuleModel? arg)
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
            await _app.Database.DeleteReplaceRuleAsync(items);
            for (int i = RuleItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(RuleItems[i].Id))
                {
                    RuleItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(ReplaceRuleModel? arg)
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

        private async void EditRule(ReplaceRuleModel data)
        {
            var picker = new AddReplaceDialog
            {
                DataContext = data.Clone<ReplaceRuleModel>()
            };
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.MatchValue))
            {
                return;
            }
            picker.ViewModel.CopyTo(data);
            await _app.Database.SaveReplaceRuleAsync(data);
        }

        private async void TapAdd()
        {
            var picker = new AddReplaceDialog();
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.MatchValue))
            {
                return;
            }
            var item = picker.ViewModel.Clone<ReplaceRuleModel>();
            RuleItems.Add(item);
            await _app.Database.SaveReplaceRuleAsync(item);
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
            var items = await dialog.Importer.LoadReplaceRuleAsync<ReplaceRuleModel>(file.Path);
            foreach (var item in items)
            {
                if (Contains(item))
                {
                    continue;
                }
                RuleItems.Add(item);
                await _app.Database.SaveReplaceRuleAsync(item);
            }
        }

        public bool Contains(IReplaceRule rule)
        {
            foreach (var item in RuleItems)
            {
                if (item.MatchValue == rule.MatchValue 
                    && item.ReplaceValue == rule.ReplaceValue)
                {
                    return true;
                }
            }
            return false;
        }

        public async void LoadAsync()
        {
            RuleItems.Clear();
            var items = await _app.Database.GetReplaceRuleAsync<ReplaceRuleModel>();
            foreach (var item in items)
            {
                RuleItems.Add(item);
            }
        }
    }
}
