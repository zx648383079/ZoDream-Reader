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
    public class AddRuleViewModel : ViewModelBase
    {
        private NotificationMessageAction<WebRuleItem> _addItem;
        /// <summary>
        /// Initializes a new instance of the AddRuleViewModel class.
        /// </summary>
        public AddRuleViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<WebRuleItem>>(this, "rule", m =>
            {
                _addItem = m;
                if (m.Sender != null)
                {
                    
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

        /// <summary>
        /// The <see cref="CatalogBegin" /> property's name.
        /// </summary>
        public const string CatalogBeginPropertyName = "CatalogBegin";

        private string _catalogBegin = string.Empty;

        /// <summary>
        /// Sets and gets the CatalogBegin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CatalogBegin
        {
            get
            {
                return _catalogBegin;
            }
            set
            {
                Set(CatalogBeginPropertyName, ref _catalogBegin, value);
            }
        }

        /// <summary>
        /// The <see cref="CatalogEnd" /> property's name.
        /// </summary>
        public const string CatalogEndPropertyName = "CatalogEnd";

        private string _catalogEnd = string.Empty;

        /// <summary>
        /// Sets and gets the CatalogEnd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CatalogEnd
        {
            get
            {
                return _catalogEnd;
            }
            set
            {
                Set(CatalogEndPropertyName, ref _catalogEnd, value);
            }
        }

        /// <summary>
        /// The <see cref="ChapterBegin" /> property's name.
        /// </summary>
        public const string ChapterBeginPropertyName = "ChapterBegin";

        private string _chapterBegin = string.Empty;

        /// <summary>
        /// Sets and gets the ChapterBegin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ChapterBegin
        {
            get
            {
                return _chapterBegin;
            }
            set
            {
                Set(ChapterBeginPropertyName, ref _chapterBegin, value);
            }
        }

        /// <summary>
        /// The <see cref="ChapterEnd" /> property's name.
        /// </summary>
        public const string ChapterEndPropertyName = "ChapterEnd";

        private string _chapterEnd = string.Empty;

        /// <summary>
        /// Sets and gets the ChapterEnd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ChapterEnd
        {
            get
            {
                return _chapterEnd;
            }
            set
            {
                Set(ChapterEndPropertyName, ref _chapterEnd, value);
            }
        }

        /// <summary>
        /// The <see cref="Replace" /> property's name.
        /// </summary>
        public const string ReplacePropertyName = "Replace";

        private string _replace = string.Empty;

        /// <summary>
        /// Sets and gets the Replace property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Replace
        {
            get
            {
                return _replace;
            }
            set
            {
                Set(ReplacePropertyName, ref _replace, value);
            }
        }

        /// <summary>
        /// The <see cref="AuthorBegin" /> property's name.
        /// </summary>
        public const string AuthorBeginPropertyName = "AuthorBegin";

        private string _authorBegin = string.Empty;

        /// <summary>
        /// Sets and gets the AuthorBegin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AuthorBegin
        {
            get
            {
                return _authorBegin;
            }
            set
            {
                Set(AuthorBeginPropertyName, ref _authorBegin, value);
            }
        }

        /// <summary>
        /// The <see cref="AuthorEnd" /> property's name.
        /// </summary>
        public const string AuthorEndPropertyName = "AuthorEnd";

        private string _authorEnd = string.Empty;

        /// <summary>
        /// Sets and gets the AuthorEnd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AuthorEnd
        {
            get
            {
                return _authorEnd;
            }
            set
            {
                Set(AuthorEndPropertyName, ref _authorEnd, value);
            }
        }

        /// <summary>
        /// The <see cref="DescriptionBegin" /> property's name.
        /// </summary>
        public const string DescriptionBeginPropertyName = "DescriptionBegin";

        private string _descriptionBegin = string.Empty;

        /// <summary>
        /// Sets and gets the DescriptionBegin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DescriptionBegin
        {
            get
            {
                return _descriptionBegin;
            }
            set
            {
                Set(DescriptionBeginPropertyName, ref _descriptionBegin, value);
            }
        }

        /// <summary>
        /// The <see cref="DescriptionEnd" /> property's name.
        /// </summary>
        public const string DescriptionEndPropertyName = "DescriptionEnd";

        private string _descriptionEnd = string.Empty;

        /// <summary>
        /// Sets and gets the DescriptionEnd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DescriptionEnd
        {
            get
            {
                return _descriptionEnd;
            }
            set
            {
                Set(DescriptionEndPropertyName, ref _descriptionEnd, value);
            }
        }

        /// <summary>
        /// The <see cref="CoverBegin" /> property's name.
        /// </summary>
        public const string CoverBeginPropertyName = "CoverBegin";

        private string _coverBegin = string.Empty;

        /// <summary>
        /// Sets and gets the CoverBegin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CoverBegin
        {
            get
            {
                return _coverBegin;
            }
            set
            {
                Set(CoverBeginPropertyName, ref _coverBegin, value);
            }
        }

        /// <summary>
        /// The <see cref="CoverEnd" /> property's name.
        /// </summary>
        public const string CoverEndPropertyName = "CoverEnd";

        private string _coverEnd = string.Empty;

        /// <summary>
        /// Sets and gets the CoverEnd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CoverEnd
        {
            get
            {
                return _coverEnd;
            }
            set
            {
                Set(CoverEndPropertyName, ref _coverEnd, value);
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
            if (string.IsNullOrWhiteSpace(Url)) return;
            var item = new WebRuleItem();
            item.Name = Name;
            item.Url = UrlHelper.GetWeb(Url);
            item.CatalogBegin = CatalogBegin;
            item.CatalogEnd = CatalogEnd;
            item.ChapterBegin = ChapterBegin;
            item.ChapterEnd = ChapterEnd;
            item.Replace = Replace;
            item.AuthorBegin = AuthorBegin;
            item.AuthorEnd = AuthorEnd;
            item.DescriptionBegin = DescriptionBegin;
            item.DescriptionEnd = DescriptionEnd;
            item.CoverBegin = CoverBegin;
            item.CoverEnd = CoverEnd;
            _addItem.Execute(item);
        }
    }
}