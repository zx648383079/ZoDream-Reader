using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Reader.Dialogs;
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
        }

        private ObservableCollection<DictionaryRuleModel> ruleItems = new();

        public ObservableCollection<DictionaryRuleModel> RuleItems {
            get => ruleItems;
            set => Set(ref ruleItems, value);
        }

        public ICommand AddCommand { get; private set; }

        private async void TapAdd(object? _)
        {
            var picker = new AddDictionaryDialog();
            var res = await App.GetService<AppViewModel>().OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            RuleItems.Add(picker.ViewModel.Clone<DictionaryRuleModel>());
        }
    }
}
