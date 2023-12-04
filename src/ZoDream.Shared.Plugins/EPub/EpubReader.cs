using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using System.Xml;
using System.Xml.Linq;

namespace ZoDream.Shared.Plugins.EPub
{
    public class EPubReader : INovelReader
    {
        const string EPUB_CONTAINER_FILE_PATH = "META-INF/container.xml";
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetChapter(string fileName, INovelChapter chapter)
        {
            var archive = ZipFile.OpenRead(fileName);
            var doc = Read(archive, chapter.Url);
            return Task.FromResult(doc.Root.ToString());
        }

        public async Task<List<INovelChapter>> GetChaptersAsync(string fileName)
        {
            var archive = ZipFile.OpenRead(fileName);
            var rootFile = GetRootFileName(archive);
            if (rootFile == null)
            {
                return [];
            }
            var doc = Read(archive, rootFile);
            XNamespace opfNamespace = "http://www.idpf.org/2007/opf";
            var root = doc.Element(opfNamespace + "package");
            if (root is null)
            {
                return [];
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
                            novel.Cover = GetImageResource(archive, maps[item.Attribute("content").Value]);
                        }
                        break;
                    default:
                        break;
                }
            }
            var spine = root.Element(opfNamespace + "spine");
            var ncx = maps[spine.Attribute("toc").Value];
            XNamespace ncxNamespace = "http://www.daisy.org/z3986/2005/ncx/";
            var ncxDoc = Read(archive, ncx);
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
            return await Task.FromResult(items);
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }

        private string? GetRootFileName(ZipArchive archive)
        {
            var reader = Read(archive, EPUB_CONTAINER_FILE_PATH);
            XNamespace cnsNamespace = "urn:oasis:names:tc:opendocument:xmlns:container";
            return reader.Element(cnsNamespace + "container")?.Element(cnsNamespace + "rootfiles")?.Element(cnsNamespace + "rootfile")?.
                Attribute("full-path").Value;
        }

        private string GetImageResource(ZipArchive archive, string fileName)
        {
            var entry = archive.GetEntry(fileName);
            using var ms = new MemoryStream();
            entry.Open().CopyTo(ms);
            return Convert.ToBase64String(ms.ToArray());
        }

        private XDocument Read(ZipArchiveEntry entry)
        {
            var xmlReader = XmlReader.Create(entry.Open(), new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                Async = true
            });
            return XDocument.Load(xmlReader);
        }

        private XDocument Read(ZipArchive archive, string fileName)
        {
            return Read(archive.GetEntry(fileName));
        }

        
    }
}
