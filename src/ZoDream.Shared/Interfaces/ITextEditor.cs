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
        /// <summary>
        /// 清除 Undo Redo 历史记录
        /// </summary>
        public void ResetUndo();

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
        /// <summary>
        /// 粘贴字符串
        /// </summary>
        /// <param name="text"></param>
        public void Paste(string text);

        public void Select(int start, int count);
        public void ScrollTo(int position);
        public void Unselect();
        /// <summary>
        /// 焦点
        /// </summary>
        public void Focus();
        /// <summary>
        /// 添加换行
        /// </summary>
        public void AddNewLine();
        /// <summary>
        /// 从指定位置截断，并返回后面部分的内容
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string Split(int position);
        /// <summary>
        /// 从选择的起始位置截断，并返回后面部分的内容
        /// </summary>
        /// <returns></returns>
        public string Split();

        public IDictionary<char, int> Count();
        
    }
}
