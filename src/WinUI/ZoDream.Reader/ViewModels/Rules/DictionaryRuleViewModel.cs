using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class DictionaryRuleViewModel: ObservableObject
    {
        public DictionaryRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand<DictionaryRuleModel>(TapEdit);
            DeleteCommand = new RelayCommand<DictionaryRuleModel>(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand<DictionaryRuleModel>(TapToggle);
            SortCommand = new RelayCommand<DictionaryRuleModel>(TapSort);
            SortBottomCommand = new RelayCommand<DictionaryRuleModel>(TapSortBottom);
            SortTopCommand = new RelayCommand<DictionaryRuleModel>(TapSortTop);
            CustomDictCommand = new RelayCommand(TapCustomDict);
            LoadAsync();
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<DictionaryRuleModel> ruleItems = [];

        public ObservableCollection<DictionaryRuleModel> RuleItems {
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

        public ICommand CustomDictCommand {  get; private set; }

        private void TapCustomDict()
        {
            var router = App.GetService<IRouter>();
            router.GoToAsync("creator/dictionary");
        }

        private void TapSort(DictionaryRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            SaveSort();
        }

        private void TapSortTop(DictionaryRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            RuleItems.MoveToFirst(RuleItems.IndexOf(arg));
            SaveSort();
        }

        private void TapSortBottom(DictionaryRuleModel? arg)
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
            await _app.Database.SortDictionaryRuleAsync(RuleItems);
        }

        private void TapToggle(DictionaryRuleModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            _app.Database.ToggleDictionaryRuleAsync(arg.IsEnabled, arg.Id);
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

        private void TapDelete(DictionaryRuleModel? arg)
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
            await _app.Database.DeleteDictionaryRuleAsync(items);
            for (int i = RuleItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(RuleItems[i].Id))
                {
                    RuleItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(DictionaryRuleModel? arg)
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

        private async void TapAdd()
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

        private async void TapImport()
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
