using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Storage;

namespace ZoDream.Reader.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public DatabaseRepository(StorageFile dbFile)
        {
            AppData.DefaultFileName = Path.Combine(ApplicationData.Current.LocalFolder.Path, "setting.xml");
            connection = new Database(new SqliteConnection($"Data Source={dbFile.Path}"));
        }
        private readonly IDatabase connection;

        public void DeleteBook(object id)
        {
            connection.Delete<BookEntity>(id);
        }

        public void DeleteBook(BookItem item)
        {
            DeleteBook(item.Id);
        }
        public IList<BookItem> GetBooks()
        {
            return connection.Fetch<BookItem>(@"SELECT 
                    Id,Name,Cover,FileName,Position 
                    FROM Book ORDER BY UpdatedAt DESC");
        }

        public void AddBook(BookItem item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            
            connection.Insert(item);
        }

        public void UpdateBook(BookItem item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            connection.Update(item);
        }


        public async Task<AppOption> LoadSettingAsync()
        {
            var option = await AppData.LoadAsync<AppOption>();
            if (option == null)
            {
                return new AppOption();
            }
            return option;
        }

        public async Task SaveSettingAsync(AppOption data)
        {
            await AppData.SaveAsync(data);
        }


        public void Dispose()
        {
            connection.Dispose();
        }


        public void Initialize()
        {
            new SQLMigration(connection).Up();
        }

        
    }
}
