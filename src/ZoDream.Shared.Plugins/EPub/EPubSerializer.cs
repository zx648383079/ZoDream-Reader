using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Tokenizers;
using ZoDream.Shared.Extensions;

namespace ZoDream.Shared.Plugins.EPub
{
    public class EPubSerializer : INovelSerializer
    {
        
        public void Dispose()
        {
        }

        public INovelSource CreateSource(INovelSourceEntity entry)
        {
            return new FileSource(entry);
        }

        public Task<ISectionSource> GetChapterAsync(INovelSource source, INovelChapter chapter)
        {
            return Task.Factory.StartNew(() => {
                using var fs = File.OpenRead(((FileSource)source).FileName);
                return GetChapter(fs, chapter);
            });
        }

        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source)
        {
            return Task.Factory.StartNew(() => {
                using var fs = File.OpenRead(((FileSource)source).FileName);
                var (_, items) = GetChapters(fs);
                return items;
            });
        }

        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source)
        {
            return Task.Factory.StartNew(() => {
                using var fs = File.OpenRead(((FileSource)source).FileName);
                return GetChapters(fs);
            });
        }

        public (INovel?, INovelChapter[]) GetChapters(Stream input)
        {
            using var archive = new ZipArchive(input, ZipArchiveMode.Read);
            var rootFile = EPubReader.GetRootFileName(archive);
            if (rootFile == null)
            {
                return (null, []);
            }
            var doc = EPubReader.Read(archive, rootFile);
            var opfNamespace = EPubReader.Opf;
            var root = doc.Element(opfNamespace + "package");
            if (root is null)
            {
                return (null, []);
            }
            var maps = new Dictionary<string, string>();
            var folder = Path.GetDirectoryName(rootFile);
            foreach (var item in root.Element(opfNamespace + "manifest").Elements())
            {
                maps.Add(item.Attribute("id").Value, folder + "/" + item.Attribute("href").Value);
            }
            var novel = new BookEntity();
            foreach (var item in root.Element(opfNamespace + "metadata").Elements())
            {
                switch (item.Name.LocalName.ToLowerInvariant())
                {
                    case "title":
                        novel.Name = item.Value;
                        break;
                    case "description":
                        novel.Description = item.Value;
                        break;
                    case "creator":
                        novel.Author = item.Value;
                        break;
                    case "meta":
                        if (item.Attribute("name").Value == "cover")
                        {
                            novel.Cover = EPubReader.GetImageResource(archive, maps[item.Attribute("content").Value])
                                .ToBase64String();
                        }
                        break;
                    default:
                        break;
                }
            }
            var spine = root.Element(opfNamespace + "spine");
            var ncx = maps[spine.Attribute("toc").Value];
            var ncxNamespace = EPubReader.Ncx;
            var ncxDoc = EPubReader.Read(archive, ncx);
            var items = new List<INovelChapter>();
            foreach (var item in ncxDoc.Element(ncxNamespace + "ncx")
                .Element(ncxNamespace + "navMap").Elements())
            {
                items.Add(new ChapterEntity()
                {
                    Title = item.Element(ncxNamespace + "navLabel").Element(ncxNamespace + "text").Value,
                    Url = folder + "/" + item.Element(ncxNamespace + "content").Attribute("src").Value
                });
            }
            return (novel, [..items]);
        }

        public ISectionSource GetChapter(Stream input, INovelChapter chapter)
        {
            using var archive = new ZipArchive(input, ZipArchiveMode.Read);
            var doc = EPubReader.Read(archive, chapter.Url);
            return new HtmlDocument(chapter.Title, doc.Root.ToString());
        }


        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }

        





        
    }
}
