﻿using FontInfo;
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
using Vortice.DirectWrite;
using static Vortice.DirectWrite.DWrite;

namespace ZoDream.Reader.Repositories
{
    public class Disk : IDiskRepository<StorageFolder, StorageFile>
    {
        public StorageFolder BaseFolder;
        public StorageFolder BookFolder { get; private set; }

        public StorageFolder ThemeFolder { get; private set; }


        StorageFolder IDiskRepository<StorageFolder, StorageFile>.BaseFolder => throw new NotImplementedException();

        public async Task<StorageFile> TxtFileName(string fileId)
        {
            return await BookFolder.GetFileAsync(fileId);
        }


        public async Task<StorageFile> FontFileName(string fileId)
        {
            return await BookFolder.GetFileAsync(fileId);
        }


        public async Task<BookItem> AddTxt(StorageFile file)
        {
            var name = file.Name.Substring(0, file.Name.IndexOf('.'));
            var fileId = file.Name;
            await file.CopyAsync(BookFolder);
            return new BookItem(name, fileId);
        }

        public async Task<FontItem> AddFont(StorageFile file)
        {
            var name = file.Name.Substring(0, file.Name.IndexOf('.'));
            var fileId = file.Name;
            var tempfile = await file.CopyAsync(ThemeFolder, file.Name, NameCollisionOption.ReplaceExisting);
            //var factory = DWriteCreateFactory<IDWriteFactory>();
            //var fontRef = factory.CreateFontFileReference(tempfile.Path);
            //fontRef.Analyze(out var isSupprted, out var fontFileType, out var fontFaceType, out var numberOfFaces);
            //if (!isSupprted)
            //{
            //    return null;
            //}
            //var fontFace = factory.CreateFontFace(fontFaceType, new []{ fontRef });
            var font = await Font.CreateAsync(tempfile.Path);
            return new FontItem(font.Details.Family)
            {
                FileName = fileId,
            };
        }



        public Disk(StorageFolder folder = null)
        {
            BaseFolder = folder == null ? ApplicationData.Current.LocalFolder : folder;
            Init();
        }

        private async void Init()
        {
            BookFolder = await BaseFolder.CreateFolderAsync("txt", CreationCollisionOption.OpenIfExists);
            ThemeFolder = await BaseFolder.CreateFolderAsync("theme", CreationCollisionOption.OpenIfExists);
        }

        public async Task<StorageFile> CreateDatabaseAsync()
        {
            return await BaseFolder.CreateFileAsync("zodream.db", CreationCollisionOption.OpenIfExists);
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

        public async Task<StorageFile> GetBookAsync(BookItem item)
        {
            return await BookFolder.GetFileAsync(item.FileName);
        }

        public Task<string> GetBookPathAsync(BookItem item)
        {
            return Task.FromResult(Path.Combine(BookFolder.Path, item.FileName));
        }

        public Task<BookItem> AddBookAsync(StorageFile file)
        {
            return AddTxt(file);
        }

        public async Task DeleteBookAsync(BookItem item)
        {
            var file = await GetBookAsync(item);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        public Task<FontItem> AddFontAsync(StorageFile file)
        {
            return AddFont(file);
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
            var files = await ThemeFolder.GetFilesAsync();
            foreach (var item in files)
            {
                if (!item.Name.Contains(".ttf"))
                {
                    continue;
                }
                var font = await Font.CreateAsync(item.Path);
                items.Add(new FontItem(font.Details.Family)
                {
                    FileName = item.Name,
                });

            }
            return items;
        }
    }
}
