using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public interface IEditableSection
    {
        public string Title { get; set; }
        public bool IsWrong { get; set; }
    }

    public interface IEditableSectionCommand
    {
        public ICommand EditCommand { get; }
        public ICommand AddNewCommand { get; }
        public ICommand MoveTopCommand { get; }
        public ICommand MoveBottomCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }

        public ICommand DeleteCommand { get; }
    }

    public abstract class EditableSectionBase : ObservableObject, IEditableSection, IEditableSectionCommand
    {
        protected EditableSectionBase(IEditableSectionCommand host)
        {
            _host = host;
            EditCommand = new RelayCommand(TapEdit);
            AddNewCommand = new RelayCommand(TapAddNew);
            MoveBottomCommand = new RelayCommand(TapMoveBottom);
            MoveDownCommand = new RelayCommand(TapMoveDown);
            MoveUpCommand = new RelayCommand(TapMoveUp);
            MoveTopCommand = new RelayCommand(TapMoveTop);
            DeleteCommand = new RelayCommand(TapDelete);
        }

        private readonly IEditableSectionCommand _host;

        private string _title = string.Empty;

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        private bool _isWrong;

        public bool IsWrong {
            get => _isWrong;
            set => SetProperty(ref _isWrong, value);
        }


        public ICommand EditCommand { get; private set; }

        public ICommand AddNewCommand { get; private set; }
        public ICommand MoveTopCommand { get; private set; }
        public ICommand MoveBottomCommand { get; private set; }
        public ICommand MoveUpCommand { get; private set; }
        public ICommand MoveDownCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        private void TapEdit()
        {
            _host.EditCommand.Execute(this);
        }

        private void TapAddNew()
        {
            _host.AddNewCommand.Execute(this);
        }

        private void TapMoveTop()
        {
            _host.MoveTopCommand.Execute(this);
        }

        private void TapMoveBottom()
        {
            _host.MoveBottomCommand.Execute(this);
        }

        private void TapMoveUp()
        {
            _host.MoveUpCommand.Execute(this);
        }
        private void TapMoveDown()
        {
            _host.MoveDownCommand.Execute(this);
        }

        private void TapDelete()
        {
            _host.DeleteCommand.Execute(this);
        }
    }

    public class ChapterItemViewModel(IEditableSectionCommand host) : EditableSectionBase(host), INovelSection
    {

        public IList<INovelBlock> Items { get; set; } = [];
        public string Text 
        {
            get {
                var sb = new StringBuilder();
                foreach (var item in Items)
                {
                    if (item is INovelTextBlock o)
                    {
                        sb.Append("    ");
                        sb.Append(o.Text);
                        sb.Append('\n');
                    }
                }
                return sb.ToString();
            }
            set {
                Items.Clear();
                foreach (var item in value.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        Items.Add(new NovelTextBlock(item.Trim()));
                    }
                }
            }
        }
    }

    public class VolumeItemViewModel(IEditableSectionCommand host) : EditableSectionBase(host), IEditableSection
    {
    }
}
