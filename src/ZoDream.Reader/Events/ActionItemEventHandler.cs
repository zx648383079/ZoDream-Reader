using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Reader.Events
{

    public class ActionHanlderArgs(INovel novel, ActionEvent e)
    {
        public INovel Source { get; private set; } = novel;

        public ActionEvent Action { get; private set; } = e;
    }

    public delegate void ActionEventHandler(object sender, ActionEvent e);
    public enum ActionEvent
    {
        NONE,
        CLICK,
        EDIT,
        DELETE,
    }
}
