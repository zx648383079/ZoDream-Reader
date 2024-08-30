using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface IDiskRepository
    {
        public Task<IDatabaseRepository> CreateDatabaseAsync();
        public Task<IDatabaseRepository> OpenDatabaseAsync();

        public Task<string> GetBookPathAsync(INovel item);

        public Task<string> GetFontFamilyAsync(string fontName);

        public Task<string> GetFileUriAsync(string fileName);
        public Task<string> GetFilePathAsync(string fileName);

        public Task<INovel?> AddBookAsync<T>(T file);

        public Task<string?> AddImageAsync<T>(T file);

        public Task DeleteBookAsync(INovel item);


        public Task<FontItem?> AddFontAsync<T>(T file);

        public Task ClearThemeAsync();

        public Task<IList<FontItem>> GetFontsAsync();
  
    }
}
