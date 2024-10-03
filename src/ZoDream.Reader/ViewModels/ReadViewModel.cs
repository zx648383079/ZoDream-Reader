using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Net;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public partial class ReadViewModel: ObservableObject
    {
        public ReadViewModel()
        {
            _app = App.GetService<AppViewModel>();
            SourceService = new NovelService(this);
        }

        private readonly AppViewModel _app;

        private ICanvasSource _sourceService;

        public ICanvasSource SourceService {
            get => _sourceService;
            set => SetProperty(ref _sourceService, value);
        }

        private string chapterTitle = string.Empty;

        public string ChapterTitle
        {
            get => chapterTitle;
            set => SetProperty(ref chapterTitle, value);
        }

        private bool isLoading = true;

        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }


        private ObservableCollection<ChapterModel> chapterItems = [];

        public ObservableCollection<ChapterModel> ChapterItems
        {
            get => chapterItems;
            set => SetProperty(ref chapterItems, value);
        }

    }
}
