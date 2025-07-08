using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class NovelItemViewModel : ObservableObject
    {

        public NovelItemViewModel(BookEntity entity)
        {
            Cover = entity.Cover;
            Name = entity.Name;
            Author = entity.Author;
            Remaining = entity.ChapterCount - entity.CurrentChapterIndex;
            LatestChapterTitle = entity.LatestChapterTitle;
            CurrentChapterTitle = entity.CurrentChapterTitle;
        }


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

    }
}
