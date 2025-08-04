using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoDream.Reader.ViewModels
{
    public class WordItemViewModel(string word) : ObservableObject
    {
        public WordItemViewModel(char code)
            : this(code.ToString())
        {

        }
        public string Word { get; private set; } = word;

        private int _count;

        public int Count {
            get => _count;
            set => SetProperty(ref _count, value);
        }


        public override string ToString()
        {
            return Word;
        }
    }
}