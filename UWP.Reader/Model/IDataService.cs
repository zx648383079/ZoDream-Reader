using System.Threading.Tasks;

namespace UWP.Reader.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}