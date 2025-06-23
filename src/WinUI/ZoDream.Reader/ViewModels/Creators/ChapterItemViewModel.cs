using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public class ChapterItemViewModel : ObservableObject
    {


        private string _title = string.Empty;

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IList<INodeLine> Nodes { get; private set; } = [];
    }
}
