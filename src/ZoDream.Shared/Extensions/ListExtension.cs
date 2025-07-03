using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZoDream.Shared.Extensions
{
    public static class ListExtension
    {
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
        public static void MoveOffset<T>(this IList<T> items, int selected, int offset)
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
        /// 移动元素到新的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selected"></param>
        /// <param name="newIndex"></param>
        public static void Move<T>(this IList<T> items, int selected, int newIndex)
        {
            MoveOffset(items, selected, newIndex - selected);
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

        public static void MoveUp<T>(this ObservableCollection<T> items, int selected)
        {
            if (selected < 1)
            {
                return;
            }
            items.Move(selected, selected - 1);
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

        public static void MoveDown<T>(this ObservableCollection<T> items, int selected)
        {
            if (selected < 0 || selected > items.Count - 2)
            {
                return;
            }
            items.Move(selected, selected + 1);
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
            MoveOffset(items, selected, -selected);
        }

        public static void MoveToFirst<T>(this ObservableCollection<T> items, int selected)
        {
            if (selected < 1)
            {
                return;
            }
            items.Move(selected, 0);
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
            MoveOffset(items, selected, items.Count - selected - 1);
        }

        public static void MoveToLast<T>(this ObservableCollection<T> items, int selected)
        {
            if (selected < 0 || selected > items.Count - 2)
            {
                return;
            }
            items.Move(selected, items.Count - 1);
        }
    }
}
