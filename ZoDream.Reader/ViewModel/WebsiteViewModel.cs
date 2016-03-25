using System.Collections.ObjectModel;
using System.Data.SQLite;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
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
    public class WebsiteViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the WebsiteViewModel class.
        /// </summary>
        public WebsiteViewModel()
        {
            DatabaseHelper.Open();
            var reader = DatabaseHelper.Select<WebsiteItem>();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    WesitesList.Add(new WebsiteItem(reader));
                }
            }
            reader.Close();
            DatabaseHelper.Close();
        }

        /// <summary>
        /// The <see cref="WesitesList" /> property's name.
        /// </summary>
        public const string WesitesListPropertyName = "WesitesList";

        private ObservableCollection<WebsiteItem> _websitesList = new ObservableCollection<WebsiteItem>();

        /// <summary>
        /// Sets and gets the WesitesList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<WebsiteItem> WesitesList
        {
            get
            {
                return _websitesList;
            }
            set
            {
                Set(WesitesListPropertyName, ref _websitesList, value);
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
            new AddWebView().Show();
            Messenger.Default.Send(new NotificationMessageAction<WebsiteItem>(null, item =>
            {
                DatabaseHelper.Open();
                var row = DatabaseHelper.InsertOrIgnore<WebsiteItem>("Name, Url", "@name, @url",
                        new SQLiteParameter("@name", item.Name),
                        new SQLiteParameter("@url", item.Url));
                DatabaseHelper.Close();
                if (row > 0)
                {
                    WesitesList.Add(item);
                }
            }), "web");
        }

        private RelayCommand<int> _editCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand<int> EditCommand
        {
            get
            {
                return _editCommand
                    ?? (_editCommand = new RelayCommand<int>(ExecuteEditCommand));
            }
        }

        private void ExecuteEditCommand(int index)
        {
            if (index < 0 || index >= WesitesList.Count) return;
            new AddWebView().Show();
            Messenger.Default.Send(new NotificationMessageAction<WebsiteItem>(WesitesList[index], null, item =>
            {
                item.Id = WesitesList[index].Id;
                DatabaseHelper.Open();
                var row = DatabaseHelper.Update<WebsiteItem>("Name = @name, Url = @url", $"Id = {item.Id}",
                        new SQLiteParameter("@name", item.Name),
                        new SQLiteParameter("@url", item.Url));
                DatabaseHelper.Close();
                if (row > 0)
                {
                    WesitesList[index] = item;
                }
            }), "web");
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
            if (index < 0 || index >= WesitesList.Count) return;
            
            DatabaseHelper.Open();
            var row = DatabaseHelper.Delete<WebsiteItem>($"Id = {WesitesList[index].Id}");
            DatabaseHelper.Close();
            if (row > 0)
            {
                WesitesList.RemoveAt(index);
            }
        }
    }
}