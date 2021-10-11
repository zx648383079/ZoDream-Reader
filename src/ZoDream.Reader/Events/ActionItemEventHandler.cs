using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Events
{
    public delegate void ActionItemEventHandler(object sender, BookItem item, ActionEvent e);

    public delegate void ActionEventHandler(object sender, ActionEvent e);

    public enum ActionEvent
    {
        NONE,
        CLICK,
        EDIT,
        DELETE,
    }
}
