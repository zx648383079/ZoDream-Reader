using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class BookModel: BindableBase, INovel
    {
        public string Id { get; set; } = string.Empty;

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


        public string FileName { get; set; } = string.Empty;
    }
}
