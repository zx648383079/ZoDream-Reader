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
    public class ChapterRuleViewModel: BindableBase
    {
        public ChapterRuleViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
        }

        private ObservableCollection<ChapterRuleModel> ruleItems = new();

        public ObservableCollection<ChapterRuleModel> RuleItems {
            get => ruleItems;
            set => Set(ref ruleItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand {  get; private set; }

        private async void TapAdd(object? _)
        {
            var picker = new AddChapterRuleDialog();
            var res = await App.GetService<AppViewModel>().OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            RuleItems.Add(picker.ViewModel.Clone<ChapterRuleModel>());
        }

        private async void TapImport(object? _)
        {
            var picker = new ImportDialog();
            var res = await App.GetService<AppViewModel>().OpenDialogAsync(picker);
            
        }
    }
}
