using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZoDream.Shared.Database.Mappers
{
    public class EnumMapper : IDisposable
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _cacheItems = new();
        private readonly ReaderWriterLockSlim _lock = new();
        public object FromString(Type type, string value)
        {
            PopulateIfNotPresent(type);
            if (!_cacheItems[type].TryGetValue(value, out var res))
            {
                throw new Exception(string.Format("The value '{0}' could not be found for Enum '{1}'", value, type));
            }
            return res;
        }

        private void PopulateIfNotPresent(Type type)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (!_cacheItems.ContainsKey(type))
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        Populate(type);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        private void Populate(Type type)
        {
            var values = Enum.GetValues(type);
            _cacheItems[type] = new Dictionary<string, object>(values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                object value = values.GetValue(i);
                _cacheItems[type].Add(value.ToString(), value);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
