using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Controls
{
    public class FileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }
        public DataTemplate? DirectoryTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is VolumeItemViewModel)
            {
                return DirectoryTemplate;
            }
            if (item is ChapterItemViewModel o)
            {
                return o.Items.Count == 0 ? DirectoryTemplate : DefaultTemplate;
            }
            if (item is NovelSourceViewModel s)
            {
                return s.IsDirectory ? DirectoryTemplate : DefaultTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
