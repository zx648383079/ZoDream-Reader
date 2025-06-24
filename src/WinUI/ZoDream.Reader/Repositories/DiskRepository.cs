using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Font;
using Microsoft.Graphics.Canvas.Text;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using Windows.Graphics.Imaging;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Umd;
using ZoDream.Shared.Plugins.Pdf;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.Repositories
{
    public class DiskRepository : IDiskRepository
    {
        public StorageFolder BaseFolder { get; private set; }
        public StorageFolder BookFolder { get; private set; }

        public StorageFolder ThemeFolder { get; private set; }

        public async Task<StorageFile> TxtFileName(string fileId)
        {
            return await BookFolder.GetFileAsync(fileId);
        }


        public async Task<StorageFile> FontFileName(string fileId)
        {
            return await BookFolder.GetFileAsync(fileId);
        }


        public async Task<FontItem?> AddFontAsync<T>(T file)
        {
            if (file is not StorageFile src)
            {
                return null;
            }
            var name = src.Name[..src.Name.IndexOf('.')];
            var fileId = src.Name;
            var tempFile = await src.CopyAsync(ThemeFolder, src.Name, NameCollisionOption.ReplaceExisting);
            //var factory = DWriteCreateFactory<IDWriteFactory>();
            //var fontRef = factory.CreateFontFileReference(tempFile.Path);
            //fontRef.Analyze(out var isSupported, out var fontFileType, out var fontFaceType, out var numberOfFaces);
            //if (!isSupported)
            //{
            //    return null;
            //}
            //var fontFace = factory.CreateFontFace(fontFaceType, new []{ fontRef });
            var items = await FontHelper.GetFontFamilyAsync(tempFile.Path);
            return new FontItem(items.FirstOrDefault().Name)
            {
                FileName = fileId,
            };
        }



        public DiskRepository(StorageFolder? folder = null)
        {
            BaseFolder = folder ?? ApplicationData.Current.LocalFolder;
            Init();
        }

        private async void Init()
        {
            BookFolder = BaseFolder;
            ThemeFolder = await BaseFolder.CreateFolderAsync("theme", CreationCollisionOption.OpenIfExists);
        }

        public async Task<IDatabaseRepository> CreateDatabaseAsync()
        {
            var file = await BaseFolder.CreateFileAsync(AppConstants.DatabaseFileName, CreationCollisionOption.ReplaceExisting);
            var database = new DatabaseRepository(file);
            database.Initialize();
            return database;
        }

        public Task<string> GetFileUriAsync(string fileName)
        {
            return Task.FromResult($"ms-appdata:///local/theme/{fileName}");
        }

        public Task<string> GetFilePathAsync(string fileName)
        {
            return Task.FromResult(Path.Combine(ThemeFolder.Path, fileName));
        }

        public async Task<string> GetFontFamilyAsync(string fontName)
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
            var file = await ThemeFolder.GetFileAsync(font.FileName);
            if (file == null)
            {
                return string.Empty;
            }
            font.FileName = await GetFileUriAsync(font.FileName);
            return font.FontFamily;
        }

        public async Task<StorageFile> GetBookAsync(INovel item)
        {
            return await BookFolder.GetFileAsync(item.FileName);
        }

        public Task<string> GetBookPathAsync(INovel item)
        {
            return Task.FromResult(Path.Combine(BookFolder.Path, item.FileName));
        }

        private static async Task<IStorageFile> CopyOrReplaceFileAsync(IStorageFile src, IStorageFolder folder)
        {
            try
            {
                var dst = await folder.GetFileAsync(src.Name);
                await src.CopyAndReplaceAsync(dst);
                return dst;
            }
            catch (FileNotFoundException)
            {
            }
            return await src.CopyAsync(folder);
        }

        public async Task<INovel?> AddBookAsync<T>(T file)
        {
            if (file is not IStorageFile src)
            {
                return null;
            }
            var name = src.Name[..src.Name.IndexOf('.')];
            var fileId = src.Name;
            if (!src.Path.StartsWith(BookFolder.Path))
            {
                src = await CopyOrReplaceFileAsync(src, BookFolder);
            }
            var reader = await GetReaderAsync(src.Name, true);
            var (novel, items) = await reader.LoadAsync(new FileSource(src.Path));
            novel ??= new BookEntity();
            novel.Id = fileId;
            novel.FileName = fileId;
            if (string.IsNullOrWhiteSpace(novel.Name))
            {
                new TxtReader().Decode(name, novel);
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
                _ => init ? new TxtReader(await App.GetService<AppViewModel>().Database.GetEnabledChapterRuleAsync()) : new TxtReader()
            };
        }

        public async Task DeleteBookAsync(INovel item)
        {
            var file = await GetBookAsync(item);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
        

        public async Task<string> AddImageAsync(StorageFile file)
        {
            var fileId = file.Name;
            await file.CopyAsync(ThemeFolder, file.Name, NameCollisionOption.ReplaceExisting);
            return fileId;
        }

        public async Task ClearThemeAsync()
        {
            var items = await ThemeFolder.GetItemsAsync();
            foreach (var item in items)
            {
                await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        public async Task<IList<FontItem>> GetFontsAsync()
        {
            var fontCollection = CanvasTextFormat.GetSystemFontFamilies();
            var items = new List<FontItem>();
            foreach (var name in fontCollection)
            {
                items.Add(new FontItem(name));
            }
            var files = await ThemeFolder.GetFilesAsync();
            foreach (var item in files)
            {
                if (!item.Name.Contains(".ttf"))
                {
                    continue;
                }
                var font = await FontHelper.GetFontFamilyAsync(item.Path);
                items.Add(new FontItem(font.FirstOrDefault().Name)
                {
                    FileName = item.Name,
                });

            }
            return items;
        }

        public async Task<IDatabaseRepository> OpenDatabaseAsync()
        {
            return new DatabaseRepository(await BaseFolder.CreateFileAsync(AppConstants.DatabaseFileName, CreationCollisionOption.OpenIfExists));
        }


        public async Task<string?> AddImageAsync<T>(T file)
        {
            if (file is StorageFile f)
            {
                var img = await f.CopyAsync(ThemeFolder);
                return img.Path;
            }
            return null;
        }

        public async Task AddImageAsync(string fileName, Stream stream)
        {
            var bi = new BitmapImage();
            using var s = stream.AsRandomAccessStream();
            await bi.SetSourceAsync(s);
            var wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
            s.Seek(0);
            await wb.SetSourceAsync(s);
            await AddImageAsync(fileName, wb);
        }

        public async Task AddImageAsync(string fileName, byte[] buffer)
        {
            var bi = new BitmapImage();
            using var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(buffer.AsBuffer());
            await bi.SetSourceAsync(stream);
            var wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
            stream.Seek(0);
            await wb.SetSourceAsync(stream);
            await AddImageAsync(fileName, wb);
        }

        public async Task AddImageAsync(string fileName, WriteableBitmap writeable)
        {
            var bitmapEncoder = Path.GetExtension(fileName)[1..].ToLower() switch
            {
                "png" => BitmapEncoder.PngEncoderId,
                "bmp" => BitmapEncoder.BmpEncoderId,
                "tiff" => BitmapEncoder.TiffEncoderId,
                "gif" => BitmapEncoder.GifEncoderId,
                _ => BitmapEncoder.JpegEncoderId
            };
            var file = await ThemeFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            var encoder = await BitmapEncoder.CreateAsync(bitmapEncoder, stream);
            var pixelStream = writeable.PixelBuffer.AsStream();
            byte[] pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                        (uint)writeable.PixelWidth,
                        (uint)writeable.PixelHeight,
                        96.0,
                        96.0,
                        pixels);
            await encoder.FlushAsync();
        }
    }
}
