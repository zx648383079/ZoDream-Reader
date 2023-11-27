using Newtonsoft.Json;

namespace ZoDream.Shared.Plugins.Importers
{
    public class TxtTocRule
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rule")]
        public string Rule { get; set; }

        [JsonProperty("example")]
        public string Example { get; set; }

        [JsonProperty("serialNumber")]
        public long SerialNumber { get; set; }
    }

    public class ThemeConfig
    {
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }

        [JsonProperty("isNightTheme")]
        public bool IsNightTheme { get; set; }

        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonProperty("accentColor")]
        public string AccentColor { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("bottomBackground")]
        public string BottomBackground { get; set; }
    }

    public class RssSource
    {
        [JsonProperty("customOrder")]
        public long CustomOrder { get; set; }

        [JsonProperty("enableJs")]
        public bool EnableJs { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("singleUrl")]
        public bool SingleUrl { get; set; }

        [JsonProperty("sourceGroup")]
        public string SourceGroup { get; set; }

        [JsonProperty("sourceIcon")]
        public Uri SourceIcon { get; set; }

        [JsonProperty("sourceName")]
        public string SourceName { get; set; }

        [JsonProperty("sourceUrl")]
        public Uri SourceUrl { get; set; }
    }
    public class ReadConfig
    {
        [JsonProperty("bgStr")]
        public string BgStr { get; set; }

        [JsonProperty("bgStrEInk", NullValueHandling = NullValueHandling.Ignore)]
        public string BgStrEInk { get; set; }

        [JsonProperty("bgStrNight")]
        public string BgStrNight { get; set; }

        [JsonProperty("bgType")]
        public long BgType { get; set; }

        [JsonProperty("bgTypeEInk", NullValueHandling = NullValueHandling.Ignore)]
        public long? BgTypeEInk { get; set; }

        [JsonProperty("bgTypeNight")]
        public long BgTypeNight { get; set; }

        [JsonProperty("darkStatusIcon")]
        public bool DarkStatusIcon { get; set; }

        [JsonProperty("darkStatusIconEInk", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DarkStatusIconEInk { get; set; }

        [JsonProperty("darkStatusIconNight")]
        public bool DarkStatusIconNight { get; set; }

        [JsonProperty("footerMode", NullValueHandling = NullValueHandling.Ignore)]
        public long? FooterMode { get; set; }

        [JsonProperty("footerPaddingBottom", NullValueHandling = NullValueHandling.Ignore)]
        public long? FooterPaddingBottom { get; set; }

        [JsonProperty("footerPaddingLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long? FooterPaddingLeft { get; set; }

        [JsonProperty("footerPaddingRight", NullValueHandling = NullValueHandling.Ignore)]
        public long? FooterPaddingRight { get; set; }

        [JsonProperty("footerPaddingTop", NullValueHandling = NullValueHandling.Ignore)]
        public long? FooterPaddingTop { get; set; }

        [JsonProperty("headerMode", NullValueHandling = NullValueHandling.Ignore)]
        public long? HeaderMode { get; set; }

        [JsonProperty("headerPaddingBottom", NullValueHandling = NullValueHandling.Ignore)]
        public long? HeaderPaddingBottom { get; set; }

        [JsonProperty("headerPaddingLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long? HeaderPaddingLeft { get; set; }

        [JsonProperty("headerPaddingRight", NullValueHandling = NullValueHandling.Ignore)]
        public long? HeaderPaddingRight { get; set; }

        [JsonProperty("headerPaddingTop", NullValueHandling = NullValueHandling.Ignore)]
        public long? HeaderPaddingTop { get; set; }

        [JsonProperty("letterSpacing", NullValueHandling = NullValueHandling.Ignore)]
        public long? LetterSpacing { get; set; }

        [JsonProperty("lineSpacingExtra", NullValueHandling = NullValueHandling.Ignore)]
        public long? LineSpacingExtra { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("paddingBottom", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaddingBottom { get; set; }

        [JsonProperty("paddingLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaddingLeft { get; set; }

        [JsonProperty("paddingRight", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaddingRight { get; set; }

        [JsonProperty("paddingTop", NullValueHandling = NullValueHandling.Ignore)]
        public long? PaddingTop { get; set; }

        [JsonProperty("paragraphIndent", NullValueHandling = NullValueHandling.Ignore)]
        public string ParagraphIndent { get; set; }

        [JsonProperty("paragraphSpacing", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParagraphSpacing { get; set; }

        [JsonProperty("showFooterLine", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowFooterLine { get; set; }

        [JsonProperty("showHeaderLine", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowHeaderLine { get; set; }

        [JsonProperty("textBold", NullValueHandling = NullValueHandling.Ignore)]
        public long? TextBold { get; set; }

        [JsonProperty("textColor")]
        public string TextColor { get; set; }

        [JsonProperty("textColorEInk", NullValueHandling = NullValueHandling.Ignore)]
        public string TextColorEInk { get; set; }

        [JsonProperty("textColorNight")]
        public string TextColorNight { get; set; }

        [JsonProperty("textSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? TextSize { get; set; }

        [JsonProperty("tipColor", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipColor { get; set; }

        [JsonProperty("tipFooterLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipFooterLeft { get; set; }

        [JsonProperty("tipFooterMiddle", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipFooterMiddle { get; set; }

        [JsonProperty("tipFooterRight", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipFooterRight { get; set; }

        [JsonProperty("tipHeaderLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipHeaderLeft { get; set; }

        [JsonProperty("tipHeaderMiddle", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipHeaderMiddle { get; set; }

        [JsonProperty("tipHeaderRight", NullValueHandling = NullValueHandling.Ignore)]
        public long? TipHeaderRight { get; set; }

        [JsonProperty("titleBottomSpacing", NullValueHandling = NullValueHandling.Ignore)]
        public long? TitleBottomSpacing { get; set; }

        [JsonProperty("titleMode", NullValueHandling = NullValueHandling.Ignore)]
        public long? TitleMode { get; set; }

        [JsonProperty("titleSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? TitleSize { get; set; }

        [JsonProperty("titleTopSpacing", NullValueHandling = NullValueHandling.Ignore)]
        public long? TitleTopSpacing { get; set; }
    }

    public class TtsSource
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("loginUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginUrl { get; set; }

        [JsonProperty("loginUi", NullValueHandling = NullValueHandling.Ignore)]
        public Input[] LoginUi { get; set; }

        [JsonProperty("loginCheckJs", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginCheckJs { get; set; }
    }

    public class Input
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class DictRule
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("urlRule")]
        public string UrlRule { get; set; }

        [JsonProperty("showRule")]
        public string ShowRule { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("sortNumber")]
        public long SortNumber { get; set; }
    }

    public class BookSource
    {
        [JsonProperty("bookSourceComment")]
        public string BookSourceComment { get; set; }

        [JsonProperty("bookSourceGroup")]
        public string BookSourceGroup { get; set; }

        [JsonProperty("bookSourceName")]
        public string BookSourceName { get; set; }

        [JsonProperty("bookSourceType")]
        public long BookSourceType { get; set; }

        [JsonProperty("bookSourceUrl")]
        public Uri BookSourceUrl { get; set; }

        [JsonProperty("customOrder")]
        public long CustomOrder { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("enabledExplore")]
        public bool EnabledExplore { get; set; }

        [JsonProperty("exploreUrl")]
        public string ExploreUrl { get; set; }

        [JsonProperty("searchUrl")]
        public string SearchUrl { get; set; }

        [JsonProperty("lastUpdateTime")]
        public long LastUpdateTime { get; set; }

        [JsonProperty("loginCheckJs")]
        public string LoginCheckJs { get; set; }

        [JsonProperty("loginUi")]
        public string LoginUi { get; set; }

        [JsonProperty("loginUrl")]
        public string LoginUrl { get; set; }

        [JsonProperty("respondTime")]
        public long RespondTime { get; set; }

        [JsonProperty("ruleBookInfo")]
        public RuleBookInfo RuleBookInfo { get; set; }

        [JsonProperty("ruleContent")]
        public RuleContent RuleContent { get; set; }

        [JsonProperty("ruleExplore")]
        public Rule RuleExplore { get; set; }

        [JsonProperty("ruleSearch")]
        public Rule RuleSearch { get; set; }

        [JsonProperty("ruleToc")]
        public RuleToc RuleToc { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }
    }

    public class RuleBookInfo
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

    public class RuleContent
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("payAction")]
        public string PayAction { get; set; }
    }

    public class Rule
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("bookList")]
        public string BookList { get; set; }

        [JsonProperty("bookUrl")]
        public string BookUrl { get; set; }

        [JsonProperty("coverUrl")]
        public string CoverUrl { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("lastChapter")]
        public string LastChapter { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class RuleToc
    {
        [JsonProperty("chapterList")]
        public string ChapterList { get; set; }

        [JsonProperty("chapterName")]
        public string ChapterName { get; set; }

        [JsonProperty("chapterUrl")]
        public string ChapterUrl { get; set; }
    }

}
