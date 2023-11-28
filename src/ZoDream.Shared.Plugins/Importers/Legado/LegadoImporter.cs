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
        public async Task<List<IAppTheme>> LoadAppThemeAsync(string fileName)
        {
            var items = await ReadAsync<List<ThemeConfig>>(fileName);
            var res = new List<IAppTheme>();
            if (items is null)
            {
                return res;
            }
            foreach (var item in items)
            {
                res.Add(new AppThemeEntity()
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
        public async Task<List<IReadTheme>> LoadReadThemeAsync(string fileName)
        {
            var items = await ReadAsync<List<ReadConfig>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<IReadTheme>();
            foreach (var item in items)
            {
                res.Add(new ReadThemeEntity()
                {
                    Name = item.Name,
                    Background = item.BgType == 0 ? item.BgStr : string.Empty,
                    BackgroundImage = item.BgType != 0 ? item.BgStr : string.Empty,
                    DarkBackground = item.BgTypeNight == 0 ? item.BgStrNight : string.Empty,
                    DarkBackgroundImage = item.BgTypeNight != 0 ? item.BgStrNight : string.Empty,
                    DarForeground = item.TextColorNight,
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

        public async Task<List<ISubscribeSource>> LoadRssAsync(string fileName)
        {
            var items = await ReadAsync<List<RssSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<ISubscribeSource>();
            foreach (var item in items)
            {
                res.Add(new SubscribeSourceEntity()
                {
                    Name = item.SourceName,
                });
            }
            return res;
        }

        public async Task<List<ISourceRule>> LoadSourceAsync(string fileName)
        {
            var items = await ReadAsync<List<BookSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<ISourceRule>();
            foreach (var item in items)
            {
                res.Add(new SourceRuleEntity()
                {
                    Name = item.BookSourceName,
                    IsEnabled = item.Enabled
                });
            }
            return res;
        }
        public async Task<List<ITextToSpeech>> LoadTTSAsync(string fileName)
        {
            var items = await ReadAsync<List<TtsSource>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<ITextToSpeech>();
            foreach (var item in items)
            {
                res.Add(new TextToSpeechEntity()
                {
                    Name = item.Name,
                    Url = item.Url,
                });
            }
            return res;
        }

        public async Task<List<IDictionaryRule>> LoadDictionaryRuleAsync(string fileName)
        {
            var items = await ReadAsync<List<DictRule>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<IDictionaryRule>();
            foreach (var item in items)
            {
                res.Add(new DictionaryRuleEntity()
                {
                    Name = item.Name,
                    UrlRule = item.UrlRule,
                    ShowRule = item.ShowRule,
                    IsEnabled = item.Enabled,
                });
            }
            return res;
        }

        public async Task<List<IChapterRule>> LoadChapterRuleAsync(string fileName)
        {
            var items = await ReadAsync<List<TxtTocRule>>(fileName);
            if (items is null)
            {
                return [];
            }
            var res = new List<IChapterRule>();
            foreach ( var item in items)
            {
                res.Add(new ChapterRuleEntity()
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
