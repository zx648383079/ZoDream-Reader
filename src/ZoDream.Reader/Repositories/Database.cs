using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using ZoDream.Reader.Models;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Repositories
{
    public class Database: IDisposable
    {
        public Database()
        {
            connection = new SqliteConnection("Data Source=zodream.db");
            connection.Open();
        }
        private SqliteConnection connection;

        public void Open()
        {
            var command = connection.CreateCommand();
            command.CommandText =
            @"
        SELECT name
        FROM user
        WHERE id = $id
    ";
            command.Parameters.AddWithValue("$id", 1);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    Console.WriteLine($"Hello, {name}!");
                }
            }
        }

        public IList<BookItem> GetBookList()
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
                    items.Add(new BookItem()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Cover = reader.GetString(2),
                        FileName = reader.GetString(3),
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
                item.Cover = BookItem.RandomCover();
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
                item.Cover = BookItem.RandomCover();
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

        public void RemoveBook(BookItem item)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                @"DELETE FROM Book WHERE Id=:id";
            command.Parameters.AddWithValue(":id", item.Id);;
            command.ExecuteNonQuery();
        }

        public void GetSetting()
        {
            throw new NotImplementedException();
        }

        public void SaveSetting()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
