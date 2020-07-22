using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZoDream.Helper;
using ZoDream.Reader.Helper.Database;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.Helper
{
    public class SystemHelper
    {
        public static Dictionary<string, string> Options = new Dictionary<string, string>();

        public static void Open()
        {
            Options.Clear();
            var reader = DatabaseHelper.Select<OptionItem>("Name, Value");
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    Options.Add(reader.GetString(0), reader[1].ToString());
                }
            }
            reader.Close();
        }

        public static bool Save()
        {
            DbTransaction trans = SqLiteHelper.Conn.BeginTransaction();
            try
            {
                foreach (var option in Options)
                {
                    DatabaseHelper.Replace<OptionItem>("Name, Value", "@name, @value",
                        new SQLiteParameter("@name", option.Key), new SQLiteParameter("@value", option.Value));
                }
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback();
                return false;
            }
        }

        public static void Set(string key, object value)
        {
            if (Options.ContainsKey(key))
            {
                Options[key] = value.ToString();
            }
            else
            {
                Options.Add(key, value.ToString());
            }
        }

        public static string Get(string key, string defaul = "")
        {
            return Options.ContainsKey(key) ? Options[key] : defaul;
        }

        public static int GetInt(string key)
        {
            return Options.ContainsKey(key) ? Convert.ToInt32(Options[key]) : 0;
        }

        public static Color GetColor(string key, string defaul = "")
        {
            return StringHelper.ToColor(Get(key, defaul));
        }

        public static FontFamily GetFontFamily(string key)
        {
            return new FontFamily(Get(key));
        }

        public static ImageBrush GetImageBrush(string key)
        {
            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Get(key)))
            };
        }

        public static SolidColorBrush GetSolidColorBrush(string key)
        {
            return new SolidColorBrush(GetColor(key));
        }

        public static Brush GetBrush(string key, string defaul = "")
        {
            var value = Get(key, defaul);
            if (string.IsNullOrEmpty(value) || value[0] == '#' || value.IndexOf(',') > 0)
            {
                return new SolidColorBrush(StringHelper.ToColor(value));
            }
            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(value))
            };
        }

        public static FontWeight GetFontWeight(string key)
        {
            return FontHelper.GetFontWeight(GetInt(key));
        }
    }
}
