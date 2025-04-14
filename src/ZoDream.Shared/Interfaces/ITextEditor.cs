using System.IO;

namespace ZoDream.Shared.Interfaces
{
    public interface ITextEditor
    {
        public string Text { get; set; }

        public void LoadFromFile(string fileName);
        public void Load(Stream input);

        public bool FindNext(string text);

        public void Select(int start, int count);
        public void ScrollTo(int position);
        public void Unselect();
        
    }
}
