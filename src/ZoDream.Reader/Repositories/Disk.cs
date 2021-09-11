using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZoDream.Reader.Repositories
{
    public class Disk
    {
        public string BaseFolder { get; private set; }

        public string TxtFolder { get; private set; }

        public string FontFolder { get; private set; }
        public string TxtFileName(string fileId)
        {
            return Path.Join(TxtFolder, fileId);
        }

        public string FontFileName(string fileId)
        {
            return Path.Join(FontFolder, fileId);
        }

        public void GetFonts()
        {
            var data = Fonts.SystemFontFamilies;
        }
        public FontFamily? FontFamily(string file)
        {
            var pfc = new System.Drawing.Text.PrivateFontCollection();
            pfc.AddFontFile(file);
            var item = pfc.Families.FirstOrDefault();
            if (item == null)
            {
                return null;
            }
            return new FontFamily(new Uri(file), item.Name);
        }

        public bool AddTxt(string file, out string fileId, out string Name)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                fileId = file;
                Name = info.Name.Replace(info.Extension, "");
                return false;
            }
            Name = info.Name.Replace(info.Extension, "");
            if (info.DirectoryName != null && info.DirectoryName.StartsWith(TxtFolder))
            {
                fileId = info.Name;
                return true;
            }
            fileId = info.Name;
            var fileInfo = info.CopyTo(TxtFileName(info.Name));
            return fileInfo.Exists;
        }

        public bool AddFont(string file, out string fileId, out string Name)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                fileId = file;
                Name = info.Name.Replace(info.Extension, "");
                return false;
            }
            Name = info.Name.Replace(info.Extension, "");
            if (info.DirectoryName != null && info.DirectoryName.StartsWith(FontFolder))
            {
                fileId = info.Name;
                return true;
            }
            fileId = info.Name;
            var fileInfo = info.CopyTo(FontFileName(info.Name));
            return fileInfo.Exists;
        }
        public Disk(string? folder = null)
        {
            BaseFolder = string.IsNullOrWhiteSpace(folder) ? AppDomain.CurrentDomain.BaseDirectory : folder;
            TxtFolder = Path.Join(BaseFolder, "txt");
            FontFolder = Path.Join(BaseFolder, "font");
            Directory.CreateDirectory(TxtFolder);
            Directory.CreateDirectory(FontFolder);
        }
    }
}
