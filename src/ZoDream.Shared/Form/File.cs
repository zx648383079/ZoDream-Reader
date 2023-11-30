using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Form
{
    public class FileFormInput(string name, string label, bool required) : IFormInput
    {
        public string Name { get; private set; } = name;

        public string Label { get; private set; } = label;

        public bool Required { get; private set; } = required;
        public bool IsSave { get; private set; }
        public bool IsFolder { get; private set; }
        public string Tip { get; private set; } = string.Empty;

        public bool TryParse(ref object input)
        {
            return true;
        }

        public FileFormInput(string name, string label, 
            bool required, bool isSave, bool isFolder): this(name, label, required)
        {
            IsSave = isSave;
            IsFolder = isFolder;
        }
    }
}
