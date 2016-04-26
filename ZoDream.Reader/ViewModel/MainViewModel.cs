using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Model;
using ZoDream.Reader.View;
using ZoDream.Helper.Local;
using ZoDream.Helper.Http;

namespace ZoDream.Reader.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                try
                {
                    var reader = DatabaseHelper.Select<BookItem>("*", "ORDER BY Time DESC");
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            var item = new BookItem(reader);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                BooksList.Add(item);
                            });
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    DatabaseHelper.Init();
                }
                DatabaseHelper.Close();
                RingVisibility = Visibility.Collapsed;
            });
        }

        /// <summary>
        /// The <see cref="BooksList" /> property's name.
        /// </summary>
        public const string BooksListPropertyName = "BooksList";

        private ObservableCollection<BookItem> _booksList = new ObservableCollection<BookItem>();

        /// <summary>
        /// Sets and gets the BooksList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<BookItem> BooksList
        {
            get
            {
                return _booksList;
            }
            set
            {
                Set(BooksListPropertyName, ref _booksList, value);
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

        private RelayCommand _addCommand;

        /// <summary>
        /// Gets the AddCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(ExecuteAddCommand));
            }
        }

        private void ExecuteAddCommand()
        {
            new AddBookView().Show();
            Messenger.Default.Send(new NotificationMessageAction<BookItem>(null, item =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    BooksList.Add(item);
                });
            }), "book");
        }

        private RelayCommand<int> _deleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand<int> DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand<int>(ExecuteDeleteCommand));
            }
        }

        private void ExecuteDeleteCommand(int index)
        {
            if (index < 0 || index >= BooksList.Count) return;
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                var row = DatabaseHelper.Delete<BookItem>($"Id = {BooksList[index].Id}");
                DatabaseHelper.Delete<ChapterItem>($"BookId = {BooksList[index].Id}");
                DatabaseHelper.Close();
                if (row > 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        BooksList.RemoveAt(index);
                    });
                }
                RingVisibility = Visibility.Collapsed;
            });
        }

        private RelayCommand _websiteCommand;

        /// <summary>
        /// Gets the WebsiteCommand.
        /// </summary>
        public RelayCommand WebsiteCommand
        {
            get
            {
                return _websiteCommand
                    ?? (_websiteCommand = new RelayCommand(ExecuteWebsiteCommand));
            }
        }

        private void ExecuteWebsiteCommand()
        {
            new WebsiteView().Show();
        }

        private RelayCommand _ruleCommand;

        /// <summary>
        /// Gets the RuleCommand.
        /// </summary>
        public RelayCommand RuleCommand
        {
            get
            {
                return _ruleCommand
                    ?? (_ruleCommand = new RelayCommand(ExecuteRuleCommand));
            }
        }

        private void ExecuteRuleCommand()
        {
            new WebRulesView().Show();
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
            //new ReadViewerView().Show();
            new SystemView().ShowDialog();
        }


        private RelayCommand<int> _readCommand;

        /// <summary>
        /// Gets the ReadCommand.
        /// </summary>
        public RelayCommand<int> ReadCommand
        {
            get
            {
                return _readCommand
                    ?? (_readCommand = new RelayCommand<int>(ExecuteReadCommand));
            }
        }

        private void ExecuteReadCommand(int index)
        {
            if (index < 0 || index >= BooksList.Count) return;
            new ReadView().Show();
            Messenger.Default.Send(new NotificationMessageAction<BookItem>(BooksList[index], null, item =>
            {
                item.Time = DateTime.Now;
                DatabaseHelper.Open();
                DatabaseHelper.Update<BookItem>(
                    "`Index` = @index, Time = @time", 
                    $"Id = {item.Id}",
                    new SQLiteParameter("@index", item.Index),
                    new SQLiteParameter("@time", item.Time));
                DatabaseHelper.Close();
                BooksList[index] = item;
            }), "read");
        }

        private RelayCommand<int> _exportCommand;

        /// <summary>
        /// Gets the ExportCommand.
        /// </summary>
        public RelayCommand<int> ExportCommand
        {
            get
            {
                return _exportCommand
                    ?? (_exportCommand = new RelayCommand<int>(ExecuteExportCommand));
            }
        }

        private void ExecuteExportCommand(int index)
        {
            if (index < 0 || index >= BooksList.Count) return;
            RingVisibility = Visibility.Visible;
            var file = Open.ChooseSaveFile(BooksList[index].Name);
            if (file == null) return;
            Task.Factory.StartNew(() =>
            {
                DatabaseHelper.Open();
                var writer = new StreamWriter(file, false, Encoding.UTF8);
                var reader = DatabaseHelper.Select<ChapterItem>("Name,Content", $"WHERE BookId = {BooksList[index].Id}");
                while (reader.Read())
                {
                    if (!reader.HasRows) continue;
                    writer.WriteLine(reader[0].ToString());
                    writer.WriteLine();
                    writer.WriteLine(reader[1].ToString());
                    writer.WriteLine();
                    writer.WriteLine();
                }
                reader.Close();
                writer.Close();
                DatabaseHelper.Close();
                RingVisibility = Visibility.Collapsed;
            });
        }

        private RelayCommand<int> _updateCommand;

        /// <summary>
        /// Gets the UpdateCommand.
        /// </summary>
        public RelayCommand<int> UpdateCommand
        {
            get
            {
                return _updateCommand
                    ?? (_updateCommand = new RelayCommand<int>(ExecuteUpdateCommand));
            }
        }

        private void ExecuteUpdateCommand(int index)
        {
            if (index < 0 || index >= BooksList.Count || BooksList[index].Source == BookSources.本地) return;
            RingVisibility = Visibility.Visible;
            Task.Factory.StartNew(() =>
            {
                var item = BooksList[index];
                var conn = DatabaseHelper.Open();
                var rule = DatabaseHelper.GetRule(item.Url);
                var chapters = HttpHelper.GetChapters(item, rule, new Html().SetUrl(item.Url));
                var length = chapters.Count;
                if (item.Count < length)
                {
                    var result = Parallel.For((long) item.Count, length, i =>
                    {
                        var chapter = chapters[Convert.ToInt32(i)];
                        var html = new Html();
                        html.SetUrl(chapter.Url);
                        LocalHelper.WriteTemp(html.Narrow(rule.ChapterBegin, rule.ChapterEnd).GetText(rule.Replace), chapter.Content);
                    });
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(1000);
                    }
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        for (var i = item.Count; i < length; i++)
                        {
                            var chapter = chapters[i];
                            DatabaseHelper.Insert<ChapterItem>(
                                "Name, Content, BookId, Url",
                                "@Name, @Content, @BookId, @Url",
                                new SQLiteParameter("@Name", chapter.Name),
                                new SQLiteParameter("@Content", LocalHelper.ReadTemp(chapter.Content)),
                                new SQLiteParameter("@BookId", item.Id),
                                new SQLiteParameter("@Url", chapter.Url));
                        }
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
                DatabaseHelper.Close();
                RingVisibility = Visibility.Collapsed;
            });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}