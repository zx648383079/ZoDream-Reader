using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class DictionaryRuleViewModel: BindableBase
    {
        public DictionaryRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand(TapEdit);
            DeleteCommand = new RelayCommand(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand(TapToggle);
            SortCommand = new RelayCommand(TapSort);
            SortBottomCommand = new RelayCommand(TapSortBottom);
            SortTopCommand = new RelayCommand(TapSortTop);
            LoadAsync();
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<DictionaryRuleModel> ruleItems = new();

        public ObservableCollection<DictionaryRuleModel> RuleItems {
            get => ruleItems;
            set => Set(ref ruleItems, value);
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

        private void TapSort(object? arg)
        {
            if (arg is not DictionaryRuleModel)
            {
                return;
            }
            SaveSort();
        }

        private void TapSortTop(object? arg)
        {
            if (arg is not DictionaryRuleModel data)
            {
                return;
            }
            RuleItems.MoveToFirst(RuleItems.IndexOf(data));
            SaveSort();
        }

        private void TapSortBottom(object? arg)
        {
            if (arg is not DictionaryRuleModel data)
            {
                return;
            }
            RuleItems.MoveToLast(RuleItems.IndexOf(data));
            SaveSort();
        }

        private async void SaveSort()
        {
            await _app.Database.SortDictionaryRuleAsync(RuleItems);
        }

        private void TapToggle(object? arg)
        {
            if (arg is not DictionaryRuleModel data)
            {
                return;
            }
            _app.Database.ToggleDictionaryRuleAsync(data.IsEnabled, data.Id);
        }

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
            if (arg is DictionaryRuleModel data)
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
            if (!await _app.ConfirmAsync($"确定删除 {items.Length} 条规则？"))
            {
                return;
            }
            await _app.Database.DeleteDictionaryRuleAsync(items);
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
            if (arg is DictionaryRuleModel data)
            {
                EditRule(data);
            }

        }

        private async void EditRule(DictionaryRuleModel data)
        {
            var picker = new AddDictionaryDialog
            {
                DataContext = data.Clone<DictionaryRuleModel>()
            };
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.UrlRule))
            {
                return;
            }
            picker.ViewModel.CopyTo(data);
            await _app.Database.SaveDictionaryRuleAsync(data);
        }

        private async void TapAdd(object? _)
        {
            var picker = new AddDictionaryDialog();
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.ShowRule))
            {
                return;
            }
            var item = picker.ViewModel.Clone<DictionaryRuleModel>();
            RuleItems.Add(item);
            await _app.Database.SaveDictionaryRuleAsync(item);
        }

        private async void TapImport(object? _)
        {
            var dialog = new ImportDialog();
            var res = await _app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            var items = await dialog.Importer.LoadDictionaryRuleAsync<DictionaryRuleModel>(file.Path);
            foreach (var item in items)
            {
                if (Contains(item))
                {
                    continue;
                }
                RuleItems.Add(item);
                await _app.Database.SaveDictionaryRuleAsync(item);
            }
        }

        public bool Contains(IDictionaryRule rule)
        {
            foreach (var item in RuleItems)
            {
                if (item.UrlRule == rule.UrlRule && item.ShowRule == rule.ShowRule)
                {
                    return true;
                }
            }
            return false;
        }

        public async void LoadAsync()
        {
            RuleItems.Clear();
            var items = await _app.Database.GetDictionaryRuleAsync<DictionaryRuleModel>();
            foreach (var item in items)
            {
                RuleItems.Add(item);
            }
        }
    }
}
