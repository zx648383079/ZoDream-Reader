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
    public class WebRulesViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the WebRulesViewModel class.
        /// </summary>
        public WebRulesViewModel()
        {
            DatabaseHelper.Open();
            var reader = DatabaseHelper.Select<WebRuleItem>();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    RulesList.Add(new WebRuleItem(reader));
                }
            }
            reader.Close();
            DatabaseHelper.Close();
        }

        /// <summary>
        /// The <see cref="RulesList" /> property's name.
        /// </summary>
        public const string RulesListPropertyName = "RulesList";

        private ObservableCollection<WebRuleItem> _rulesList = new ObservableCollection<WebRuleItem>();

        /// <summary>
        /// Sets and gets the RulesList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<WebRuleItem> RulesList
        {
            get
            {
                return _rulesList;
            }
            set
            {
                Set(RulesListPropertyName, ref _rulesList, value);
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
            var view = new AddRuleView();
            view.Show();
            Messenger.Default.Send(new NotificationMessageAction<WebRuleItem>(null, item =>
            {
                DatabaseHelper.Open();
                var row =
                    DatabaseHelper.InsertOrIgnore<WebRuleItem>(
                        "Name , Url , CatalogBegin , CatalogEnd , ChapterBegin , ChapterEnd , Replace , AuthorBegin , AuthorEnd , DescriptionBegin , DescriptionEnd , CoverBegin , CoverEnd", 
                        "@Name , @Url , @CatalogBegin , @CatalogEnd , @ChapterBegin , @ChapterEnd , @Replace , @AuthorBegin , @AuthorEnd , @DescriptionBegin , @DescriptionEnd , @CoverBegin , @CoverEnd",
                        new SQLiteParameter("@Name", item.Name),
                        new SQLiteParameter("@Url", item.Url),
                        new SQLiteParameter("@CatalogBegin", item.CatalogBegin),
                        new SQLiteParameter("@CatalogEnd", item.CatalogEnd),
                        new SQLiteParameter("@ChapterBegin", item.ChapterBegin),
                        new SQLiteParameter("@ChapterEnd", item.ChapterEnd),
                        new SQLiteParameter("@Replace", item.Replace),
                        new SQLiteParameter("@AuthorBegin", item.AuthorBegin),
                        new SQLiteParameter("@AuthorEnd", item.AuthorEnd),
                        new SQLiteParameter("@DescriptionBegin", item.DescriptionBegin),
                        new SQLiteParameter("@DescriptionEnd", item.DescriptionEnd),
                        new SQLiteParameter("@CoverBegin", item.CoverBegin),
                        new SQLiteParameter("@CoverEnd", item.CoverEnd));
                DatabaseHelper.Close();
                if (row > 0)
                {
                    RulesList.Add(item);
                    view.Close();
                }
                
            }), "rule");
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
            if (index < 0 || index >= RulesList.Count) return;
            var view = new AddRuleView();
            view.Show();
            Messenger.Default.Send(new NotificationMessageAction<WebRuleItem>(RulesList[index], null, item =>
            {
                item.Id = RulesList[index].Id;
                DatabaseHelper.Open();
                var row =
                    DatabaseHelper.InsertOrIgnore<WebRuleItem>(
                        "Name = @Name, Url = @Url, CatalogBegin = @CatalogBegin, CatalogEnd = @CatalogEnd, ChapterBegin = @ChapterBegin, ChapterEnd = @ChapterEnd, Replace = @Replace, AuthorBegin = @AuthorBegin, AuthorEnd = @AuthorEnd, DescriptionBegin = @DescriptionBegin, DescriptionEnd = @DescriptionEnd, CoverBegin = @CoverBegin, CoverEnd = @CoverEnd",
                        $"Id = {item.Id}",
                        new SQLiteParameter("@Name", item.Name),
                        new SQLiteParameter("@Url", item.Url),
                        new SQLiteParameter("@CatalogBegin", item.CatalogBegin),
                        new SQLiteParameter("@CatalogEnd", item.CatalogEnd),
                        new SQLiteParameter("@ChapterBegin", item.ChapterBegin),
                        new SQLiteParameter("@ChapterEnd", item.ChapterEnd),
                        new SQLiteParameter("@Replace", item.Replace),
                        new SQLiteParameter("@AuthorBegin", item.AuthorBegin),
                        new SQLiteParameter("@AuthorEnd", item.AuthorEnd),
                        new SQLiteParameter("@DescriptionBegin", item.DescriptionBegin),
                        new SQLiteParameter("@DescriptionEnd", item.DescriptionEnd),
                        new SQLiteParameter("@CoverBegin", item.CoverBegin),
                        new SQLiteParameter("@CoverEnd", item.CoverEnd));
                DatabaseHelper.Close();
                if (row > 0)
                {
                    RulesList[index] = item;
                }
                view.Close();
            }), "rule");
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
            if (index < 0 || index >= RulesList.Count) return;
            DatabaseHelper.Open();
            var row = DatabaseHelper.Delete<WebRuleItem>($"Id = {RulesList[index].Id}");
            DatabaseHelper.Close();
            if (row > 0)
            {
                RulesList.RemoveAt(index);
            }
        }
    }
}