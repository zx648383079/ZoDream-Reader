using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace ZoDream.Reader.View
{
    /// <summary>
    /// Description for SystemView.
    /// </summary>
    public partial class SystemView : Window
    {
        /// <summary>
        /// Initializes a new instance of the SystemView class.
        /// </summary>
        public SystemView()
        {
            InitializeComponent();
            Messenger.Default.Send(new NotificationMessageAction(null, () =>
            {
                DialogResult = true;
                Close();
            }), "system");
        }
    }
}