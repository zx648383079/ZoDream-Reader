using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.ViewModels
{
    public class SettingViewModel: BindableBase
    {
        public SettingViewModel()
        {
            CrumbItems.Add("设置");
            Load();
        }

        private async void Load()
        {
            var items = await App.ViewModel.DiskRepository.GetFontsAsync();
            // FontItems.Clear();
            foreach (var item in items)
            {
                FontItems.Add(item);
            }
        }

        private ObservableCollection<string> crumbItems = new ObservableCollection<string>();

        public ObservableCollection<string> CrumbItems
        {
            get => crumbItems;
            set => Set(ref crumbItems, value);
        }

        private ObservableCollection<FontItem> fontItems = new ObservableCollection<FontItem>();

        public ObservableCollection<FontItem> FontItems
        {
            get => fontItems;
            set => Set(ref fontItems, value);
        }

        private string[] animateItems = new string[] {"无", "仿真", "覆盖", "上下", "滚屏"};

        public string[] AnimateItems
        {
            get => animateItems;
            set => Set(ref animateItems, value);
        }


    }
}
