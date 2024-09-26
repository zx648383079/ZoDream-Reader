using System;

namespace ZoDream.Shared.Repositories.Extensions
{
    public static class ModelExtension
    {
        public static void CopyTo<T>(this object source, T target)
            where T : class
        {
            var sourceType = source.GetType();
            if (!sourceType.IsClass)
            {
                return;
            }
            var targetType = typeof(T);
            foreach (var item in targetType.GetProperties())
            {
                if (!item.CanWrite)
                {
                    continue;
                }
                var sourceProperty = sourceType.GetProperty(item.Name);
                if (sourceProperty is null || !sourceProperty.CanRead)
                {
                    continue;
                }
                item.SetValue(target, sourceProperty.GetValue(source));
            }
        }

        public static T Clone<T>(this object source)
            where T : class, new()
        {
            var target = (T)Activator.CreateInstance(typeof(T));
            if (!source.GetType().IsClass)
            {
                return target;
            }
            source.CopyTo(target);
            return target;
        }
    }
}
