using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public interface IFinderSource
    {

        public Task FindAsync(IAsyncObservableCollection<MatchItemViewModel> items, string text);
        public Task FindAsync(IAsyncObservableCollection<MatchItemViewModel> items, Regex pattern);
    }
}
