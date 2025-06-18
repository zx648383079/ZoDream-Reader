using System.Text.Json.Serialization;

namespace ZoDream.Shared.Plugins.Importers
{
    internal class TxtTocRule
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("enable")]
        public bool Enable { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rule")]
        public string Rule { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }

        [JsonPropertyName("serialNumber")]
        public long SerialNumber { get; set; }
    }

    internal class ThemeConfig
    {
        [JsonPropertyName("themeName")]
        public string ThemeName { get; set; }

        [JsonPropertyName("isNightTheme")]
        public bool IsNightTheme { get; set; }

        [JsonPropertyName("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonPropertyName("accentColor")]
        public string AccentColor { get; set; }

        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonPropertyName("bottomBackground")]
        public string BottomBackground { get; set; }
    }

    internal class RssSource
    {
        [JsonPropertyName("customOrder")]
        public long CustomOrder { get; set; }

        [JsonPropertyName("enableJs")]
        public bool EnableJs { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("singleUrl")]
        public bool SingleUrl { get; set; }

        [JsonPropertyName("sourceGroup")]
        public string SourceGroup { get; set; }

        [JsonPropertyName("sourceIcon")]
        public string SourceIcon { get; set; }

        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; }

        [JsonPropertyName("sourceUrl")]
        public string SourceUrl { get; set; }
    }
    internal class ReadConfig
    {
        [JsonPropertyName("bgStr")]
        public string BgStr { get; set; }

        [JsonPropertyName("bgStrEInk")]
        public string BgStrEInk { get; set; }

        [JsonPropertyName("bgStrNight")]
        public string BgStrNight { get; set; }

        [JsonPropertyName("bgType")]
        public long BgType { get; set; }

        [JsonPropertyName("bgTypeEInk")]
        public long? BgTypeEInk { get; set; }

        [JsonPropertyName("bgTypeNight")]
        public long BgTypeNight { get; set; }

        [JsonPropertyName("darkStatusIcon")]
        public bool DarkStatusIcon { get; set; }

        [JsonPropertyName("darkStatusIconEInk")]
        public bool? DarkStatusIconEInk { get; set; }

        [JsonPropertyName("darkStatusIconNight")]
        public bool DarkStatusIconNight { get; set; }

        [JsonPropertyName("footerMode")]
        public long? FooterMode { get; set; }

        [JsonPropertyName("footerPaddingBottom")]
        public long? FooterPaddingBottom { get; set; }

        [JsonPropertyName("footerPaddingLeft")]
        public long? FooterPaddingLeft { get; set; }

        [JsonPropertyName("footerPaddingRight")]
        public long? FooterPaddingRight { get; set; }

        [JsonPropertyName("footerPaddingTop")]
        public long? FooterPaddingTop { get; set; }

        [JsonPropertyName("headerMode")]
        public long? HeaderMode { get; set; }

        [JsonPropertyName("headerPaddingBottom")]
        public long? HeaderPaddingBottom { get; set; }

        [JsonPropertyName("headerPaddingLeft")]
        public long? HeaderPaddingLeft { get; set; }

        [JsonPropertyName("headerPaddingRight")]
        public int HeaderPaddingRight { get; set; }

        [JsonPropertyName("headerPaddingTop")]
        public int HeaderPaddingTop { get; set; }

        [JsonPropertyName("letterSpacing")]
        public int LetterSpacing { get; set; }

        [JsonPropertyName("lineSpacingExtra")]
        public int LineSpacingExtra { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("paddingBottom")]
        public int? PaddingBottom { get; set; }

        [JsonPropertyName("paddingLeft")]
        public int? PaddingLeft { get; set; }

        [JsonPropertyName("paddingRight")]
        public int? PaddingRight { get; set; }

        [JsonPropertyName("paddingTop")]
        public int? PaddingTop { get; set; }

        [JsonPropertyName("paragraphIndent")]
        public string ParagraphIndent { get; set; }

        [JsonPropertyName("paragraphSpacing")]
        public int? ParagraphSpacing { get; set; }

        [JsonPropertyName("showFooterLine")]
        public bool? ShowFooterLine { get; set; }

        [JsonPropertyName("showHeaderLine")]
        public bool? ShowHeaderLine { get; set; }

        [JsonPropertyName("textBold")]
        public long? TextBold { get; set; }

        [JsonPropertyName("textColor")]
        public string TextColor { get; set; }

        [JsonPropertyName("textColorEInk")]
        public string TextColorEInk { get; set; }

        [JsonPropertyName("textColorNight")]
        public string TextColorNight { get; set; }

        [JsonPropertyName("textSize")]
        public int? TextSize { get; set; }

        [JsonPropertyName("tipColor")]
        public long? TipColor { get; set; }

        [JsonPropertyName("tipFooterLeft")]
        public long? TipFooterLeft { get; set; }

        [JsonPropertyName("tipFooterMiddle")]
        public long? TipFooterMiddle { get; set; }

        [JsonPropertyName("tipFooterRight")]
        public long? TipFooterRight { get; set; }

        [JsonPropertyName("tipHeaderLeft")]
        public long? TipHeaderLeft { get; set; }

        [JsonPropertyName("tipHeaderMiddle")]
        public long? TipHeaderMiddle { get; set; }

        [JsonPropertyName("tipHeaderRight")]
        public long? TipHeaderRight { get; set; }

        [JsonPropertyName("titleBottomSpacing")]
        public int? TitleBottomSpacing { get; set; }

        [JsonPropertyName("titleMode")]
        public long? TitleMode { get; set; }

        [JsonPropertyName("titleSize")]
        public int? TitleSize { get; set; }

        [JsonPropertyName("titleTopSpacing")]
        public long? TitleTopSpacing { get; set; }
    }

    internal class TtsSource
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("loginUrl")]
        public string LoginUrl { get; set; }

        [JsonPropertyName("loginUi")]
        public Input[] LoginUi { get; set; }

        [JsonPropertyName("loginCheckJs")]
        public string LoginCheckJs { get; set; }
    }

    internal class Input
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    internal class DictRule
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("urlRule")]
        public string UrlRule { get; set; }

        [JsonPropertyName("showRule")]
        public string ShowRule { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("sortNumber")]
        public long SortNumber { get; set; }
    }

    internal class BookSource
    {
        [JsonPropertyName("bookSourceComment")]
        public string BookSourceComment { get; set; }

        [JsonPropertyName("bookSourceGroup")]
        public string BookSourceGroup { get; set; }

        [JsonPropertyName("bookSourceName")]
        public string BookSourceName { get; set; }

        [JsonPropertyName("bookSourceType")]
        public long BookSourceType { get; set; }

        [JsonPropertyName("bookSourceUrl")]
        public string BookSourceUrl { get; set; }

        [JsonPropertyName("customOrder")]
        public long CustomOrder { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("enabledExplore")]
        public bool EnabledExplore { get; set; }

        [JsonPropertyName("exploreUrl")]
        public string ExploreUrl { get; set; }

        [JsonPropertyName("searchUrl")]
        public string SearchUrl { get; set; }

        [JsonPropertyName("lastUpdateTime")]
        public long LastUpdateTime { get; set; }

        [JsonPropertyName("loginCheckJs")]
        public string LoginCheckJs { get; set; }

        [JsonPropertyName("loginUi")]
        public string LoginUi { get; set; }

        [JsonPropertyName("loginUrl")]
        public string LoginUrl { get; set; }

        [JsonPropertyName("respondTime")]
        public long RespondTime { get; set; }

        [JsonPropertyName("ruleBookInfo")]
        public RuleBookInfo RuleBookInfo { get; set; }

        [JsonPropertyName("ruleContent")]
        public RuleContent RuleContent { get; set; }

        [JsonPropertyName("ruleExplore")]
        public Rule RuleExplore { get; set; }

        [JsonPropertyName("ruleSearch")]
        public Rule RuleSearch { get; set; }

        [JsonPropertyName("ruleToc")]
        public RuleToc RuleToc { get; set; }

        [JsonPropertyName("weight")]
        public long Weight { get; set; }
    }

    internal class RuleBookInfo
    {
        public string init { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string intro { get; set; }
        public string kind { get; set; }
        public string lastChapter { get; set; }
        public string updateTime { get; set; }
        public string coverUrl { get; set; }
        public string tocUrl { get; set; }
        public string wordCount { get; set; }
        public string canReName { get; set; }
        public string downloadUrls { get; set; }
    }

    internal class RuleContent
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("payAction")]
        public string PayAction { get; set; }
    }

    internal class Rule
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("bookList")]
        public string BookList { get; set; }

        [JsonPropertyName("bookUrl")]
        public string BookUrl { get; set; }

        [JsonPropertyName("coverUrl")]
        public string CoverUrl { get; set; }

        [JsonPropertyName("intro")]
        public string Intro { get; set; }

        [JsonPropertyName("lastChapter")]
        public string LastChapter { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    internal class RuleToc
    {
        [JsonPropertyName("chapterList")]
        public string ChapterList { get; set; }

        [JsonPropertyName("chapterName")]
        public string ChapterName { get; set; }

        [JsonPropertyName("chapterUrl")]
        public string ChapterUrl { get; set; }
    }

}
