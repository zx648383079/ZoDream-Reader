using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class NovelSearchModel: BindableBase
    {
        private string name = string.Empty;

        public string Name {
            get => name;
            set => Set(ref name, value);
        }

        private string cover = string.Empty;

        public string Cover {
            get => cover;
            set => Set(ref cover, value);
        }


        private string description = string.Empty;

        public string Description {
            get => description;
            set => Set(ref description, value);
        }

        private string author = string.Empty;

        public string Author {
            get => author;
            set => Set(ref author, value);
        }

        private string latestChapterTitle = string.Empty;

        public string LatestChapterTitle {
            get => latestChapterTitle;
            set => Set(ref latestChapterTitle, value);
        }


        private int sourceCount;

        public int SourceCount {
            get => sourceCount;
            set => Set(ref sourceCount, value);
        }


        private ObservableCollection<string> tagItems = new();

        public ObservableCollection<string> TagItems {
            get => tagItems;
            set => Set(ref tagItems, value);
        }

        private ObservableCollection<SourceNovelModel> sourceItems = new();

        public ObservableCollection<SourceNovelModel> SourceItems {
            get => sourceItems;
            set => Set(ref sourceItems, value);
        }

    }
}
