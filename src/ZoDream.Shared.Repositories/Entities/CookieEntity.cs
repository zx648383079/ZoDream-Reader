using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("site_cookies")]
    [PrimaryKey("Url", AutoIncrement = false)]
    public class CookieEntity: ISiteCookie
    {
        public string Url { get; set; } = string.Empty;

        public string Cookie { get; set; } = string.Empty;
    }
}
