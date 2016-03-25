using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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
    public class ReadViewModel : ViewModelBase
    {
        private NotificationMessageAction<BookItem> _readItem;

        private BookItem _book;
        /// <summary>
        /// Initializes a new instance of the ReadViewModel class.
        /// </summary>
        public ReadViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<BookItem>>(this, "read", m =>
            {
                _readItem = m;
                _book = (BookItem) m.Sender;
                _loading();
            });
        }

        private void _loading()
        {
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                var reader = DatabaseHelper.Select<ChapterItem>("Id,Name,Url", $"WHERE BookId = {_book.Id}");
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        ChaptersList.Add(new ChapterItem(reader));
                    }
                }
                reader.Close();
                _setConent();
                DatabaseHelper.Close();
                RingVisibility = Visibility.Collapsed;
            });
        }

        private void _setConent()
        {
            if (_book.Index >= ChaptersList.Count) return;
            var chapter = ChaptersList[_book.Index];
            Title = chapter.Name;
            var content = Convert.ToString(DatabaseHelper.Find<ChapterItem>("Content", $"Id = {chapter.Id}"));
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(content));
            Content.Blocks.Clear();
            Content.Blocks.Add(paragraph);
        }

        private void _getContent()
        {
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                _setConent();
                DatabaseHelper.Close();
                RingVisibility = Visibility.Collapsed;
            });
        }

        /// <summary>
        /// The <see cref="ChaptersList" /> property's name.
        /// </summary>
        public const string ChaptersListPropertyName = "ChaptersList";

        private ObservableCollection<ChapterItem> _chaptersList = new ObservableCollection<ChapterItem>();

        /// <summary>
        /// Sets and gets the ChaptersList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ChapterItem> ChaptersList
        {
            get
            {
                return _chaptersList;
            }
            set
            {
                Set(ChaptersListPropertyName, ref _chaptersList, value);
            }
        }

        /// <summary>
        /// The <see cref="Content" /> property's name.
        /// </summary>
        public const string ContentPropertyName = "Content";

        private FlowDocument _content = new FlowDocument();

        /// <summary>
        /// Sets and gets the Content property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public FlowDocument Content
        {
            get
            {
                return _content;
            }
            set
            {
                Set(ContentPropertyName, ref _content, value);
            }
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = string.Empty;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(TitlePropertyName, ref _title, value);
            }
        }


        /// <summary>
        /// The <see cref="RingVisibility" /> property's name.
        /// </summary>
        public const string RingVisibilityPropertyName = "RingVisibility";

        private Visibility _ringVisibility = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the RingVisibily property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility RingVisibility
        {
            get
            {
                return _ringVisibility;
            }
            set
            {
                Set(RingVisibilityPropertyName, ref _ringVisibility, value);
            }
        }

        private RelayCommand<int> _chooseCommand;

        /// <summary>
        /// Gets the ChooseCommand.
        /// </summary>
        public RelayCommand<int> ChooseCommand
        {
            get
            {
                return _chooseCommand
                    ?? (_chooseCommand = new RelayCommand<int>(ExecuteChooseCommand));
            }
        }

        private void ExecuteChooseCommand(int index)
        {
            _book.Index = index;
            _getContent();
        }

        private RelayCommand _closeCommand;

        /// <summary>
        /// Gets the CloseCommand.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand
                    ?? (_closeCommand = new RelayCommand(ExecuteCloseCommand));
            }
        }

        private void ExecuteCloseCommand()
        {
            _readItem.Execute(_book);
        }
    }
}