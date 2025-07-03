using System;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Repositories;

namespace ZoDream.Reader.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private ApplicationDataContainer Container;

        public void Delete(string key)
        {
            if (Exist(key))
            {
                Container.Values.Remove(key);
            }
        }

        public T Get<T>(string key)
        {
            return Get<T>(key, default);
        }

        public T Get<T>(string key, T def)
        {
            if (Exist(key))
            {
                if (def is Enum)
                {
                    var tempValue = Container.Values[key].ToString();
                    Enum.TryParse(typeof(T), tempValue, out var result);
                    return (T)result;
                }
                else
                {
                    return (T)Container.Values[key];
                }
            }
            else
            {
                Set(key, def);
                return def;
            }
        }

        public Task LoadAsync()
        {
            Container = ApplicationData.Current.LocalSettings.CreateContainer(AppConstants.SettingContainerName, ApplicationDataCreateDisposition.Always);
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public void Set<T>(string key, T value)
        {
            if (value is Enum)
            {
                Container.Values[key] = value.ToString();
            }
            else
            {
                Container.Values[key] = value;
            }
        }

        public bool Exist(string key)
        {
            return Container.Values.ContainsKey(key);
        }

        public T Get<T>(object key)
        {
            return Get<T>(key.ToString());
        }

        public T Get<T>(object key, T def)
        {
            return Get(key.ToString(), def);
        }

        public void Set<T>(object key, T value)
        {
            Set<T>(key.ToString(), value);
        }

        public void Delete(object key)
        {
            Delete(key.ToString());
        }

        public bool Exist(object key)
        {
            return Exist(key.ToString());
        }
    }
}
