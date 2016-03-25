using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddWebViewModel : ViewModelBase
    {
        private NotificationMessageAction<WebsiteItem> _addItem;
        /// <summary>
        /// Initializes a new instance of the AddWebViewModel class.
        /// </summary>
        public AddWebViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<WebsiteItem>>(this, "web", m =>
            {
                _addItem = m;
                if (m.Sender != null)
                {
                    WebsiteItem item = (WebsiteItem)m.Sender;
                    Name = item.Name;
                    Url = item.Url;
                }
            });
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(NamePropertyName, ref _name, value);
            }
        }

        /// <summary>
        /// The <see cref="Url" /> property's name.
        /// </summary>
        public const string UrlPropertyName = "Url";

        private string _url = string.Empty;

        /// <summary>
        /// Sets and gets the Url property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                Set(UrlPropertyName, ref _url, value);
            }
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Url)) return;
            _addItem.Execute(new WebsiteItem(Name, UrlHelper.GetWeb(Url)));
            Name = Url = string.Empty;
        }
    }
}