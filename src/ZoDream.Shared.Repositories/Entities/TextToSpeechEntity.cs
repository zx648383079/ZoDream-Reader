using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("tts_sources")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class TextToSpeechEntity: ITextToSpeech
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string DownloadRule { get; set; } = string.Empty;

        public string LoginUrl { get; set; } = string.Empty;
        public IFormInput[] LoginForm { get; set; } = [];

        public bool IsEnabled { get; set; } = true;
        public int SortOrder { get; set; } = 99;
    }
}
