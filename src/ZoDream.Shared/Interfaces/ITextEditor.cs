using System.Collections.Generic;
using System.IO;

namespace ZoDream.Shared.Interfaces
{
    public interface ITextEditor
    {
        public char NewLine { get; }
        public string Text { get; set; }

        public string SelectedText { get; }

        public bool CanBack { get; }
        public bool CanForward { get; }
        public bool CanUndo { get; }
        public bool CanRedo { get; }
        public int SelectionStart { get; }
        public int SelectionLength { get; }

        public void Undo();
        public void Redo();

        public void LoadFromFile(string fileName);
        public void Load(Stream input);
        /// <summary>
        /// 下一部分
        /// </summary>
        public void GoForward();
        /// <summary>
        /// 上一部分
        /// </summary>
        public void GoBack();

        public bool FindNext(string text);
        public bool FindBack(string text);

        public void Select(int start, int count);
        public void ScrollTo(int position);
        public void Unselect();

        public IDictionary<char, int> Count();
       
    }
}
