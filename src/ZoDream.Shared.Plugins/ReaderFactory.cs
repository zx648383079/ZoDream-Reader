using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Net;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins
{
    public static class ReaderFactory
    {


        public static INovelSource Convert(INovelSourceEntity entity)
        {
            return entity.Type switch
            {
                2 => new NetSource(entity),
                1 => new FileSource(entity),
                _ => new TxtSource(entity),
            };
        }

        public static INovelSerializer GetReader(INovelSourceEntity entity)
        {
            return entity.Type switch
            {
                2 => new NetReader(),
                1 => new EPubSerializer(),
                _ => new TxtSerializer(),
            };
        }

        public static IPageTokenizer GetTokenizer(ISectionSource document)
        {
            if (document is HtmlDocument)
            {
                return new HtmlTokenizer();
            }
            return new TextTokenizer();
        }
    }
}
