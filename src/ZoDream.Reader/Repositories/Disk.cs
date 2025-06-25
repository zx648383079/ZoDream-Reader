using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Font;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Pdf;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Plugins.Umd;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.Repositories
{
    public class DiskRepository : IDiskRepository
    {
        public DiskRepository(string? folder = null)
        {
            BaseFolder = string.IsNullOrWhiteSpace(folder) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data") : folder;
            BookFolder = BaseFolder;
            ThemeFolder = Path.Join(BaseFolder, "theme");
            if (!Directory.Exists(ThemeFolder))
            {
                Directory.CreateDirectory(BookFolder);
            }
            Directory.CreateDirectory(ThemeFolder);
        }
        public string BaseFolder { get; private set; }

        public string BookFolder { get; private set; }

        public string ThemeFolder { get; private set; }


        public string TxtFileName(string fileId)
        {
            return Path.Join(BookFolder, fileId);
        }



        public string FontFileName(string fileId)
        {
            return Path.Join(ThemeFolder, fileId);
        }

        public IList<FontItem> GetFonts()
        {
            return Fonts.SystemFontFamilies.Select(i => new FontItem(i.Source)).ToList();
        }

        //public BookItem? AddTxt(string file)
        //{
        //    var info = new FileInfo(file);
        //    if (!info.Exists)
        //    {
        //        return null;
        //    }
        //    var name = info.Name.Replace(info.Extension, "");
        //    var fileId = info.Name;
        //    if (info.DirectoryName != null && info.DirectoryName.StartsWith(BookFolder))
        //    {
        //        return new BookItem(name, fileId);
        //    }
        //    var fileInfo = info.CopyTo(TxtFileName(info.Name), true);
        //    return new BookItem(name, fileId);
        //}

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

        //public Task<string> GetBookAsync(BookItem item)
        //{
        //    return Task.FromResult(TxtFileName(item.FileName));
        //}

        //public Task<string> GetBookPathAsync(BookItem item)
        //{
        //    return GetBookAsync(item);
        //}

        //public Task<BookItem?> AddBookAsync(string file)
        //{
        //    return Task.Factory.StartNew(() => AddTxt(file));
        //}

        //public async Task DeleteBookAsync(BookItem item)
        //{
        //    var file = await GetBookAsync(item);
        //    File.Delete(file);
        //}

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

        public Task<IDatabaseRepository> CreateDatabaseAsync()
        {
            var file = Path.Combine(BaseFolder, AppConstants.DatabaseFileName);
            var fs = File.Create(file);
            fs.Dispose();
            var database = new DatabaseRepository(file);
            database.Initialize();
            return Task.FromResult(database as IDatabaseRepository);
        }

        public Task<IDatabaseRepository> OpenDatabaseAsync()
        {
            IDatabaseRepository res = new DatabaseRepository(Path.Combine(BaseFolder, AppConstants.DatabaseFileName));
            return Task.FromResult(res);
        }

        public Task<string> GetBookPathAsync(INovel item)
        {
            return Task.FromResult(Path.Combine(BookFolder, item.FileName));
        }

        public async Task<INovel?> AddBookAsync<T>(T file)
        {
            if (file is not string src)
            {
                return null;
            }
            var name = Path.GetFileNameWithoutExtension(src);
            var fileId = Path.GetFileName(src);
            if (!src.StartsWith(BookFolder))
            {
                src = CopyOrReplaceFile(src, BookFolder);
            }
            var reader = await GetReaderAsync(fileId, true);
            var (novel, items) = await reader.LoadAsync(new FileSource(src));
            novel ??= new BookEntity();
            novel.Id = fileId;
            novel.FileName = fileId;
            if (string.IsNullOrWhiteSpace(novel.Name))
            {
                new TxtSerializer().Decode(name, novel);
            }
            var service = App.GetService<AppViewModel>().Database;
            await service.SaveBookAsync(novel);
            if (items is not null)
            {
                await service.SaveChapterAsync(novel.Id, items);
            }
            return novel;
        }

        public async Task<INovelSerializer> GetReaderAsync(string fileName, bool init = false)
        {
            return Path.GetExtension(fileName)[1..].ToLower() switch
            {
                "epub" => new EPubReader(),
                "umd" => new UmdReader(),
                "pdf" => new PdfReader(),
                _ => init ? new TxtSerializer(await App.GetService<AppViewModel>().Database.GetEnabledChapterRuleAsync()) : new TxtSerializer()
            };
        }


        public async Task DeleteBookAsync(INovel item)
        {
            var file = await GetBookPathAsync(item);
            File.Delete(file);
        }

        public Task<FontItem?> AddFontAsync<T>(T file)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> AddImageAsync<T>(T file)
        {
            if (file is string f)
            {
                var img = Path.Combine(ThemeFolder, Path.GetFileName(f));
                File.Copy(f, img);
                return img;
            }
            return null;
        }

        public async Task AddImageAsync(string fileName, Stream stream)
        {
            using var fs = File.OpenWrite(fileName);
            await stream.CopyToAsync(fs);
        }

        public Task AddImageAsync(string fileName, byte[] buffer)
        {
            using var fs = File.OpenWrite(fileName);
            fs.Write(buffer, 0, buffer.Length);
            return Task.CompletedTask;
        }


        private static string CopyOrReplaceFile(string src, string folder)
        {
            var dist = Path.Combine(folder, Path.GetFileName(src));
            //if (File.Exists(dist))
            //{
            //    return dist;
            //}
            File.Copy(src, dist, true);
            return dist;
        }
    }
}
