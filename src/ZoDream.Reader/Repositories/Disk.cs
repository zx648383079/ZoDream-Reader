using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Repositories
{
    public class Disk: IDiskRepository<string, string>
    {
        public string BaseFolder { get; private set; }

        public string BookFolder { get; private set; }

        public string ThemeFolder { get; private set; }


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
            return Path.Join(ThemeFolder, fileId);
        }

        public IList<FontItem> GetFonts()
        {
            return Fonts.SystemFontFamilies.Select(i => new FontItem(i.Source)).ToList();
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
            if (info.DirectoryName != null && info.DirectoryName.StartsWith(ThemeFolder))
            {
                return new FontItem(name)
                {
                    FileName = fileId,
                };
            }
            info.CopyTo(FontFileName(info.Name), true);
            var item = FontHelper.GetFontFamilyAsync(file).GetAwaiter().GetResult().FirstOrDefault();
            if (item != null)
            {
                name = item.Name;
            }
            return new FontItem(name)
            {
                FileName = fileId,
            };
        }

        public Task<string> GetFileUriAsync(string fileName)
        {
            return Task.FromResult(Path.Join(ThemeFolder, fileName));
        }

        public Task<string> GetFilePathAsync(string fileName)
        {
            return GetFileUriAsync(fileName);
        }
        public Task<string> GetFontFamilyAsync(string fontName)
        {
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(fontName))
                {
                    return string.Empty;
                }
                var font = new FontItem(fontName);
                if (string.IsNullOrEmpty(font.FileName))
                {
                    return font.FontFamily;
                }
                var file = Path.Join(ThemeFolder, font.FileName);
                if (!File.Exists(file))
                {
                    return string.Empty;
                }
                font.FileName = file;
                return font.FontFamily;
            });
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

        public Task<string?> AddImageAsync(string file)
        {
            return Task.Factory.StartNew(() =>
            {
                var info = new FileInfo(file);
                if (!info.Exists)
                {
                    return null;
                }
                var name = info.Name.Replace(info.Extension, "");
                var fileId = info.Name;
                if (info.DirectoryName != null && info.DirectoryName.StartsWith(ThemeFolder))
                {
                    return fileId;
                }
                var fileInfo = info.CopyTo(FontFileName(info.Name), true);
                return fileId;
            });
        }

        public Task ClearThemeAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var item in Directory.GetFileSystemEntries(ThemeFolder))
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
            ThemeFolder = Path.Join(BaseFolder, "theme");
            Directory.CreateDirectory(BookFolder);
            Directory.CreateDirectory(ThemeFolder);
        }
    }
}
