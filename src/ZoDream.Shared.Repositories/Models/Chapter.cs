using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    public class ChapterModel: ObservableObject, INovelChapter
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;

        public long Begin { get; set; }

        public long End { get; set; }

        public int Index { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

    }
}
