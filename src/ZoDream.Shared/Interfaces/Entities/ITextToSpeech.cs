using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ITextToSpeech : IRuleItem
    {
        public string Url { get; set; }

        public string DownloadRule { get; set; }

        public string LoginUrl { get; set; }
        public IFormInput[] LoginForm { get; set; }
    }
}
