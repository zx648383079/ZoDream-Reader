using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Storage;

namespace ZoDream.Reader.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly string FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
            "setting.json");

        private Dictionary<string, object> _data = [];
        
        public void Delete(object key)
        {
            Delete(key.ToString());
        }

        public void Delete(string key)
        {
            _data.Remove(key);
        }

        public bool Exist(object key)
        {
            return Exist(key.ToString()); 
        }

        public bool Exist(string key)
        {
            return _data.ContainsKey(key);
        }

        public T Get<T>(object key)
        {
            return Get<T>(key.ToString());
        }

        public T Get<T>(object key, T def)
        {
            return Get(key.ToString(), def);
        }

        public T Get<T>(string key)
        {
            return Get<T>(key, default);
        }

        public T Get<T>(string key, T def)
        {
            if (_data.TryGetValue(key, out var data))
            {
                if (data is JsonElement e)
                {
                    return e.Deserialize<T>();
                }
                return (T)data;
            }
            return def;
        }

        public Task LoadAsync()
        {
            if (!File.Exists(FileName))
            {
                return Task.CompletedTask;
            }
            using var fs = File.OpenRead(FileName);
            var res = JsonSerializer.Deserialize<Dictionary<string, object>>(fs);
            if (res != null)
            {
                _data = res;
            }
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            using var fs = File.OpenWrite(FileName);
            JsonSerializer.Serialize(fs, _data);
            return Task.CompletedTask;
        }

        public void Set<T>(object key, T value)
        {
            Set(key.ToString(), value);
        }

        public void Set<T>(string key, T value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.TryAdd(key, value);
            }
        }
    }
}
