using CommunityToolkit.Mvvm.ComponentModel;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Models
{
    public class BookModel: ObservableObject, INovel
    {
        public string Id { get; set; } = string.Empty;

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


        public string FileName { get; set; } = string.Empty;
    }
}
