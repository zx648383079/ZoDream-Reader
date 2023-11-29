using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class SourceNovelModel: BindableBase
    {
        private string name = string.Empty;

        public string Name {
            get => name;
            set => Set(ref name, value);
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

        public string Url { get; set; } = string.Empty;

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
