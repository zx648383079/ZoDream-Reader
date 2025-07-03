using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Behaviors;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Extensions;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Plugins.Umd;
using ZoDream.Shared.Text;

namespace ZoDream.Reader.ViewModels
{
    public class CreateNovelViewModel : ObservableObject, IEditableSectionCommand
    {

        public CreateNovelViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            SaveCommand = new RelayCommand(TapSave);
            BasicCommand = new RelayCommand(TapBasic);
            CatalogCommand = new RelayCommand(TapCatalog);
            AddImageCommand = new RelayCommand(TapAddImage);
            EditCommand = new RelayCommand<IEditableSection>(TapEdit);
            AddNewCommand = new RelayCommand<IEditableSection>(TapAddNew);
            SortCommand = new RelayCommand<DragItemsResult>(TapSort);
            MoveBottomCommand = new RelayCommand<IEditableSection>(TapMoveBottom);
            MoveDownCommand = new RelayCommand<IEditableSection>(TapMoveDown);
            MoveUpCommand = new RelayCommand<IEditableSection>(TapMoveUp);
            MoveTopCommand = new RelayCommand<IEditableSection>(TapMoveTop);
            DeleteCommand = new RelayCommand<IEditableSection>(TapDelete);

            PreviousCommand = new RelayCommand(TapPrevious);
            NextCommand = new RelayCommand(TapNext);
            UndoCommand = new RelayCommand(TapUndo);
            RedoCommand = new RelayCommand(TapRedo);
            CheckCommand = new RelayCommand(TapCheck);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private OwnDictionary? _dict;
        private IEditableSection? _current;
        private bool _isUpdated = false;

        public ITextEditor? Document { get; internal set; }

        private string _name = string.Empty;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _author = string.Empty;

        public string Author {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _brief = string.Empty;

        public string Brief {
            get => _brief;
            set => SetProperty(ref _brief, value);
        }

        private string _cover = string.Empty;

        public string Cover {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        private string _title = string.Empty;

        public string Title {
            get => _title;
            set {
                SetProperty(ref _title, value);
                _isUpdated = true;
            }
        }

        private string _content = string.Empty;

        public string Content {
            get => _content;
            set {
                SetProperty(ref _content, value);
                _isUpdated = true;
            }
        }

        private ObservableCollection<IEditableSection> _items = [];

        public ObservableCollection<IEditableSection> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private IEditableSection? _selectedItem;

        public IEditableSection? SelectedItem {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }


        public Visibility BasicVisible => IsBasic ? Visibility.Visible : Visibility.Collapsed;
        public Visibility CatalogVisible => !IsBasic ? Visibility.Visible : Visibility.Collapsed;

        private bool _isBasic = true;

        public bool IsBasic {
            get => _isBasic;
            set {
                _isBasic = value;
                OnPropertyChanged(nameof(BasicVisible));
                OnPropertyChanged(nameof(CatalogVisible));
            }
        }

        private bool _undoEnabled;

        public bool UndoEnabled {
            get => _undoEnabled;
            set => SetProperty(ref _undoEnabled, value);
        }

        private bool _redoEnabled;

        public bool RedoEnabled {
            get => _redoEnabled;
            set => SetProperty(ref _redoEnabled, value);
        }


        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CatalogCommand { get; private set; }
        public ICommand BasicCommand { get; private set; }
        public ICommand AddImageCommand { get; private set; }

        public ICommand EditCommand { get; private set; }
        public ICommand AddNewCommand { get; private set; }
        public ICommand CheckCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }

        public ICommand SortCommand { get; private set; }
        public ICommand MoveTopCommand { get; private set; }
        public ICommand MoveBottomCommand { get; private set; }
        public ICommand MoveUpCommand { get; private set; }
        public ICommand MoveDownCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }


        private async void TapOpen()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".npk");
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".epub");
            picker.FileTypeFilter.Add(".umd");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            INovelReader? reader = null;
            var extension = Path.GetExtension(file.Path);
            if (extension == ".txt")
            {
                var dialog = new ChapterMatchDialog();
                _ = dialog.ViewModel.LoadAsync(file.Path);
                if (!await _app.OpenFormAsync(dialog))
                {
                    return;
                }
                reader = new TxtReader(await file.OpenStreamForReadAsync(), 
                    file.Path, dialog.ViewModel.RuleText);
            } else if (extension == ".umd")
            {
                reader = new UmdReader(await file.OpenStreamForReadAsync());
            }
            else if (extension == ".epub")
            {
                reader = new EPubReader(await file.OpenStreamForReadAsync());
            }
            var doc = reader?.Read();
            reader?.Dispose();
            if (doc is null)
            {
                return;
            }
            Name = doc.Name;
            Author = doc.Author;
            Brief = doc.Brief;
            Cover = doc.Cover?.ToBase64String();
            Items.Clear();
            foreach (var item in doc.Items)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    Items.Add(new VolumeItemViewModel(this)
                    {
                        Title = item.Name,
                    });
                }
                foreach (var it in item)
                {
                    Items.Add(new ChapterItemViewModel(this)
                    {
                        Title = it.Title,
                        Items = it.Items
                    });
                }
            }
        }

        private async void TapSave()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("书籍", [".npk"]);
            var file = await picker.PickSaveFileAsync();

        }
        private void TapBasic()
        {
            IsBasic = true;
        }
        private void TapCatalog()
        {
            IsBasic = false;
            if (_current is not null)
            {
                return;
            }
            if (Items.Count == 0)
            {
                Items.Add(new ChapterItemViewModel(this));
            }
            EditSection(Items[0]);
        }

        private void TapEdit(IEditableSection? model)
        {
            model ??= SelectedItem;
            if (model is null)
            {
                return;
            }
            EditSection(model);
        }

        private void SaveSection(IEditableSection? section)
        {
            if (!_isUpdated || section is null)
            {
                return;
            }
            section.Title = Title;
            if (section is ChapterItemViewModel o)
            {
                o.Text = Content;
            }
            _isUpdated = false;
        }

        private void EditSection(IEditableSection model)
        {
            SaveSection(_current);
            _current = model;
            Title = _current.Title;
            Content = _current is ChapterItemViewModel o ? o.Text : string.Empty;
            _isUpdated = false;
        }

        private async void TapCheck()
        {
            if (Document is null)
            {
                return;
            }
            if (_dict is null)
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".npk");
                picker.FileTypeFilter.Add(".txt");
                picker.FileTypeFilter.Add(".epub");
                picker.FileTypeFilter.Add(".umd");
                _app.InitializePicker(picker);
                var file = await picker.PickSingleFileAsync();
                if (file is null)
                {
                    return;
                }
                _dict = OwnDictionary.OpenFile(await file.OpenStreamForReadAsync());
            }
            foreach (var item in Document.Text)
            {
                if (_dict.TrySerialize(item, out _))
                {
                    continue;
                }
                Document.FindNext(item.ToString());
                break;
            }
        }

        private void TapSort(DragItemsResult? data)
        {
            if (data is null || data.ItemsIndex.Length == 0)
            {
                return;
            }
            Items.Move(data.ItemsIndex[0], data.TargetIndex);
        }

        private void TapAddNew(IEditableSection? arg)
        {
            var i = 0;
            if (arg is null)
            {
                Items.Add(new ChapterItemViewModel(this));
                i = Items.Count - 1;
            }
            else
            {
                i = Items.IndexOf(arg);
                Items.Insert(i, new ChapterItemViewModel(this));
            }
            EditSection(Items[i]);
        }

        private void TapMoveTop(IEditableSection? arg)
        {
            arg ??= SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.MoveToFirst(Items.IndexOf(arg));
        }

        private void TapMoveBottom(IEditableSection? arg)
        {
            arg ??= SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.MoveToLast(Items.IndexOf(arg));
        }

        private void TapMoveUp(IEditableSection? arg)
        {
            arg ??= SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.MoveUp(Items.IndexOf(arg));
        }
        private void TapMoveDown(IEditableSection? arg)
        {
            arg ??= SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.MoveDown(Items.IndexOf(arg));
        }

        private void TapDelete(IEditableSection? arg)
        {
            arg ??= SelectedItem;
            if (arg is null)
            {
                return;
            }
            Items.Remove(arg);
        }

        public void TapPrevious()
        {
            var i = _current is null ? Items.Count : Items.IndexOf(_current);
            if (i <= 0)
            {
                return;
            }
            EditSection(SelectedItem = Items[i - 1]);
        }

        public void TapNext()
        {
            var i = _current is null ? Items.Count : Items.IndexOf(_current);
            if (i < Items.Count - 2)
            {
                return;
            }
            EditSection(SelectedItem = Items[i + 1]);
        }

        public void TapRedo()
        {
            Document?.Redo();
        }

        public void TapUndo()
        {
            Document?.Undo();
        }

        private void TapAddImage()
        {
            //if (Document is null)
            //{
            //    return;
            //}
            //var picker = new FileOpenPicker();
            //picker.FileTypeFilter.Add(".jpg");
            //picker.FileTypeFilter.Add(".jpeg");
            //picker.FileTypeFilter.Add(".png");
            //_app.InitializePicker(picker);
            //var file = await picker.PickSingleFileAsync();
            //if (file is null)
            //{
            //    return;
            //}
            //var input = await file.OpenReadAsync();
            //Document.Selection.TypeText("\n");
            //Document.Selection.InsertImage(0, 0, 0, VerticalCharacterAlignment.Baseline, file.DisplayName, input);
            //Document.Selection.Move(TextRangeUnit.Paragraph, 1);
            //Document.Selection.TypeText("\n");
        }

    }
}
