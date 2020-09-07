using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Utility
{
    public static class CollectionHelper
    {
        #region Concurrent Dictionary

        /// <summary>
        /// 添加项
        /// - 等同于TryAdd，仅避免修改代码
        /// </summary>
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (key != null)
                dic.TryAdd(key, value);
        }

        /// <summary>
        /// 设置项
        /// - 如果不存在则添加值
        /// - 如果存在则覆盖值
        /// </summary>
        public static void Renew<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (key != null)
                dic.AddOrUpdate(key, value, (k, oldVar) => value);
        }

        /// <summary>
        /// 移除项
        /// </summary>
        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TKey key)
        {
            TValue v;

            if (key != null)
                dic.TryRemove(key, out v);
        }

        public static bool Add<T, V>(this ConcurrentDictionary<T, List<V>> list, T t, V v, bool isUniqueItem = false)
        {
            if (!list.ContainsKey(t))
                list.Add(t, new List<V>());

            if (isUniqueItem && list[t].Contains(v))
                return false;

            list[t].Add(v);
            return true;
        }

        public static void Remove<T, V>(this ConcurrentDictionary<T, List<V>> list, T key, Predicate<V> prec)
        {
            if (!list.ContainsKey(key))
                return;

            list[key].RemoveAll(prec);

            if (list[key].Count == 0)
                list.Remove(key);
        }

        #endregion
    }
}