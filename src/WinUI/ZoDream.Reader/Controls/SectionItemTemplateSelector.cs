using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Controls
{
    public class SectionItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }
        public DataTemplate? VolumeTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is VolumeItemViewModel)
            {
                return VolumeTemplate;
            }
            if (item is ChapterItemViewModel o)
            {
                return o.Items.Count == 0 ? VolumeTemplate : DefaultTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
