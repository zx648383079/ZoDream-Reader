using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.EPub
{
    public class EPubWriter(INovelDocument data) : INovelWriter
    {
        const string _identifier = "zre-novel-id";
        const string _creator = "zodream";

        private readonly XmlWriterSettings _settings = new XmlWriterSettings()
        {
            Indent = true,
            Encoding = new UTF8Encoding(false),
        };

        private readonly Guid _guid = Guid.NewGuid();

        public void Write(Stream output)
        {
            using var archive = new ZipArchive(output, ZipArchiveMode.Create);
            WriteMime(archive);
            WriteContainer(archive);

            WriteCover(archive);


            var i = 0;
            foreach (var volume in data.Items)
            {
                Write(archive, i++, volume);
            }
            WriteBrief(archive);
            WriteNcx(archive);
            WriteOpf(archive);
        }

        private void WriteCover(ZipArchive archive)
        {
            var entry = archive.CreateEntry("OEBPS/Images/cover.jpg");
            using (var fs = entry.Open())
            {
                data.Cover?.CopyTo(fs);
            }

            entry = archive.CreateEntry("OEBPS/Text/cover.xhtml");
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

                writer.WriteStartElement("html");
                writer.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
                writer.WriteAttributeString("xml:lang", "zh-CN");
                writer.WriteStartElement("head");

                writer.WriteStartElement("title");
                writer.WriteString("封面");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteStartElement("body");

                writer.WriteStartElement("div");
                writer.WriteStartElement("img");
                writer.WriteAttributeString("src", "../Images/cover.jpg");
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }

        private void WriteOpf(ZipArchive archive)
        {
            var entry = archive.CreateEntry("OEBPS/content.opf");
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteStartElement("package");
                writer.WriteAttributeString("version", "2.0");
                writer.WriteAttributeString("unique-identifier", _identifier);
                writer.WriteAttributeString("xmlns", "http://www.idpf.org/2007/opf");
                writer.WriteStartElement("metadata");
                writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
                writer.WriteAttributeString("xmlns:opf", "http://www.idpf.org/2007/opf");
                writer.WriteStartElement("dc:title");
                writer.WriteString(data.Name);
                writer.WriteEndElement();

                writer.WriteStartElement("dc:creator");
                writer.WriteAttributeString("opf:file-as", _creator);
                writer.WriteAttributeString("opf:role", "aut");
                writer.WriteString(data.Author ?? _creator);
                writer.WriteEndElement(); // dc:creator

                writer.WriteStartElement("dc:identifier");
                writer.WriteAttributeString("id", _identifier);
                writer.WriteAttributeString("opf:scheme", "UUID");
                writer.WriteString($"urn:uuid:{_guid}");
                writer.WriteEndElement(); // dc:identifier

                writer.WriteStartElement("dc:description");
                writer.WriteString(data.Brief);
                writer.WriteEndElement(); // dc:description

                writer.WriteStartElement("dc:date");
                writer.WriteString(DateTime.Now.ToString());
                writer.WriteEndElement(); // dc:date

                writer.WriteStartElement("dc:language");
                writer.WriteString("zh-CN");
                writer.WriteEndElement(); // dc:language

                writer.WriteStartElement("meta");
                writer.WriteAttributeString("name", "cover");
                writer.WriteAttributeString("content", "cover.jpg");
                writer.WriteEndElement(); // meta

                writer.WriteEndElement(); // metadata

                writer.WriteStartElement("manifest");

                writer.WriteStartElement("item");
                writer.WriteAttributeString("id", "cover.xhtml");
                writer.WriteAttributeString("href", "Text/cover.xhtml");
                writer.WriteAttributeString("media-type", "application/xhtml+xml");
                writer.WriteEndElement(); // item

                writer.WriteStartElement("item");
                writer.WriteAttributeString("id", $"{nameof(data.Brief)}.xhtml");
                writer.WriteAttributeString("href", $"Text/{nameof(data.Brief)}.xhtml");
                writer.WriteAttributeString("media-type", "application/xhtml+xml");
                writer.WriteEndElement(); // item

                // 
                for (int i = 0; i < data.Items.Count; i++)
                {
                    var volume = data.Items[i];
                    if (!string.IsNullOrEmpty(volume.Name))
                    {
                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("id", $"A-{i}.xhtml");
                        writer.WriteAttributeString("href", $"Text/A-{i}.xhtml");
                        writer.WriteAttributeString("media-type", "application/xhtml+xml");
                        writer.WriteEndElement(); // item
                    }
                    for (int j = 0; j < volume.Count; j++)
                    {
                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("id", $"A-{i}-{j}.xhtml");
                        writer.WriteAttributeString("href", $"Text/A-{i}-{j}.xhtml");
                        writer.WriteAttributeString("media-type", "application/xhtml+xml");
                        writer.WriteEndElement(); // item
                    }
                }


                writer.WriteStartElement("item");
                writer.WriteAttributeString("id", "ncx");
                writer.WriteAttributeString("href", "toc.ncx");
                writer.WriteAttributeString("media-type", "application/x-dtbncx+xml");
                writer.WriteEndElement(); // item

                writer.WriteStartElement("item");
                writer.WriteAttributeString("id", "cover.jpg");
                writer.WriteAttributeString("href", "Images/cover.jpg");
                writer.WriteAttributeString("media-type", "image/jpeg");
                writer.WriteEndElement(); // cover

                //writer.WriteStartElement("item");
                //writer.WriteAttributeString("id", "style.css");
                //writer.WriteAttributeString("href", "Styles/style.css");
                //writer.WriteAttributeString("media-type", "text/css");
                //writer.WriteEndElement(); // css

                //writer.WriteStartElement("item");
                //writer.WriteAttributeString("id", "font.ttf");
                //writer.WriteAttributeString("href", "Fonts/font.ttf");
                //writer.WriteAttributeString("media-type", "application/x-font-ttf");
                //writer.WriteEndElement(); // font

                writer.WriteEndElement(); // manifest

                writer.WriteStartElement("spine");
                writer.WriteAttributeString("toc", "ncx");

                writer.WriteStartElement("itemref");
                writer.WriteAttributeString("idref", "cover.xhtml");
                writer.WriteEndElement(); // itemref

                writer.WriteStartElement("itemref");
                writer.WriteAttributeString("idref", $"{nameof(data.Brief)}.xhtml");
                writer.WriteEndElement(); // itemref

                // 
                for (int i = 0; i < data.Items.Count; i++)
                {
                    var volume = data.Items[i];
                    if (!string.IsNullOrEmpty(volume.Name))
                    {
                        writer.WriteStartElement("itemref");
                        writer.WriteAttributeString("idref", $"A-{i}.xhtml");
                        writer.WriteEndElement(); // itemref
                    }
                    for (int j = 0; j < volume.Count; j++)
                    {
                        writer.WriteStartElement("itemref");
                        writer.WriteAttributeString("idref", $"A-{i}-{j}.xhtml");
                        writer.WriteEndElement(); // itemref
                    }
                }

                writer.WriteEndElement(); // spine

                writer.WriteStartElement("guide");
                writer.WriteStartElement("reference");
                writer.WriteAttributeString("type", "cover");
                writer.WriteAttributeString("title", "封面");
                writer.WriteAttributeString("href", "Text/cover.xhtml");
                writer.WriteEndElement(); // reference
                writer.WriteEndElement(); // guide

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void WriteNcx(ZipArchive archive)
        {
            var entry = archive.CreateEntry("OEBPS/toc.ncx");
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteDocType("ncx", "-//NISO//DTD ncx 2005-1//EN", "http://www.daisy.org/z3986/2005/ncx-2005-1.dtd", null);

                writer.WriteStartElement("ncx");
                writer.WriteAttributeString("version", "2005-1");
                writer.WriteAttributeString("xmlns", "http://www.daisy.org/z3986/2005/ncx/");
                writer.WriteStartElement("head");

                writer.WriteStartElement("meta");
                writer.WriteAttributeString("content", $"urn:uuid:{_guid}");
                writer.WriteAttributeString("name", "dtb:uid");
                writer.WriteEndElement();
                writer.WriteStartElement("meta");
                writer.WriteAttributeString("content", "2");
                writer.WriteAttributeString("name", "dtb:depth");
                writer.WriteEndElement();
                writer.WriteStartElement("meta");
                writer.WriteAttributeString("content", "0");
                writer.WriteAttributeString("name", "dtb:totalPageCount");
                writer.WriteEndElement();
                writer.WriteStartElement("meta");
                writer.WriteAttributeString("content", "0");
                writer.WriteAttributeString("name", "dtb:maxPageNumber");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteStartElement("docTitle");

                writer.WriteStartElement("text");
                writer.WriteString(data.Name);
                writer.WriteEndElement();


                writer.WriteEndElement();

                writer.WriteStartElement("navMap");

                var order = 0;
                writer.WriteStartElement("navPoint");
                writer.WriteAttributeString("id", $"navPoint-{++order}");
                writer.WriteAttributeString("playOrder", $"{order}");
                writer.WriteStartElement("navLabel");
                writer.WriteStartElement("text");
                writer.WriteString("【内容简介】");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("content");
                writer.WriteAttributeString("src", $"Text/{nameof(data.Brief)}.xhtml");
                writer.WriteEndElement();
                writer.WriteEndElement();


                for (int i = 0; i < data.Items.Count; i++)
                {
                    var volume = data.Items[i];
                    if (!string.IsNullOrEmpty(volume.Name))
                    {
                        writer.WriteStartElement("navPoint");
                        writer.WriteAttributeString("id", $"navPoint-{++order}");
                        writer.WriteAttributeString("playOrder", $"{order}");
                        writer.WriteStartElement("navLabel");
                        writer.WriteStartElement("text");
                        writer.WriteString(volume.Name);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteStartElement("content");
                        writer.WriteAttributeString("src", $"Text/A-{i}.xhtml");
                        writer.WriteEndElement();
                    }
                    for (int j = 0; j < volume.Count; j++)
                    {
                        writer.WriteStartElement("navPoint");
                        writer.WriteAttributeString("id", $"navPoint-{++order}");
                        writer.WriteAttributeString("playOrder", $"{order}");
                        writer.WriteStartElement("navLabel");
                        writer.WriteStartElement("text");
                        writer.WriteString(volume[j].Title);
                        writer.WriteEndElement();
                        writer.WriteEndElement(); // navLabel
                        writer.WriteStartElement("content");
                        writer.WriteAttributeString("src", $"Text/A-{i}-{j}.xhtml");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    if (!string.IsNullOrEmpty(volume.Name))
                    {
                        writer.WriteEndElement();
                    }
                }


                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }

        private void WriteContainer(ZipArchive archive)
        {
            var entry = archive.CreateEntry("META-INF/container.xml");
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteStartElement("container");
                writer.WriteAttributeString("version", "1.0");
                writer.WriteAttributeString("xmlns", "urn:oasis:names:tc:opendocument:xmlns:container");
                writer.WriteStartElement("rootfiles");
                writer.WriteStartElement("rootfile");
                writer.WriteAttributeString("full-path", "OEBPS/content.opf");
                writer.WriteAttributeString("media-type", "application/oebps-package+xml");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void WriteMime(ZipArchive archive)
        {
            var entry = archive.CreateEntry("mimetype");
            using (var writer = new StreamWriter(entry.Open()))
            {
                writer.Write("application/epub+zip");
            }
        }

        private void WriteBrief(ZipArchive archive)
        {
            var entry = archive.CreateEntry($"OEBPS/Text/{nameof(data.Brief)}.xhtml");
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

                writer.WriteStartElement("html");
                writer.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
                writer.WriteAttributeString("xml:lang", "zh-CN");
                writer.WriteStartElement("head");

                writer.WriteStartElement("title");
                writer.WriteString("内容简介");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteStartElement("body");

                writer.WriteStartElement("h2");
                writer.WriteString("内容简介");
                writer.WriteEndElement();

                foreach (var item in data.Brief.Split('\n'))
                {
                    writer.WriteStartElement("p");
                    writer.WriteString(item);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }

        private void Write(ZipArchive archive, int index, INovelVolume volume)
        {
            if (!string.IsNullOrEmpty(volume.Name))
            {
                var entry = archive.CreateEntry($"OEBPS/Text/A-{index}.xhtml");
                using (var writer = XmlWriter.Create(entry.Open(), _settings))
                {
                    writer.WriteStartDocument(false);
                    writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

                    writer.WriteStartElement("html");
                    writer.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
                    writer.WriteAttributeString("xml:lang", "zh-CN");
                    writer.WriteStartElement("head");

                    writer.WriteStartElement("title");
                    writer.WriteString(volume.Name);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteStartElement("body");

                    writer.WriteStartElement("h2");
                    writer.WriteString(volume.Name);
                    writer.WriteEndElement();


                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }

            for (int j = 0; j < volume.Count; j++)
            {
                var entry = archive.CreateEntry($"OEBPS/Text/A-{index}-{j}.xhtml");
                Write(entry, volume[j]);
            }
        }

        private void Write(ZipArchiveEntry entry, INovelSection section)
        {
            using (var writer = XmlWriter.Create(entry.Open(), _settings))
            {
                writer.WriteStartDocument(false);
                writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

                writer.WriteStartElement("html");
                writer.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
                writer.WriteAttributeString("xml:lang", "zh-CN");
                writer.WriteStartElement("head");

                writer.WriteStartElement("title");
                writer.WriteString(section.Title);
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteStartElement("body");

                writer.WriteStartElement("h2");
                writer.WriteString(section.Title);
                writer.WriteEndElement();

                foreach (var item in section.Items)
                {
                    if (item is INovelTextBlock text)
                    {
                        writer.WriteStartElement("p");
                        writer.WriteString(text.Text);
                        writer.WriteEndElement();
                    } 
                    else if (item is INovelImageBlock image)
                    {
                        writer.WriteStartElement("div");
                        writer.WriteStartElement("img");
                        writer.WriteAttributeString("src", image.Source.GetHashCode().ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }

        public void Dispose()
        {
        }
    }
}
