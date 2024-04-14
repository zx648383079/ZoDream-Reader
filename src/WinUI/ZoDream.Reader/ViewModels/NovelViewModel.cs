using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class NovelViewModel : BookModel, IQueryAttributable
    {
        public NovelViewModel()
        {
            ReadCommand = new RelayCommand(TapRead);
            SecondaryCommand = new RelayCommand(TapSecondary);
        }

        private bool isOnShelf;

        public bool IsOnShelf {
            get => isOnShelf;
            set {
                SetProperty(ref isOnShelf, value);
                OnPropertyChanged(nameof(SecondaryText));
            }
        }

        public string SecondaryText => IsOnShelf ? "删除书籍" : "加入书架";

        public ICommand ReadCommand { get; private set; }
        public ICommand SecondaryCommand { get; private set; }

        private void TapRead()
        {

        }

        private void TapSecondary() 
        {
            IsOnShelf = !IsOnShelf;
        }


        public void ApplyQueryAttributes(IDictionary<string, object> queries)
        {
            if (queries.TryGetValue("novel", out var obj) && obj is INovel novel)
            {
                novel.CopyTo(this);
            }
        }
    }
}
