using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: BindableBase
    {
        public MainViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var dbFile = await DiskRepository.CreateDatabaseAsync();
            Database.Initialize(dbFile);
            DatabaseRepository = new Database(dbFile);
            Load();

        }

        internal Frame RootFrame;
        internal Frame ChildFrame;
        public Database DatabaseRepository { get; private set; }
        public Disk DiskRepository { get; private set; } = new Disk();

        private ObservableCollection<BookItem> bookItems = new ObservableCollection<BookItem>();

        public ObservableCollection<BookItem> BookItems
        {
            get => bookItems;
            set => Set(ref bookItems, value);
        }

        private AppOption setting = new AppOption();

        public AppOption Setting
        {
            get { return setting; }
            set {
                setting = value;
                DatabaseRepository.SaveSetting(value);
            }
        }


        public void Load()
        {
            var items = DatabaseRepository.GetBooks();
            foreach (var item in items)
            {
                BookItems.Add(item);
            }
            Setting = DatabaseRepository.GetSetting();
        }
        public async void Load(StorageFile file)
        {
            var item = await DiskRepository.AddTxt(file);
            DatabaseRepository.AddBook(item);
            BookItems.Add(item);
        }

        public void Load(IEnumerable<StorageFile> files)
        {
            foreach (var item in files)
            {
                Load(item);
            }
        }



        public void Navigate(Type page, object parameter = null, bool isRoot = false)
        {
            if (isRoot)
            {
                RootFrame.Navigate(page, parameter);
                return;
            }
            if (ChildFrame == null)
            {
                RootFrame.Navigate(typeof(MainPage), parameter);
            }
            ChildFrame?.Navigate(page, parameter);
        }

        public bool CanNavigateBack => RootFrame.CanGoBack || (ChildFrame != null && ChildFrame.CanGoBack);

        /// <summary>
        /// 这个放在每一个单独的页面
        /// </summary>
        public void ListenNavigate(bool hasChild = false)
        {
            var view = SystemNavigationManager.GetForCurrentView();
            view.AppViewBackButtonVisibility = CanNavigateBack ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
            view.BackRequested += (o, e) =>
            {
                e.Handled = true;
                if (ChildFrame != null && ChildFrame.CanGoBack)
                {
                    ChildFrame.GoBack();
                    return;
                }
                if (RootFrame.CanGoBack)
                {
                    RootFrame.GoBack();
                }
            };
            if (hasChild)
            {
                BindChildFrame();
            }
        }

        /// <summary>
        /// 这个放在包含Frame 的页面
        /// </summary>
        public void BindChildFrame()
        {
            if (ChildFrame == null)
            {
                return;
            }
            ChildFrame.Navigated += (o, e) =>
            {
                var view = SystemNavigationManager.GetForCurrentView();
                view.AppViewBackButtonVisibility = CanNavigateBack ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            };
        }
    }
}
