using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ZoDream.Reader.Helper;
using ZoDream.Reader.Model;
using System.Data.Common;
using System.Diagnostics;
using ZoDream.Helper.Http;
using ZoDream.Helper.Local;

namespace ZoDream.Reader.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddBookViewModel : ViewModelBase
    {
        private NotificationMessageAction<BookItem> _addItem;
        /// <summary>
        /// Initializes a new instance of the AddBookViewModel class.
        /// </summary>
        public AddBookViewModel()
        {
            Messenger.Default.Register<NotificationMessageAction<BookItem>>(this, "book", m =>
            {
                _addItem = m;
            });
        }

        /// <summary>
        /// The <see cref="Message" /> property's name.
        /// </summary>
        public const string MessagePropertyName = "Message";

        private string _message = string.Empty;

        /// <summary>
        /// Sets and gets the Message property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                Set(MessagePropertyName, ref _message, value);
            }
        }

        /// <summary>
        /// The <see cref="Sources" /> property's name.
        /// </summary>
        public const string SourcesPropertyName = "Sources";

        private Array _sources = Enum.GetValues(typeof(BookSources));

        /// <summary>
        /// Sets and gets the Sources property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Array Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                Set(SourcesPropertyName, ref _sources, value);
            }
        }

        /// <summary>
        /// The <see cref="Kinds" /> property's name.
        /// </summary>
        public const string KindsPropertyName = "Kinds";

        private Array _kinds = Enum.GetValues(typeof(BookKinds));

        /// <summary>
        /// Sets and gets the Kinds property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Array Kinds
        {
            get
            {
                return _kinds;
            }
            set
            {
                Set(KindsPropertyName, ref _kinds, value);
            }
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
        /// The <see cref="Source" /> property's name.
        /// </summary>
        public const string SourcePropertyName = "Source";

        private BookSources _source = BookSources.本地;

        /// <summary>
        /// Sets and gets the Source property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BookSources Source
        {
            get
            {
                return _source;
            }
            set
            {
                Set(SourcePropertyName, ref _source, value);
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
        /// The <see cref="Kind" /> property's name.
        /// </summary>
        public const string KindPropertyName = "Kind";

        private BookKinds _kind = BookKinds.其他;

        /// <summary>
        /// Sets and gets the Kind property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BookKinds Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                Set(KindPropertyName, ref _kind, value);
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
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Url)) return;
            RingVisibility = Visibility.Visible;
            
            Task.Factory.StartNew(() =>
            {
                var item = new BookItem(Name, Source, Kind, Url);
                var conn = DatabaseHelper.Open();
                var count = DatabaseHelper.Find<BookItem>("count(Name)", "Name = @name", new SQLiteParameter("@name", item.Name));
                if (Convert.ToInt32(count) > 0)
                {
                    DatabaseHelper.Close();
                    _showMessage("书名存在，请更改！");
                    RingVisibility = Visibility.Collapsed;
                    return;
                }
                var chapters = new List<ChapterItem>();
                
                var watch = new Stopwatch();
                watch.Start();
                WebRuleItem rule = new WebRuleItem();
                if (item.Source == BookSources.网络)
                {
                    rule = DatabaseHelper.GetRule(item.Url);
                    if (rule == null)
                    {
                        DatabaseHelper.Close();
                        _showMessage("网站规则不存在，请添加！");
                        RingVisibility = Visibility.Collapsed;
                        watch.Stop();
                        return;
                    }
                    chapters.AddRange(HttpHelper.GetBook(ref item, rule));
                }
                else
                {
                    chapters.AddRange(LocalHelper.GetChapters(item.Url));
                }
                var id = DatabaseHelper.InsertId<BookItem>(
                    "Name, Image, Description, Author, Source, Kind, Url, `Index`, Count, Time",
                    "@name,@image,@description,@author,@source,@kind, @url, @index, @count, @time",
                    new SQLiteParameter("@name", item.Name),
                    new SQLiteParameter("@image", item.Image),
                    new SQLiteParameter("@description", item.Description),
                    new SQLiteParameter("@author", item.Author),
                    new SQLiteParameter("@source", item.Source),
                    new SQLiteParameter("@kind", item.Kind),
                    new SQLiteParameter("@url", item.Url),
                    new SQLiteParameter("@index", item.Index),
                    new SQLiteParameter("@count", item.Count),
                    new SQLiteParameter("@time", item.Time));
                LocalHelper.CreateTempDir();
                if (item.Source == BookSources.网络)
                {
                    var result = Parallel.ForEach<ChapterItem>(chapters, chapter =>
                    {
                        var html = new Html();
                        html.SetUrl(chapter.Url);
                        LocalHelper.WriteTemp(html.Narrow(rule.ChapterBegin, rule.ChapterEnd).GetText(rule.Replace), chapter.Content);
                    });
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(1000);
                    }
                }
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (var chapter in chapters)
                    {
                        DatabaseHelper.Insert<ChapterItem>(
                            "Name, Content, BookId, Url",
                            "@Name, @Content, @BookId, @Url",
                            new SQLiteParameter("@Name", chapter.Name),
                            new SQLiteParameter("@Content", LocalHelper.ReadTemp(chapter.Content)),
                            new SQLiteParameter("@BookId", id),
                            new SQLiteParameter("@Url", chapter.Url));
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    _showMessage("章节插入失败，已进行回滚！");
                }
                DatabaseHelper.Close();
                _addItem.Execute(item);
                RingVisibility = Visibility.Collapsed;
                Name = Url = string.Empty;
                watch.Stop();
                _showMessage("执行完成！耗时：" + watch.Elapsed);
            });
        }

        private RelayCommand _openCommand;

        /// <summary>
        /// Gets the OpenCommand.
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand
                    ?? (_openCommand = new RelayCommand(ExecuteOpenCommand));
            }
        }

        private void ExecuteOpenCommand()
        {
            var file = Open.ChooseFile();
            if (string.IsNullOrEmpty(file))
            {
                return;
            }
            Url = file;
            Source = BookSources.本地;
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Regex.Match(file, @"\\([^\.\\]+)\.", RegexOptions.RightToLeft).Groups[1].Value;
            }
        }

        private void _showMessage(string message)
        {
            Message = message;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                Message = string.Empty;
            });
        }
    }
}