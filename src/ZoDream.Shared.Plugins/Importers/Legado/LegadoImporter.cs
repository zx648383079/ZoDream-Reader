using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Plugins.Importers
{
    public class LeGaDoImporter
    {


        
        public async Task<List<IChapterRule>> LoadChapterRuleAsync(string path)
        {
            var content = await LocationStorage.ReadAsync(path);
            var items = JsonConvert.DeserializeObject<List<TxtTocRule>>(content);
            if (items is null)
            {
                return [];
            }
            var res = new List<IChapterRule>();
            foreach ( var item in items)
            {
                res.Add(new ChapterRuleEntity()
                {
                    Name = item.Name,
                    Example = item.Example,
                    MatchRule = item.Rule,
                    IsEnabled = item.Enable
                });
            }
            return res;
        }
    }
}
