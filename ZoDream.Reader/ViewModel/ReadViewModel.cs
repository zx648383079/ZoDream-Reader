using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Helper.Http;
using ZoDream.Reader.Model;
using ZoDream.Reader.View;

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

        private NotificationMessageAction<int> _readViewer;

        private BookItem _book;
        /// <summary>
        /// Initializes a new instance of the ReadViewModel class.
        /// </summary>
        public ReadViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<int>>(this, "readViewer", m =>
            {
                _readViewer = m;
            });
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
                SystemHelper.Open();
                var reader = DatabaseHelper.Select<ChapterItem>("Id,Name,Url", $"WHERE BookId = {_book.Id}");
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        var item = new ChapterItem(reader);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ChaptersList.Add(item);
                        });
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                _setConent(content);
            });
        }

        private void _setConent(string content)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(content));
            Content = new FlowDocument();
            _reset();
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

        private void _reset()
        {
            Content.FontFamily = SystemHelper.GetFontFamily("FontFamily");
            Content.Background = SystemHelper.GetBrush("Background");
            Content.FontSize = SystemHelper.GetInt("FontSize");
            Content.FontWeight = SystemHelper.GetFontWeight("FontWeight");
            Content.Foreground = new SolidColorBrush(SystemHelper.GetColor("Foreground"));
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

        private FlowDocument _content;

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
            if (index == _book.Index) return;
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
                return _closeCommand ?? (_closeCommand = new RelayCommand(
                    ExecuteCloseCommand));
            }
        }

        private void ExecuteCloseCommand()
        {
            _readItem.Execute(_book);
        }

        private RelayCommand _updateCommand;

        /// <summary>
        /// Gets the UpdateCommand.
        /// </summary>
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand
                    ?? (_updateCommand = new RelayCommand(ExecuteUpdateCommand));
            }
        }

        private void ExecuteUpdateCommand()
        {
            if (_book.Source == BookSources.本地) return;
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                var rule = DatabaseHelper.GetRule(_book.Url);
                var chapter = ChaptersList[_book.Index];
                var html = new Html();
                html.SetUrl(chapter.Url);
                var content = html.Narrow(rule.ChapterBegin, rule.ChapterEnd).GetText(rule.Replace);
                DatabaseHelper.Update<ChapterItem>(
                    "Content = @content", 
                    $"Id = {chapter.Id}",
                    new SQLiteParameter("@content", content));
                DatabaseHelper.Close();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _setConent(content);
                });
                RingVisibility = Visibility.Collapsed;
            });
        }

        private RelayCommand _previousCommand;

        /// <summary>
        /// Gets the PreviousCommand.
        /// </summary>
        public RelayCommand PreviousCommand
        {
            get
            {
                return _previousCommand
                    ?? (_previousCommand = new RelayCommand(ExecutePreviousCommand));
            }
        }

        private void ExecutePreviousCommand()
        {
            if (_book.Index < 1) return;
            _book.Index --;
            _getContent();
        }

        private RelayCommand _nextCommand;

        /// <summary>
        /// Gets the NextCommand.
        /// </summary>
        public RelayCommand NextCommand
        {
            get
            {
                return _nextCommand
                    ?? (_nextCommand = new RelayCommand(ExecuteNextCommand));
            }
        }

        private void ExecuteNextCommand()
        {
            if (_book.Index > ChaptersList.Count - 2) return;
            _book.Index ++;
            _getContent();
        }

        private RelayCommand _openWebCommand;

        /// <summary>
        /// Gets the OpenWebCommand.
        /// </summary>
        public RelayCommand OpenWebCommand
        {
            get
            {
                return _openWebCommand
                    ?? (_openWebCommand = new RelayCommand(ExecuteOpenWebCommand));
            }
        }

        private void ExecuteOpenWebCommand()
        {
            if (_book.Source == BookSources.本地) return;
            LocalHelper.OpenBrowser(ChaptersList[_book.Index].Url);
        }

        private RelayCommand _systemCommand;

        /// <summary>
        /// Gets the SystemCommand.
        /// </summary>
        public RelayCommand SystemCommand
        {
            get
            {
                return _systemCommand
                    ?? (_systemCommand = new RelayCommand(ExecuteSystemCommand));
            }
        }

        private void ExecuteSystemCommand()
        {
            var result = new SystemView().ShowDialog();
            if (result == true)
            {
                _reset();
            }
        }

        private readonly SpeechSynthesizer _speecher = new SpeechSynthesizer();


        /// <summary>
            /// The <see cref="Speech" /> property's name.
            /// </summary>
        public const string SpeechPropertyName = "Speech";

        private bool _speech = false;

        /// <summary>
        /// Sets and gets the Speech property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Speech
        {
            get
            {
                return _speech;
            }
            set
            {
                Set(SpeechPropertyName, ref _speech, value);
                _changedSpeech();
            }
        }

        private void _changedSpeech()
        {
            if (Speech)
            {
                var content = ((Run)((Paragraph)Content.Blocks.FirstBlock).Inlines.FirstInline).Text;
                _speecher.SelectVoiceByHints(VoiceGender.Female);
                _speecher.SpeakAsync(content);
            }
            else
            {
                _speecher.SpeakAsyncCancelAll();
            }
        }
    }
}