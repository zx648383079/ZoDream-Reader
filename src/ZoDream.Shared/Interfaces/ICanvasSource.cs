using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasSource
    {
        public Task<IList<PageItem>> GetAsync(int page);

        public bool Canable(int page);
    }
}
