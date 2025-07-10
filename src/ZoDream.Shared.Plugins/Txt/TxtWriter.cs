using System.Collections.Generic;
using System.IO;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtWriter(INovelDocument data) : INovelWriter
    {
        const string Indent = "    ";
       

        public void Write(Stream output)
        {
            var writer = new StreamWriter(output);
            writer.WriteLine(data.Name);
            if (!string.IsNullOrWhiteSpace(data.Author))
            {
                writer.WriteLine($"作者：{data.Author}");
            }
            if (!string.IsNullOrWhiteSpace(data.Brief))
            {
                writer.WriteLine("简介：");
                foreach (var item in data.Brief.Split('\n'))
                {
                    writer.WriteLine($"{Indent}{item}");
                }
            }
            writer.WriteLine();
            writer.WriteLine();
            foreach (var volume in data.Items)
            {
                Write(writer, volume);
            }
        }

        private void Write(StreamWriter writer, INovelVolume volume)
        {
            if (!string.IsNullOrEmpty(volume.Name))
            {
                writer.WriteLine(volume.Name);
                writer.WriteLine();
            }
            foreach (var item in volume)
            {
                Write(writer, item);
            }
        }

        private void Write(StreamWriter writer, INovelSection section)
        {
            WriteTitle(writer, section.Title);
            WriteContent(writer, section.Items);
        }

        private void WriteTitle(StreamWriter writer, string title)
        {
            writer.WriteLine(title);
            writer.WriteLine();
        }

        private void WriteContent(StreamWriter writer, IEnumerable<INovelBlock> items)
        {
            foreach (var block in items)
            {
                if (block is INovelTextBlock text)
                {
                    writer.WriteLine($"{Indent}{text.Text}");
                }
            }
            writer.WriteLine();
            writer.WriteLine();
        }

        public void Dispose()
        {
        }
    }
}
