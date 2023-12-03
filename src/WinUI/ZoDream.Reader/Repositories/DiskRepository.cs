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
            BookFolder = BaseFolder;//await BaseFolder.CreateFolderAsync("txt", CreationCollisionOption.OpenIfExists);
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

        public async Task<INovel?> AddBookAsync<T>(T file)
        {
            if (file is not StorageFile src)
            {
                return null;
            }
            var name = src.Name[..src.Name.IndexOf('.')];
            var fileId = src.Name;
            if (!src.Path.StartsWith(BookFolder.Path))
            {
                await src.CopyAsync(BookFolder);
            }
            return new BookEntity() { 
                Name = name,
                Id = fileId,
                FileName = fileId
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


        public Task<string?> AddImageAsync<T>(T file)
        {
            throw new NotImplementedException();
        }

    }
}
