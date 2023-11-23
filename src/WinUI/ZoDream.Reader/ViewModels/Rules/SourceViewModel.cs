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
    public class SourceViewModel: BindableBase
    {
        public SourceViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            GroupCommand = new RelayCommand(TapGroup);
        }

        private ObservableCollection<SourceRuleModel> ruleItems = new();

        public ObservableCollection<SourceRuleModel> RuleItems {
            get => ruleItems;
            set => Set(ref ruleItems, value);
        }

        public ICommand AddCommand { get; private set; }
        public ICommand GroupCommand { get; private set; }

        private async void TapAdd(object? _)
        {
            var picker = new AddSourceDialog();
            var res = await App.GetService<AppViewModel>().OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            RuleItems.Add(picker.ViewModel.Clone<SourceRuleModel>());
        }

        public void TapGroup(object? _)
        {
            var picker = new GroupDialog();
            _ = App.GetService<AppViewModel>().OpenDialogAsync(picker);
        }
    }
}
