using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;

namespace ZoDream.Reader.ViewModels
{
    public partial class ReadViewModel: ObservableObject, IQueryAttributable
    {

        public ReadViewModel()
        {
            CatalogCommand = new RelayCommand(TapCatalog);
            PreviousCommand = new RelayCommand(TapPrevious);
            NextCommand = new RelayCommand(TapNext);
            SourceCommand = new RelayCommand(TapSource);
            SettingCommand = new RelayCommand(TapSetting);
            SourceService = new NovelService(this);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ICanvasSource _sourceService;

        public ICanvasSource SourceService {
            get => _sourceService;
            set => SetProperty(ref _sourceService, value);
        }



        private string chapterTitle = string.Empty;

        public string ChapterTitle {
            get => chapterTitle;
            set => SetProperty(ref chapterTitle, value);
        }

        private ObservableCollection<ChapterModel> _chapterItems = [];

        public ObservableCollection<ChapterModel> ChapterItems {
            get => _chapterItems;
            set => SetProperty(ref _chapterItems, value);
        }

        private bool catalogOpen;

        public bool CatalogOpen {
            get => catalogOpen;
            set => SetProperty(ref catalogOpen, value);
        }

        public ICommand CatalogCommand { get; private set; }
        public ICommand SourceCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        private void TapCatalog()
        {
            CatalogOpen = !CatalogOpen;
        }

        private void TapPrevious()
        {
            
        }
        private void TapNext()
        {

        }

        private async void TapSource()
        {
            var app = App.GetService<AppViewModel>();
            var dialog = new SearchSourceDialog();
            await app.OpenDialogAsync(dialog);
        }

        private void TapSetting()
        {

        }

        public void ApplyQueryAttributes(IDictionary<string, object> queries)
        {
            if (queries.TryGetValue("novel", out var obj) && obj is INovel novel)
            {
                _ = LoadAsync(novel);
            }
        }

        public async Task LoadAsync(INovel arg)
        {
            var book = await GetBookAsync(arg);
            if (book == null)
            {
                return;
            }
            _novel = book;
            var data = await _app.Database.GetChapterAsync<ChapterModel>(arg.Id);
            data.ToCollection(ChapterItems);
            ChapterTitle = ChapterItems[_novel.CurrentChapterIndex].Title;
        }

        private async Task<BookEntity?> GetBookAsync(INovel arg)
        {
            if (arg is BookEntity e)
            {
                return e;
            }
            return await _app.Database.GetBookAsync<BookEntity>(arg.Id);
        }
    }
}
