using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Helper.Database;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.Helper
{
    public class DatabaseHelper
    {
        public static void ChooseSqlite()
        {
            string dir;
            switch (IntPtr.Size)
            {
                case 8:
                    dir = "x64";
                    break;
                case 4:
                default:
                    dir = "x86";
                    break;
            }
            var files = LocalHelper.GetAllFile(AppDomain.CurrentDomain.BaseDirectory + "\\" + dir);
            foreach (var file in files)
            {
                File.Copy(file, file.Replace($"\\{dir}", ""), true);
            }
        }

        public static void Init()
        {
            SqLiteHelper.CreateCommand(
                "CREATE TABLE IF NOT EXISTS BookItem ( Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100) UNIQUE, Image VARCHAR(100), Description VARCHAR(255), Author VARCHAR(45), Source VARCHAR(20) DEFAULT '本地', Kind VARCHAR(45) DEFAULT '其他', Url VARCHAR(255), `Index` INT DEFAULT 0, Count INT, Time DATETIME);"
                ).ExecuteNonQuery();
            SqLiteHelper.CreateCommand(
                "CREATE TABLE IF NOT EXISTS ChapterItem (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100), Content TEXT NULL, BookId INT, Url VARCHAR(255));"
                ).ExecuteNonQuery();
            SqLiteHelper.CreateCommand(
                "CREATE TABLE IF NOT EXISTS WebsiteItem (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100) UNIQUE, Url VARCHAR(255) UNIQUE);"
                ).ExecuteNonQuery();
            SqLiteHelper.CreateCommand(
                "CREATE TABLE IF NOT EXISTS WebRuleItem (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100) UNIQUE, Url VARCHAR(255) UNIQUE, CatalogBegin VARCHAR(100), CatalogEnd VARCHAR(100), ChapterBegin VARCHAR(100), ChapterEnd VARCHAR(100), Replace TEXT NULL, AuthorBegin VARCHAR(100), AuthorEnd VARCHAR(100), DescriptionBegin VARCHAR(100), DescriptionEnd VARCHAR(100), CoverBegin VARCHAR(100), CoverEnd VARCHAR(100));"
                ).ExecuteNonQuery();
            SqLiteHelper.CreateCommand(
                "CREATE TABLE IF NOT EXISTS OptionItem (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(100) UNIQUE, Value Text);"
                ).ExecuteNonQuery();
        }

        public static SQLiteConnection Open()
        {
            SqLiteHelper.Conn.Open();
            return SqLiteHelper.Conn;
        }

        public static void Close()
        {
            SqLiteHelper.Conn.Close();
        }

        /// <summary>
        /// 插入语句
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="columns">插入的列</param>
        /// <param name="tags">标签或值</param>
        /// <param name="parameters">标签对应的值</param>
        /// <returns></returns>
        public static int Insert<T>(string columns, string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"INSERT INTO {table} ({columns}) VALUES ({tags});", parameters).ExecuteNonQuery();
        }

        /// <summary>
        /// 返回插入的自增id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="tags"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int InsertId<T>(string columns, string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return Convert.ToInt32(SqLiteHelper.CreateCommand($"INSERT INTO {table} ({columns}) VALUES ({tags});select last_insert_rowid();", parameters).ExecuteScalar());
        }

        public static int Insert<T>(string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"INSERT INTO {table} VALUES ({tags});", parameters).ExecuteNonQuery();
        }

        /// <summary>
        /// 插入时，某条记录不存在则插入，存在则更新 id 会改变
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="tags"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int Replace<T>(string columns, string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"REPLACE INTO {table} ({columns}) VALUES ({tags});", parameters).ExecuteNonQuery();
        }
        /// <summary>
        /// 插入 如果存在忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="tags"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int InsertOrIgnore<T>(string columns, string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"INSERT OR IGNORE INTO {table} ({columns}) VALUES ({tags});", parameters).ExecuteNonQuery();
        }
        /// <summary>
        /// 返回自增id 0 表示失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="tags"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int InsertOrIgnoreId<T>(string columns, string tags, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return Convert.ToInt32(SqLiteHelper.CreateCommand($"INSERT OR IGNORE INTO {table} ({columns}) VALUES ({tags});select last_insert_rowid();", parameters).ExecuteScalar());
        }

        public static int Update<T>(string sql, string where, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"UPDATE {table} SET {sql} WHERE {where};", parameters).ExecuteNonQuery();
        }

        public static int Delete<T>(string where)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"DELETE FROM {table} WHERE {where};").ExecuteNonQuery();
        }

        public static SQLiteDataReader Select<T>(string feild = "*", string sql = "", params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"SELECT {feild} FROM {table} {sql};", parameters).ExecuteReader();
        }

        /// <summary>
        /// 获取第一行第一列的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="feild"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object Find<T>(string feild, string where, params SQLiteParameter[] parameters)
        {
            var table = typeof(T).Name;
            return SqLiteHelper.CreateCommand($"SELECT {feild} FROM {table} WHERE {where};", parameters).ExecuteScalar();
        }

        public static WebRuleItem GetRule(string url)
        {
            url = UrlHelper.GetWeb(url);
            var reader = Select<WebRuleItem>("*", "WHERE Url = @url LIMIT 1", new SQLiteParameter("@url", url));
            reader.Read();
            if (reader.HasRows)
            {
                var item = new WebRuleItem(reader);
                reader.Close();
                return item;
            }
            reader.Close();
            return null;
        }
    }
}
