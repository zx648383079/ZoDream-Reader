using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;

namespace ZoDream.Reader.ViewModels
{
    public class CreateNovelViewModel : ObservableObject
    {

        public CreateNovelViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            SaveCommand = new RelayCommand(TapSave);
            BasicCommand = new RelayCommand(TapBasic);
            CatalogCommand = new RelayCommand(TapCatalog);
            AddImageCommand = new RelayCommand(TapAddImage);
            EditCommand = new RelayCommand<ChapterItemViewModel>(TapEdit);

            PreviousCommand = new RelayCommand(TapPrevious);
            NextCommand = new RelayCommand(TapNext);
            UndoCommand = new RelayCommand(TapUndo);
            RedoCommand = new RelayCommand(TapRedo);
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();
        private ChapterItemViewModel? _current;
        //public RichEditTextDocument? Document { get; internal set; }

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
            set => SetProperty(ref _title, value);
        }

        private string _content = string.Empty;

        public string Content {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private ObservableCollection<IEditableSection> _items = [];

        public ObservableCollection<IEditableSection> Items {
            get => _items;
            set => SetProperty(ref _items, value);
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
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }


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
            var dialog = new ChapterMatchDialog();
            await _app.OpenDialogAsync(dialog);
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
        }

        private void TapEdit(ChapterItemViewModel? model)
        {

        }

        public void TapPrevious()
        {

        }

        public void TapNext()
        {

        }

        public void TapRedo()
        {
            //Document?.Redo();
        }

        public void TapUndo()
        {
            //Document?.Undo();
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
