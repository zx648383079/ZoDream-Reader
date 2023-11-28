using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IAppTheme
    {

        public string Name { get; set; }

        public bool IsDarkTheme { get; set; }

        public string PrimaryColor { get; set; }

        public string PrimaryTextColor { get; set; }

        public string AccentTextColor { get; set; }

        public string BodyColor { get; set; }

        public string BodyTextColor { get; set; }

    }
}
