using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Model;
using ZoDream.Reader.View;

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
            DatabaseHelper.Open();
            var reader = DatabaseHelper.Select<BookItem>("*", "ORDER BY Time DESC");
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    BooksList.Add(new BookItem(reader));
                }
            }
            reader.Close();
            DatabaseHelper.Close();
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
                 BooksList.Add(item);
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
            DatabaseHelper.Open();
            var row = DatabaseHelper.Delete<BookItem>($"Id = {BooksList[index].Id}");
            DatabaseHelper.Delete<ChapterItem>($"BookId = {BooksList[index].Id}");
            DatabaseHelper.Close();
            if (row > 0)
            {
                BooksList.RemoveAt(index);
            }
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
            new SystemView().Show();
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
            var file = LocalHelper.ChooseSaveFile(BooksList[index].Name);
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
            });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}