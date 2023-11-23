using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Entities
{
    public class ChapterModel: BindableBase
    {

        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;

        public long Begin { get; set; }

        public long End { get; set; }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => Set(ref isChecked, value);
        }

    }
}
