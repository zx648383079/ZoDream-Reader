﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Storage;

namespace ZoDream.Reader.Repositories
{
    public class Database: IDatabaseRepository
    {
        public Database(string dbFile)
        {
            connection = new SqliteConnection($"Data Source={dbFile}");
            connection.Open();
        }
        private SqliteConnection connection;
        

        public void DeleteBook(object id)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                @"DELETE FROM Book WHERE Id=:id";
            command.Parameters.AddWithValue(":id", id); ;
            command.ExecuteNonQuery();
        }

        public void DeleteBook(BookItem item)
        {
            DeleteBook(item.Id);
        }

        public IList<BookItem> GetBooks()
        {
            var items = new List<BookItem>();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT 
                    Id,Name,Cover,FileName,Position 
                    FROM Book ORDER BY UpdatedAt DESC";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new BookItem(reader.GetString(1), reader.GetString(3))
                    {
                        Id = reader.GetInt32(0),
                        Cover = reader.GetString(2),
                        Position = new PositionItem(reader.GetString(4)),
                    });
                }
            }
            return items;
        }

        public void AddBook(BookItem item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO Book (Name,Cover,FileName,Position,CreatedAt,UpdatedAt)
                  VALUES (:name,:cover,:file,:position,:time,:time)";
            command.Parameters.AddWithValue(":name", item.Name);
            command.Parameters.AddWithValue(":cover", item.Cover);
            command.Parameters.AddWithValue(":file", item.FileName);
            command.Parameters.AddWithValue(":position", item.Position.ToString());
            command.Parameters.AddWithValue(":time", DateTime.Now);
            command.ExecuteNonQuery();
            var query = connection.CreateCommand();
            command.CommandText = "select last_insert_rowid()";
            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void UpdateBook(BookItem item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE Book 
                    SET Name=:name,
                        Cover=:cover,Position=:position,UpdatedAt=:time
                  WHERE Id=:id";
            command.Parameters.AddWithValue(":name", item.Name);
            command.Parameters.AddWithValue(":cover", item.Cover);
            command.Parameters.AddWithValue(":id", item.Id);
            command.Parameters.AddWithValue(":position", item.Position.ToString());
            command.Parameters.AddWithValue(":time", DateTime.Now);
            command.ExecuteNonQuery();
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

        public static void Initialize(string dbFile)
        {
            using (var db = new SqliteConnection($"Data Source={dbFile}"))
            {
                db.Open();
                var sql = @"
CREATE TABLE IF NOT EXISTS Book (
    Id    INTEGER NOT NULL,
	Name  TEXT NOT NULL,
	Cover TEXT NOT NULL,
	FileName  TEXT NOT NULL,
	Position  TEXT NOT NULL,
	CreatedAt NUMERIC NOT NULL,
	UpdatedAt BLOB NOT NULL,
	PRIMARY KEY(Id AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS Setting (
    Name  TEXT NOT NULL,
	Value TEXT NOT NULL,
	PRIMARY KEY(Name)
);
";
                var createTable = new SqliteCommand(sql, db);
                var q = createTable.ExecuteReader();
            }
        }

    }
}
