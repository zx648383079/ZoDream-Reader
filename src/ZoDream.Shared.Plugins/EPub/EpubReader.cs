using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.EPub
{
    public class EPubReader(Stream input) : INovelReader
    {
        internal const string EPUB_CONTAINER_FILE_PATH = "META-INF/container.xml";
        const string NcxPrefixNamespace = "http://www.daisy.org/z3986/2005/ncx/";
        const string OpfPrefixNamespace = "http://www.idpf.org/2007/opf";
        const string XHtmlPrefixNamespace = "http://www.w3.org/1999/xhtml";

        internal static XNamespace Ncx => NcxPrefixNamespace;
        internal static XNamespace Opf => OpfPrefixNamespace;
        internal static XNamespace XHtml => XHtmlPrefixNamespace;

        public INovelBasic ReadBasic()
        {
            var data = new NovelBasic();
            using var archive = new ZipArchive(input, ZipArchiveMode.Read);
            Read(archive, data);
            return data;
        }

        public INovelDocument Read()
        {
            using var archive = new ZipArchive(input, ZipArchiveMode.Read);
            var rootFile = GetRootFileName(archive);
            if (rootFile is null)
            {
                return null;
            }
            var doc = Read(archive, rootFile);
            var opfNamespace = Opf;
            var root = doc.Element(opfNamespace + "package");
            if (root is null)
            {
                return null;
            }
            var maps = new Dictionary<string, string>();
            var folder = Path.GetDirectoryName(rootFile);
            foreach (var item in root.Element(opfNamespace + "manifest").Elements())
            {
                maps.Add(item.Attribute("id").Value, folder + "/" + item.Attribute("href").Value);
            }
            var novel = new RichDocument();
            foreach (var item in root.Element(opfNamespace + "metadata").Elements())
            {
                switch (item.Name.LocalName.ToLowerInvariant())
                {
                    case "title":
                        novel.Name = item.Value;
                        break;
                    case "description":
                        novel.Brief = item.Value;
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
            var ncxNamespace = Ncx;
            var ncxDoc = Read(archive, ncx);
            var pointName = ncxNamespace + "navPoint";
            foreach (var item in ncxDoc.Element(ncxNamespace + "ncx")
                .Element(ncxNamespace + "navMap").Elements(pointName))
            {
                if (item.Element(pointName) is null)
                {
                    novel.Add(ReadDocument(archive, 
                        folder + "/" + item.Element(ncxNamespace + "content").Attribute("src").Value,
                        item.Element(ncxNamespace + "navLabel").Element(ncxNamespace + "text").Value));
                    continue;
                }
                var volume = new NovelVolume(item.Element(ncxNamespace + "navLabel").Element(ncxNamespace + "text").Value);
                novel.Add(volume);
                foreach (var it in item.Elements(pointName))
                {
                    volume.Add(ReadDocument(archive,
                        folder + "/" + it.Element(ncxNamespace + "content").Attribute("src").Value,
                        it.Element(ncxNamespace + "navLabel").Element(ncxNamespace + "text").Value));
                    
                }
            }
            return novel;
        }

        private void Read(ZipArchive archive, NovelBasic novel)
        {
            var rootFile = GetRootFileName(archive);
            if (rootFile is null)
            {
                return;
            }
            var doc = Read(archive, rootFile);
            var opfNamespace = Opf;
            var root = doc.Element(opfNamespace + "package");
            if (root is null)
            {
                return;
            }
            var maps = new Dictionary<string, string>();
            var folder = Path.GetDirectoryName(rootFile);
            foreach (var item in root.Element(opfNamespace + "manifest").Elements())
            {
                maps.Add(item.Attribute("id").Value, folder + "/" + item.Attribute("href").Value);
            }
            foreach (var item in root.Element(opfNamespace + "metadata").Elements())
            {
                switch (item.Name.LocalName.ToLowerInvariant())
                {
                    case "title":
                        novel.Name = item.Value;
                        break;
                    case "description":
                        novel.Brief = item.Value;
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
        }

        private static INovelSection ReadDocument(ZipArchive archive, 
            string fileName, string title)
        {
            var doc = Read(archive, fileName);
            var xHtmlNamespace = XHtml;
            var root = doc.Root;
            var res = new NovelSection(root?.Element(xHtmlNamespace + "head")
                ?.Element(xHtmlNamespace + "title")?.Value ?? title);
            foreach (var item in root.Element(xHtmlNamespace + "body").Elements())
            {
                var text = item.Value;
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }
                res.Items.Add(new NovelTextBlock(text));
            }
            return res;
        }

        public void Dispose()
        {
            input.Dispose();
        }

        internal static string? GetRootFileName(ZipArchive archive)
        {
            var reader = Read(archive, EPUB_CONTAINER_FILE_PATH);
            XNamespace cnsNamespace = "urn:oasis:names:tc:opendocument:xmlns:container";
            return reader.Element(cnsNamespace + "container")?.Element(cnsNamespace + "rootfiles")?.Element(cnsNamespace + "rootfile")?.
                Attribute("full-path").Value;
        }

        private static XDocument Read(ZipArchiveEntry entry)
        {
            var xmlReader = XmlReader.Create(entry.Open(), new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                Async = true
            });
            return XDocument.Load(xmlReader);
        }

        internal static XDocument Read(ZipArchive archive, string fileName)
        {
            return Read(archive.GetEntry(fileName));
        }

        internal static Stream GetImageResource(ZipArchive archive, string fileName)
        {
            var entry = archive.GetEntry(fileName);
            var ms = new MemoryStream();
            entry.Open().CopyTo(ms);
            return ms;
        }
    }
}
