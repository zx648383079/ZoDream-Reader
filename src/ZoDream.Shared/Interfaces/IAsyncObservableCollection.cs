using System.Collections.Specialized;
using System.ComponentModel;

namespace ZoDream.Shared.Interfaces
{
    public interface IAsyncObservableCollection<T> : INotifyCollectionChanged, INotifyPropertyChanged
    {

        public bool IsPaused { get; }
        public bool InProgress { get; }

        public int Count { get; }

        public void Add(T item);

        public void Clear();

        public void Start();

        public void Stop();
    }
}
