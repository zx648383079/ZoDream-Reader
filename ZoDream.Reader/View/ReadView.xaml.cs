using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace ZoDream.Reader.View
{
    /// <summary>
    /// Description for ReadView.
    /// </summary>
    public partial class ReadView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ReadView class.
        /// </summary>
        public ReadView()
        {
            InitializeComponent();
            Messenger.Default.Send(new NotificationMessageAction(null, () =>
            {
                ReadViewer.FirstPage();
            }), "readViewer");
        }
    }
}