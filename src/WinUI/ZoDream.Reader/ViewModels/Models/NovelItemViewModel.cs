using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class NovelItemViewModel : ObservableObject
    {

        public NovelItemViewModel(ShelfViewModel host, BookEntity entity)
        {
            Host = host;
            Source = entity;
            Cover = entity.Cover;
            Name = entity.Name;
            Author = entity.Author;
            Remaining = entity.ChapterCount - entity.CurrentChapterIndex;
            LatestChapterTitle = entity.LatestChapterTitle;
            CurrentChapterTitle = entity.CurrentChapterTitle;

            EditCommand = new RelayCommand(TapEdit);
            DeleteCommand = new RelayCommand(TapDelete);
            DetailCommand = new RelayCommand(TapDetail);
            ReadCommand = new RelayCommand(TapRead);
        }

        public ShelfViewModel Host { get; private set; }

        public BookEntity Source {  get; private set; }


        private string _cover = string.Empty;

        public string Cover {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        private string _author = string.Empty;

        public string Author {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _currentChapterTitle = string.Empty;

        public string CurrentChapterTitle {
            get => _currentChapterTitle;
            set => SetProperty(ref _currentChapterTitle, value);
        }


        private string _latestChapterTitle = string.Empty;

        public string LatestChapterTitle {
            get => _latestChapterTitle;
            set => SetProperty(ref _latestChapterTitle, value);
        }


        private int _remaining;

        public int Remaining {
            get => _remaining;
            set => SetProperty(ref _remaining, value);
        }

        private bool _isChecked;

        public bool IsChecked {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }


        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand DetailCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }

        private void TapDetail()
        {
            if (Host.IsMultipleSelect)
            {
                IsChecked = !IsChecked;
                return;
            }
            Host.DetailCommand.Execute(this);
        }

        private void TapRead()
        {
            if (Host.IsMultipleSelect)
            {
                IsChecked = !IsChecked;
                return;
            }
            Host.ReadCommand.Execute(this);
        }

        private void TapEdit()
        {
            Host.EditCommand.Execute(this);
        }

        private void TapDelete()
        {
            Host.DeleteCommand.Execute(this);
        }
    }
}
