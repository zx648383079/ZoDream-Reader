using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Font
{
    public class FontHelper
    {

        public static async Task<IList<FontItem>> GetFontFamilyAsync(IEnumerable<string> pathItems)
        {
            var items = new List<FontItem>();
            foreach (var item in pathItems)
            {
                var info = new FileInfo(item);
                if (!info.Exists)
                {
                    continue;
                }
                if ((info.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    items.AddRange(await GetFontFamilyAsync(item));
                    continue;
                }
                var ttfFonts = Directory.GetFiles(item, "*.ttf", SearchOption.AllDirectories);
                var otfFonts = Directory.GetFiles(item, "*.otf", SearchOption.AllDirectories);
                var fileItems = ttfFonts.Union(otfFonts);
                foreach (var file in fileItems)
                {
                    items.AddRange(await GetFontFamilyAsync(file));
                }
            }
            return items;
        }

        public static async Task<IList<FontItem>> GetFontFamilyAsync(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            return (await GetFontFamilyAsync(fs)).Select(i => new FontItem(i, fileName)).ToList();
        }

        public static async Task<List<string>> GetFontFamilyAsync(Stream fs)
        {
            using var reader = new FontReader(fs);
            return await GetFontFamilyAsync(reader);
        }

        public static async Task<List<string>> GetFontFamilyAsync(FontReader reader)
        {
            var sfntVersion = await reader.ReadUInt32BEAsync();
            FontCheck.ValidateSfntVersion(sfntVersion);
            var tableCount = await reader.ReadUInt16BEAsync();

            await reader.SkipAsync(6);
            var offsetItems = new List<uint>();
            for (var i = 0; i < tableCount; i++)
            {
                var tagByte = await reader.ReadBytesAsync(4);
                var checksum = await reader.ReadUInt32BEAsync();
                var offset = await reader.ReadUInt32BEAsync();
                var length = await reader.ReadUInt32BEAsync();
                var tag = FontCheck.ConvertToTagName(tagByte);
                if (tag != Tables.NAME)
                {
                    continue;
                }
                offsetItems.Add(offset);
            }
            var items = new List<string>();
            foreach (var item in offsetItems)
            {
                items.AddRange(await GetTableNameAsync(reader, item));
            }

            return items;
        }

        private static async Task<IList<string>> GetTableNameAsync(FontReader reader, uint offset)
        {
            await reader.SeekAsync(offset);
            await reader.SkipAsync(2); //version
            var nameRecordCount = await reader.ReadUInt16BEAsync();
            ushort storageOffset = await reader.ReadUInt16BEAsync();
            var items = new List<string>();
            for (int i = 0; i < nameRecordCount; i++)
            {
                var platformID = await reader.ReadUInt16BEAsync();
                var encodingID = await reader.ReadUInt16BEAsync();
                var languageID = await reader.ReadUInt16BEAsync();
                var nameID = await reader.ReadUInt16BEAsync();
                var length = await reader.ReadUInt16BEAsync();
                var stringOffset = (ushort)(await reader.ReadUInt16BEAsync() + storageOffset);
                var actualPosition = reader.BaseStream.Position;
                if (nameID != NameID.Family)
                {
                    continue;
                }
                var data = await ExtractStringFromNameRecordAsync(reader,
                    offset,
                    stringOffset,
                    length,
                    platformID,
                    encodingID);

                reader.BaseStream.Seek(actualPosition, SeekOrigin.Begin);
                items.Add(data);
            }
            return items;
        }

        private static async Task<string> ExtractStringFromNameRecordAsync(FontReader reader, 
            uint offset, ushort stringOffset, ushort length, ushort platformID, 
            ushort encodingID)
        {
            await reader.SeekAsync(offset + stringOffset);
            var data = await reader.ReadBytesAsync(length);
            if (platformID == PlatformID.Windows)
            {
                return Encoding.BigEndianUnicode.GetString(data);
            }
            if (platformID == PlatformID.Unicode)
            {
                return Encoding.BigEndianUnicode.GetString(data);
            }
            if (platformID == PlatformID.Macintosh)
            {
                return Encoding.ASCII.GetString(data);
            }
            return Encoding.UTF8.GetString(data);
        }


    }
}
