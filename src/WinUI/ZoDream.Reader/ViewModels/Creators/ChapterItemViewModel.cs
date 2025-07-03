using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Tokenizers;

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

        public IList<INovelBlock> Items { get; set; } = [];
        public string Text 
        {
            get {
                var sb = new StringBuilder();
                foreach (var item in Items)
                {
                    if (item is INovelTextBlock o)
                    {
                        sb.Append("    ");
                        sb.Append(o.Text);
                        sb.Append('\n');
                    }
                }
                return sb.ToString();
            }
            set {
                Items.Clear();
                foreach (var item in value.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        Items.Add(new NovelTextBlock(item.Trim()));
                    }
                }
            }
        }
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
