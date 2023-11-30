using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Plugins.Importers
{
    public class LeGaDoImporter: INovelImporter
    {
        public async Task<List<T>> LoadAppThemeAsync<T>(string fileName) 
            where T : IAppTheme, new()
        {
            var items = await ReadAsync<List<ThemeConfig>>(fileName);
            var res = new List<T>();
            if (items is null)
            {
                return res;
            }
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.ThemeName,
                    IsDarkTheme = item.IsNightTheme,
                    PrimaryColor = item.PrimaryColor,
                    AccentTextColor = item.AccentColor,
                    BodyColor = item.BackgroundColor
                });
            }
            return res;
        }
        public async Task<List<T>> LoadReadThemeAsync<T>(string fileName) 
            where T : IReadTheme, new()
        {
            var items = await ReadAsync<List<ReadConfig>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.Name,
                    Background = item.BgType == 0 ? item.BgStr : string.Empty,
                    BackgroundImage = item.BgType != 0 ? item.BgStr : string.Empty,
                    DarkBackground = item.BgTypeNight == 0 ? item.BgStrNight : string.Empty,
                    DarkBackgroundImage = item.BgTypeNight != 0 ? item.BgStrNight : string.Empty,
                    DarkForeground = item.TextColorNight,
                    Foreground = item.TextColor,
                    FontSize = item.TextSize ?? 16,
                    PaddingTop = item.PaddingTop ?? 0,
                    PaddingLeft = item.PaddingLeft ?? 0,
                    PaddingBottom = item.PaddingBottom ?? 0,
                    PaddingRight = item.PaddingRight ?? 0,
                    ParagraphSpacing = item.ParagraphSpacing?? 0,
                    ParagraphIndent = string.IsNullOrEmpty(item.ParagraphIndent) ? 4 : item.ParagraphIndent.Length,
                    TitleFontSize = item.TitleSize ?? 16,
                    TitleSpacing = item.TitleBottomSpacing ?? 0,
                    LetterSpacing = item.LetterSpacing,
                    LineSpacing = item.LineSpacingExtra,
                });
            }
            return res;
        }

        public async Task<List<T>> LoadRssAsync<T>(string fileName) 
            where T : ISubscribeSource, new()
        {
            var items = await ReadAsync<List<RssSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.SourceName,
                });
            }
            return res;
        }

        public async Task<List<T>> LoadSourceAsync<T>(string fileName) 
            where T : ISourceRule, new()
        {
            var items = await ReadAsync<List<BookSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.BookSourceName,
                    IsEnabled = item.Enabled
                });
            }
            return res;
        }
        public async Task<List<T>> LoadTTSAsync<T>(string fileName) 
            where T : ITextToSpeech, new()
        {
            var items = await ReadAsync<List<TtsSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.Name,
                    Url = item.Url,
                });
            }
            return res;
        }

        public async Task<List<T>> LoadDictionaryRuleAsync<T>(string fileName) 
            where T : IDictionaryRule, new()
        {
            var items = await ReadAsync<List<DictRule>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.Name,
                    UrlRule = item.UrlRule,
                    ShowRule = item.ShowRule,
                    IsEnabled = item.Enabled,
                });
            }
            return res;
        }

        public async Task<List<T>> LoadReplaceRuleAsync<T>(string fileName) 
            where T : IReplaceRule, new()
        {
            var items = await ReadAsync<List<DictRule>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach (var item in items)
            {
                res.Add(new T()
                {
                    Name = item.Name,
                    IsEnabled = item.Enabled,
                });
            }
            return res;
        }

        public async Task<List<T>> LoadChapterRuleAsync<T>(string fileName) 
            where T : IChapterRule, new()
        {
            var items = await ReadAsync<List<TxtTocRule>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<T>();
            foreach ( var item in items)
            {
                res.Add(new T()
                {
                    Name = item.Name,
                    Example = item.Example,
                    MatchRule = item.Rule,
                    IsEnabled = item.Enable
                });
            }
            return res;
        }

        public async Task<T?> ReadAsync<T>(string fileName)
        {
            var content = await LocationStorage.ReadAsync(fileName);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
