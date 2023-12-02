using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("app_themes")]
    [PrimaryKey("Id")]
    public class AppThemeEntity: IAppTheme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsDarkTheme { get; set; }

        public string PrimaryColor { get; set; } = string.Empty;

        public string PrimaryTextColor { get; set; } = string.Empty;

        public string AccentTextColor { get; set; } = string.Empty;

        public string BodyColor { get; set; } = string.Empty;

        public string BodyTextColor { get; set; } = string.Empty;
    }
}
