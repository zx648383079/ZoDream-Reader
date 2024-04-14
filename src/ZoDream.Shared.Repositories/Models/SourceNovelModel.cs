using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoDream.Shared.Repositories.Models
{
    public class SourceNovelModel: ObservableObject
    {
        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string author = string.Empty;

        public string Author {
            get => author;
            set => SetProperty(ref author, value);
        }

        private string latestChapterTitle = string.Empty;

        public string LatestChapterTitle {
            get => latestChapterTitle;
            set => SetProperty(ref latestChapterTitle, value);
        }

        public string Url { get; set; } = string.Empty;

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
    }
}
