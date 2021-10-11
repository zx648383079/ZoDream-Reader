using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface IDiskRepository<FolderT, FileT>
    {
        public FolderT BaseFolder { get; }

        public FolderT BookFolder { get; }

        public FolderT ThemeFolder {  get; }

        public Task<FileT> CreateDatabaseAsync();

        public Task<FileT> GetBookAsync(BookItem item);

        public Task<string> GetBookPathAsync(BookItem item);

        public Task<BookItem?> AddBookAsync(FileT file);

        public Task<string?> AddImageAsync(FileT file);

        public Task DeleteBookAsync(BookItem item);

        public Task<FontItem?> AddFontAsync(FileT file);

        public Task ClearThemeAsync();

        public Task<IList<FontItem>> GetFontsAsync();
  
    }
}
