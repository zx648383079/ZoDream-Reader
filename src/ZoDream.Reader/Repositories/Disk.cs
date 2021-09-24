using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using Vortice.DirectWrite;
using static Vortice.DirectWrite.DWrite;

namespace ZoDream.Reader.Repositories
{
    public class Disk: IDiskRepository<string, string>
    {
        public string BaseFolder { get; private set; }

        public string BookFolder { get; private set; }

        public string FontFolder { get; private set; }


        public string TxtFileName(string fileId)
        {
            return Path.Join(BookFolder, fileId);
        }

        public Task<string> CreateDatabaseAsync()
        {
            var file = Path.Combine(BaseFolder, "zodream.db");
            using (var fs = new FileStream(file, FileMode.OpenOrCreate))
            {}
            return Task.FromResult(file);
        }


        public string FontFileName(string fileId)
        {
            return Path.Join(FontFolder, fileId);
        }

        public IList<FontItem> GetFonts()
        {
            var factory = DWriteCreateFactory<IDWriteFactory>();

            var fontCollection = factory.GetSystemFontCollection(false);
            var familCount = fontCollection.FontFamilyCount;
            var items = new List<FontItem>();
            for (int i = 0; i < familCount; i++)
            {
                var fontFamily = fontCollection.GetFontFamily(i);
                var familyNames = fontFamily.FamilyNames;
                int index;
                if (!familyNames.FindLocaleName(CultureInfo.CurrentCulture.Name, out index))
                {
                    if (!familyNames.FindLocaleName("en-us", out index))
                    {
                        index = 0;
                    }
                }

                string name = familyNames.GetString(index);
                items.Add(new FontItem(name));
            }
            fontCollection.Dispose();
            factory.Dispose();
            return items;
        }

        public BookItem? AddTxt(string file)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                return null;
            }
            var name = info.Name.Replace(info.Extension, "");
            var fileId = info.Name;
            if (info.DirectoryName != null && info.DirectoryName.StartsWith(BookFolder))
            {
                return new BookItem(name, fileId);
            }
            var fileInfo = info.CopyTo(TxtFileName(info.Name), true);
            return new BookItem(name, fileId);
        }

        public FontItem? AddFont(string file)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                return null;
            }
            var name = info.Name.Replace(info.Extension, "");
            var fileId = info.Name;
            if (info.DirectoryName != null && info.DirectoryName.StartsWith(FontFolder))
            {
                return new FontItem(name)
                {
                    FileName = fileId,
                };
            }
            var fileInfo = info.CopyTo(FontFileName(info.Name), true);
            var pfc = new System.Drawing.Text.PrivateFontCollection();
            pfc.AddFontFile(file);
            var item = pfc.Families.FirstOrDefault();
            if (item != null)
            {
                name = item.Name;
            }
            return new FontItem(name)
            {
                FileName = fileId,
            };
        }

        public Task<string> GetBookAsync(BookItem item)
        {
            return Task.FromResult(TxtFileName(item.FileName));
        }

        public Task<string> GetBookPathAsync(BookItem item)
        {
            return GetBookAsync(item);
        }

        public Task<BookItem?> AddBookAsync(string file)
        {
            return Task.Factory.StartNew(() => AddTxt(file));
        }

        public async Task DeleteBookAsync(BookItem item)
        {
            var file = await GetBookAsync(item);
            File.Delete(file);
        }

        public Task<FontItem?> AddFontAsync(string file)
        {
            return Task.Factory.StartNew(() => AddFont(file));
        }

        public Task ClearFontAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var item in Directory.GetFileSystemEntries(FontFolder))
                {
                    File.Delete(item);
                }
            });
        }

        public Task<IList<FontItem>> GetFontsAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                return GetFonts();
            });
        }

        public Disk(string? folder = null)
        {
            BaseFolder = string.IsNullOrWhiteSpace(folder) ? AppDomain.CurrentDomain.BaseDirectory : folder;
            BookFolder = Path.Join(BaseFolder, "txt");
            FontFolder = Path.Join(BaseFolder, "font");
            Directory.CreateDirectory(BookFolder);
            Directory.CreateDirectory(FontFolder);
        }
    }
}
