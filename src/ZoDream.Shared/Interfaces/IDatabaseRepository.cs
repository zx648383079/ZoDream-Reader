using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface IDatabaseRepository: IDisposable
    {

        public IList<BookItem> GetBooks();

        public void AddBook(BookItem item);

        public void UpdateBook(BookItem item);

        public void DeleteBook(object id);

        public void DeleteBook(BookItem item);

        public UserSetting GetSetting();

        public void SaveSetting(UserSetting data);

        public UserSetting ResetSetting();
    }
}
