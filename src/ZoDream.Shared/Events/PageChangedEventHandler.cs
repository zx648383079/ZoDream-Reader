using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Events
{
    public delegate void PageChangedEventHandler(object sender, int page, PositionItem pagePosition);
}
