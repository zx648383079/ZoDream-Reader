using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ZoDream.Shared.Repositories.Models
{
    public class NovelSearchModel: ObservableObject
    {
        private string name = string.Empty;

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string cover = string.Empty;

        public string Cover {
            get => cover;
            set => SetProperty(ref cover, value);
        }


        private string description = string.Empty;

        public string Description {
            get => description;
            set => SetProperty(ref description, value);
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


        private int sourceCount;

        public int SourceCount {
            get => sourceCount;
            set => SetProperty(ref sourceCount, value);
        }


        private ObservableCollection<string> tagItems = new();

        public ObservableCollection<string> TagItems {
            get => tagItems;
            set => SetProperty(ref tagItems, value);
        }

        private ObservableCollection<SourceNovelModel> sourceItems = new();

        public ObservableCollection<SourceNovelModel> SourceItems {
            get => sourceItems;
            set => SetProperty(ref sourceItems, value);
        }

    }
}
