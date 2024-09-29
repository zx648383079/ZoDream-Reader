﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Script;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Net
{
    public class NetReader : INovelReader
    {

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public INovelSource CreateSource(INovelSourceEntity entry)
        {
            return new NetSource(entry);
        }

        public INovelDocument GetChapter(Stream input, INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public async Task<INovelDocument> GetChapterAsync(INovelSource source, INovelChapter chapter)
        {
            var inter = new Interpreter();
            return await Task.FromResult(inter.Execute<INovelDocument>("", new NetSpider()));
        }

        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source)
        {
            throw new NotImplementedException();
        }

        public async Task<INovelChapter[]> GetChaptersAsync(INovelSource source)
        {
            var inter = new Interpreter();
            return await Task.FromResult(inter.Execute<INovelChapter[]>("", new NetSpider()));
        }

        public async Task<string> GetChapterAsync(ISourceRule rule,
            string url)
        {
            var inter = new Interpreter();
            var client = new NetSpider();
            client.From(url);
            return await Task.FromResult(inter.Execute<string>(rule.ContentMatchRule, client));
        }

        public async Task<List<INovelChapter>> GetChaptersAsync(ISourceRule rule, 
            string url)
        {
            var inter = new Interpreter();
            var client = new NetSpider();
            client.From(url);
            return await Task.FromResult(inter.Execute<List<INovelChapter>>(rule.DetailMatchRule, client));
        }

        public async Task<List<INovel>> GetExploreAsync(ISourceRule rule, int page = 1)
        {
            if (!rule.EnabledExplore)
            {
                return [];
            }
            var inter = new Interpreter();
            var client = new NetSpider();
            client.From(rule.ExploreUrl.Replace("{{page}}", page.ToString()));
            return await Task.FromResult(inter.Execute<List<INovel>>(rule.ExploreMatchRule, 
                client));
        }

        public async Task<List<INovel>> SearchAsync(ISourceRule rule, string keywords, int page = 1)
        {
            if (!rule.EnabledSearch)
            {
                return [];
            }
            var inter = new Interpreter();
            var client = new NetSpider();
            client.From(rule.SearchUrl.Replace("{{keywords}}", keywords).Replace("{{page}}", page.ToString()));
            return await Task.FromResult(inter.Execute<List<INovel>>(rule.SearchMatchRule,
                client));
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }
    }
}
