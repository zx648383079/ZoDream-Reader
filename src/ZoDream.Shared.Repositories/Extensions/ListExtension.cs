using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZoDream.Shared.Repositories.Extensions
{
    public static class ListExtension
    {

        public static ObservableCollection<T> ToCollection<T>(this IEnumerable<T> items)
        {
            var data = new ObservableCollection<T>();
            foreach (var item in items)
            {
                data.Add(item);
            }
            return data;
        }

        public static void ToCollection<T>(this IEnumerable<T> items, ICollection<T> target)
        {
            target.Clear();
            foreach (var item in items)
            {
                target.Add(item);
            }
        }

        public static void Remove<T>(this IList<T> items, IEnumerable<T> removeItems)
        {
            foreach (var item in removeItems)
            {
                items.Remove(item);
            }
        }

        /// <summary>
        /// 移动元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected">要移动的元素</param>
        /// <param name="offset">负为前移 正为后移</param>
        public static void Move<T>(this IList<T> items, int selected, int offset)
        {
            if (offset < 0 && selected + offset < 0)
            {
                offset = -selected;
            }
            else if (offset > 0 && selected + offset >= items.Count)
            {
                offset = items.Count - selected - 1;
            }
            if (offset == 0)
            {
                return;
            }
            var item = items[selected];
            items.RemoveAt(selected);
            items.Insert(selected + offset, item);
        }
        /// <summary>
        /// 上移一个位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected"></param>
        public static void MoveUp<T>(this IList<T> items, int selected)
        {
            if (selected < 1)
            {
                return;
            }
            (items[selected - 1], items[selected]) = (items[selected], items[selected - 1]);
        }
        /// <summary>
        /// 下移一个位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected"></param>
        public static void MoveDown<T>(this IList<T> items, int selected)
        {
            if (selected < 0 || selected > items.Count - 2)
            {
                return;
            }
            (items[selected + 1], items[selected]) = (items[selected], items[selected + 1]);
        }

        /// <summary>
        /// 移动到首部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected"></param>
        public static void MoveToFirst<T>(this IList<T> items, int selected)
        {
            if (selected < 1)
            {
                return;
            }
            items.Move(selected, -selected);
        }
        /// <summary>
        /// 移动到尾部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected"></param>
        public static void MoveToLast<T>(this IList<T> items, int selected)
        {
            if (selected < 0 || selected > items.Count - 2)
            {
                return;
            }
            items.Move(selected, items.Count - selected - 1);
        }
    }
}
