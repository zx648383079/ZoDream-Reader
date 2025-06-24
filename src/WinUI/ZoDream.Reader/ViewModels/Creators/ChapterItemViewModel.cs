using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public interface IEditableSection
    {
        public string Title { get; }
    }

    public class ChapterItemViewModel : ObservableObject, INovelSection, IEditableSection
    {


        private string _title = string.Empty;

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IList<INovelBlock> Items { get; private set; } = [];
    }

    public class VolumeItemViewModel : ObservableObject, IEditableSection
    {


        private string _title = string.Empty;

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

    }
}
