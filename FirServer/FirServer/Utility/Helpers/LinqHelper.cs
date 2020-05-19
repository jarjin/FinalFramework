using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirServer.Utility;

namespace Utility
{
   public static class LinqHelper
    {
        #region Get MethodValues

        public static List<int> GetListInt(this string str, string splitor = ",")
        {
            var array = str.SplitTrim(splitor).ToArray();
            var intArray = Array.ConvertAll<string, int>(array, s => Convert.ToInt32(s));
            return intArray.ToList();
        }

        public static bool GetBool(this IList<string> list, int index, bool defaultValue = false)
        {
            var v = list.GetString(index);
            bool result;

            if (!string.IsNullOrEmpty(v))
            {
                if (bool.TryParse(v, out result))
                    return result;

                int intResult;
                if (int.TryParse(v, out intResult))
                    return Convert.ToBoolean(intResult);
            }

            return defaultValue;
        }

        public static DateTime GetDateTime(this IList<string> list, int index)
        {
            var v = list.GetString(index);
            DateTime result;

            if (!string.IsNullOrEmpty(v))
            {
                if (DateTime.TryParse(v, out result))
                    return result;
            }

            return DateTime.MinValue;
        }

        public static T GetEnum<T>(this IList<string> list, int index)
        {
            var v_int = list.GetInt(index);
            var v_str = list.GetString(index);
            T result = default(T);

            if (!Enum.IsDefined(typeof(T), v_int))
                return result;

            try
            {
                if (!string.IsNullOrEmpty(v_str))
                    result = (T)(Enum.Parse(typeof(T), v_str, true));
            }
            catch
            {
                result = default(T);
            }

            return result;
        }

        public static int GetInt(this IList<string> list, int index, int defaultValue = 0)
        {
            var v = list.GetString(index);
            int result;

            if (!string.IsNullOrEmpty(v))
            {
                if (int.TryParse(v, out result))
                    return result;
            }

            return defaultValue;
        }

        public static float GetFloat(this IList<string> list, int index, float defaultValue = 0)
        {
            var v = list.GetString(index);
            float result;

            if (!string.IsNullOrEmpty(v))
            {
                if (float.TryParse(v, out result))
                    return result;
            }

            return defaultValue;
        }

        public static double GetDouble(this IList<string> list, int index, double defaultValue = 0)
        {
            var v = list.GetString(index);
            double result;

            if (!string.IsNullOrEmpty(v))
            {
                if (double.TryParse(v, out result))
                    return result;
            }

            return defaultValue;
        }

        public static long GetLong(this IList<string> list, int index, long defaultValue = 0)
        {
            var v = list.GetString(index);
            long result;

            if (!string.IsNullOrEmpty(v))
            {
                if (long.TryParse(v, out result))
                    return result;
            }

            return defaultValue;
        }

        public static Guid GetGuid(this IList<string> list, int index)
        {
            var v = list.GetString(index).Trim();
            //Guid result;

            if (!string.IsNullOrEmpty(v))
            {
                try
                {
                    return new Guid(v);
                }
                catch { }
            }

            return Guid.Empty;
        }

        public static string GetString(this IList<string> list, int index, string defaultValue = "")
        {
            if (index >= 0 && list.Count > index)
            {
                if (!string.IsNullOrEmpty(list[index]))
                    return list[index];
                else
                    return defaultValue;
            }

            return defaultValue;
        }

        public static List<int> GetListInt(this IList<string> list, int index, string splittor = ";")
        {
            var li = new List<int>();

            if (list.Count <= index)
                return li;

            var str = list[index];
            var arr = str.SplitTrim(splittor);

            foreach (var s in arr)
            {
                var value = 0;
                if (int.TryParse(s, out value))
                    li.Add(value);
            }

            return li;
        }

        public static List<Guid> GetListGuid(this IList<string> list, int index, string splittor = ";")
        {
            var li = new List<Guid>();

            if (list.Count <= index)
                return li;

            var str = list[index];
            var arr = str.SplitTrim(splittor);

            foreach (var s in arr)
                li.Add(new Guid(s));

            return li;
        }

        public static List<string> GetListString(this IList<string> list, int index, string splittor = ";")
        {
            var li = new List<string>();

            if (list.Count <= index)
                return li;

            var str = list[index];
            var arr = str.SplitTrim(splittor);

            li.AddRange(arr);

            return li;
        }

        public static T GetData<T>(this IList<string> list, int index)
        {
            var v = list.GetString(index);
            var result = default(T);

            if (!string.IsNullOrEmpty(v))
            {
                var data = Encoding.UTF8.GetBytes(v);
                result = SerializationHelper.Deserialize<T>(data);
            }

            return result;
        }

        #endregion

        #region To

        public static int ToInt(this object obj)
        {
            if (obj == null || obj == System.DBNull.Value)
                return 0;

            string str = obj.ToString();

            int result;
            if (int.TryParse(str, out result))
                return result;

            decimal resultD;
            if (decimal.TryParse(str, out resultD))
                return (int)Math.Floor(resultD); // truncate decimal part as desired

            bool resultB;
            if (bool.TryParse(str, out resultB))
                return Convert.ToInt32(resultB);

            return 0;
        }

        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == System.DBNull.Value)
                return 0;

            double result;
            if (double.TryParse(obj.ToString(), out result))
                return result;

            bool resultB;
            if (bool.TryParse(obj.ToString(), out resultB))
                return Convert.ToDouble(resultB);

            return 0d;
        }

        public static bool ToBool(this object obj)
        {
            if (obj == null)
                return false;

            return Convert.ToBoolean(obj);
        }

        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
                return DateTime.MinValue;

            if (string.IsNullOrEmpty(obj.ToString()))
                return DateTime.MinValue;

            return Convert.ToDateTime(obj);
        }

        public static DateTime FromDateTimeDisplay(this string dateStr)
        {
            return string.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// 将顺序打乱
        /// </summary>
        public static List<T> Shuffle<T>(this IEnumerable<T> list, Random ran = null)
        {
            return list.Shuffle(-1, ran);
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> list, int limitNumber, Random ran = null)
        {
            var newList = new List<T>();
            var count = list.Count();

            if (count <= 0)
                return list.ToList();

            var _list = new List<T>();
            foreach (var i in list)
                _list.Add(i);

            int cbRandCount = 0;
            int cbPosition = 0;

            do
            {
                int r = count - cbRandCount;
                cbPosition = MathHelper.GetRandom(0, r - 1, ran);

                newList.Add(_list[cbPosition]);
                cbRandCount++;
                _list[cbPosition] = _list[r - 1];
            } while (cbRandCount < count);

            if (limitNumber < 0)
                return newList;
            else
                return newList.Take(limitNumber).ToList();
        }

        public static List<T> RandomShuffle<T>(this IEnumerable<T> list, int minNumber, int maxNumber, double changedRate = 0, Random ran = null, bool isRestrictNumber = false, bool checkRepeat = true, bool addWhenRepeat = false)
            where T : IRandom, new()
        {
            if (minNumber == 0 || maxNumber == 99999)
            {
                #region 旧算法

                var mustDropList = new List<T>();
                var dropList = new List<T>();
                var dropNumber = 0;

                // 打乱顺序
                list = list.Shuffle(ran);

                foreach (var drop in list)
                {
                    // 如果不需要验证几率，则直接添加..
                    if (drop.Chance == 10000)
                    {
                        mustDropList.Add(drop);
                        continue;
                    }

                    // 如果不掉落，则直接Pass
                    if (drop.Chance == 0)
                        continue;

                    // 如果需要验证几率..
                    // - 使用Mod让changedRate后难掉落的东西变简单
                    //var dropRate = Convert.ToInt32((drop.Chance * (1.0 + changedRate)) % 10000);
                    var dropRate = drop.Chance;
                    var chance = MathHelper.GetRandom(1, 10000, ran);

                    //drop.DiceValue = (drop.Chance - chance);    // 负数为相差远，正数为相差近

                    // 添加至掉落列表
                    if (chance <= dropRate)
                    {
                        // 掉落物添加至列表前端
                        dropList.Insert(0, drop);
                        dropNumber++;
                    }
                    else
                    {
                        // 非掉落物添加至列表末端
                        dropList.Add(drop);
                    }
                }

                if (isRestrictNumber)
                    dropNumber = dropNumber.GetBetween(minNumber - mustDropList.Count, MathHelper.GetRandom(minNumber, maxNumber, ran) - mustDropList.Count);
                else
                    dropNumber = dropNumber.GetBetween(minNumber, MathHelper.GetRandom(minNumber, maxNumber, ran));

                var finalList = new List<T>();
                finalList.AddRange(mustDropList.Shuffle(ran));
                finalList.AddRange(dropList.Take(dropNumber));

                return finalList;

                #endregion
            }
            else
            {
                #region 新算法

                var mustDropList = new List<T>();
                var dropList = new List<T>();
                var randomList = new List<RandomDice<T>>();
                var pointer = 0;

                // 打乱顺序
                list = list.Shuffle(ran);

                foreach (var drop in list)
                {
                    // 如果不需要验证几率，则直接添加至必出组
                    if (drop.Chance == 10000)
                    {
                        mustDropList.Add(drop);
                        continue;
                    }

                    if (drop.Chance == 0)
                        continue;

                    // 添加至随机组
                    var dice = new RandomDice<T>();
                    dice.MinRange = pointer + 1;
                    dice.MaxRange = dice.MinRange + Convert.ToInt32((double)drop.Chance * (1 + changedRate));
                    dice.Data = drop;

                    randomList.Add(dice);
                    pointer = dice.MaxRange;
                }

                dropList.AddRange(mustDropList.Shuffle(ran));

                var minDropNumber = Math.Max(0, minNumber);
                var maxDropNumber = Math.Min(maxNumber, list.Count(d => d.Chance.Between(1, 10000)));
                var dropNumber = MathHelper.GetRandom(minDropNumber, maxDropNumber, ran); // 从min, max中随机一个number, 掉落number个物品

                // 如果需要把必掉的物品计算在总量中
                if (isRestrictNumber)
                    dropNumber -= mustDropList.Count;

                if (randomList.Count > 0)
                {
                    var missList = new List<int>();
                    for (var i = 0; i < dropNumber; i++)
                    {
                        var chance = MathHelper.GetRandom(1, pointer, ran);
                        var drop = randomList.Find(r => r.MinRange <= chance && r.MaxRange >= chance);

                        // checkRepeat - 已经添加了是否不再添加
                        if (checkRepeat)
                        {
                            if (!dropList.Contains(drop.Data))
                                dropList.Add(drop.Data);
                            else
                            {
                                if (addWhenRepeat)
                                    i--;
                                else
                                    missList.Add(chance);
                            }
                        }
                        else
                        {
                            dropList.Add(drop.Data);
                        }
                    }

                    if (missList.Count > 0)
                    {
                        foreach (var chance in missList)
                        {
                            var drop = randomList.Find(r => !dropList.Contains(r.Data) && (r.MaxRange >= chance || r.MinRange <= chance));
                            if (drop != null)
                                dropList.Add(drop.Data);
                        }
                    }
                }

                return dropList;

                #endregion
            }
        }

        public static List<T> RandomShuffle<T>(this List<RandomDice<T>> diceList, int minNumber, int maxNumber)
            where T : IRandom, new()
        {
            var dropList = new List<T>();
            var pointer = diceList.Last().MaxRange;

            var minDropNumber = Math.Max(0, minNumber);
            var dropNumber = MathHelper.GetRandom(minDropNumber, maxNumber); // 从min, max中随机一个number, 掉落number个物品

            // 如果需要把必掉的物品计算在总量中
            for (var i = 0; i < dropNumber; i++)
            {
                var chance = MathHelper.GetRandom(1, pointer, null);
                var drop = diceList.Find(r => r.MinRange <= chance && r.MaxRange >= chance);

                if (!dropList.Contains(drop.Data))
                    dropList.Add(drop.Data);
            }

            return dropList;
        }

        /// <summary>
        /// 从一个列表中随机x个不重复的项目
        /// </summary>
        public static List<T> RandomShuffleDistinct<T>(this List<T> list, int minNumber, int maxNumber, Random ran = null) where T : IRandom, new()
        {
            var dropList = new List<T>();
            var dropNumber = MathHelper.GetRandom(minNumber, maxNumber, ran);

            // 打乱顺序
            list = list.Shuffle(ran);

            // 必掉组
            var mustDropList = list.Where(d => d.Chance == 10000).ToList();
            // 随机掉组
            var randomDropList = list.Where(d => d.Chance.Between(1, 10000)).ToList();

            for (var i = 0; i < dropNumber; i++)
            {
                // 如果mustDropList里有相应物品, 则直接添加
                if (i < mustDropList.Count)
                {
                    dropList.Add(mustDropList[i]);
                }
                else
                {
                    // 从random里面随一个物品
                    var randomList = new List<RandomDice<T>>();
                    var pointer = 0;

                    foreach (var randomDrop in randomDropList)
                    {
                        var dice = new RandomDice<T>();
                        dice.MinRange = pointer + 1;
                        dice.MaxRange = dice.MinRange + randomDrop.Chance;
                        dice.Data = randomDrop;

                        randomList.Add(dice);
                        pointer = dice.MaxRange;
                    }

                    if (randomList.Count > 0)
                    {
                        var chance = MathHelper.GetRandom(1, pointer, ran);
                        var drop = randomList.Find(r => r.MinRange <= chance && r.MaxRange >= chance);

                        dropList.Add(drop.Data);

                        randomDropList.Remove(drop.Data);
                    }
                }
            }

            //if (randomList.Count > 0)
            //{
            //    for (var i = 0; i < dropNumber; i++)
            //    {
            //        var chance = MathHelper.GetRandom(1, pointer, ran);
            //        var drop = randomList.Find(r => r.MinRange <= chance && r.MaxRange >= chance);
            //        if (!checkRepeat || !dropList.Contains(drop.Data)) // checkRepeat - 已经添加了是否不再添加
            //            dropList.Add(drop.Data);
            //    }
            //}

            return dropList;
        }

        #endregion

        public static void Record<T>(this IList<T> list, T item, int maxCapacity)
        {
            if (list.Count >= maxCapacity && maxCapacity > 0)
                list.RemoveAt(0);

            list.Add(item);
        }

        #region RoundRobin

        /// <summary>
        /// 轮流选择
        /// </summary>
        public static T RoundRobin<T>(this List<T> list, Predicate<T> currentMatch, Predicate<T> match)
        {
            var startIndex = list.FindIndex(currentMatch) + 1;

            if (startIndex < list.Count)
            {
                var result = list.Find(match);

                for (var i = startIndex; i < list.Count; i++)
                {
                    if (match(list[i]))
                        return list[i];
                }
            }

            return list.Find(match);
        }

        #endregion

        #region Loop Get

        /// <summary>
        /// 循环读取
        /// </summary>
        public static List<T> LoopGet<T>(this List<T> list, int startIndex, int endIndex)
        {
            var resultList = new List<T>();

            // 如果已经循环过了，则先从startIndex取到尾，再从头取到endIndex；否则直接从startIndex取到endIndex
            if (startIndex > endIndex)
            {
                for (var i = startIndex; i < list.Capacity; i++)
                {
                    resultList.Add(list[i]);
                }

                // 列表重头开始..
                startIndex = 0;
            }

            for (var i = startIndex; i <= endIndex; i++)
            {
                resultList.Add(list[i]);
            }

            return resultList;
        }

        #endregion

        #region 重新排序

        public static IOrderedEnumerable<T> FilterBy<T>(this IOrderedEnumerable<T> list, Func<T, int> func, bool ignoreExistOrder)
        {
            return (!ignoreExistOrder) ? list.ThenBy(func) : list.OrderBy(func);
        }

        public static IOrderedEnumerable<T> FilterByDescending<T>(this IOrderedEnumerable<T> list, Func<T, int> func, bool ignoreExistOrder)
        {
            return (!ignoreExistOrder) ? list.ThenByDescending(func) : list.OrderByDescending(func);
        }

        #endregion

        #region Random

        public static T Random<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).Random();
        }

        public static T Random<T>(this IEnumerable<T> list, Random ran = null)
        {
            if (list.Count() == 0)
                return default(T);

            if (list.Count() == 1)
                return list.ElementAt(0);

            var random = MathHelper.GetRandom(0, list.Count() - 1, ran);
            return list.ElementAt(random);
        }

        // public static T RandomSource<T>(this IEnumerable<T> list) where T : ISource, new()
        // {
        //     var random = MathHelper.GetRandom(0, list.Count() - 1);
        //     var sourceID = list.ElementAt(random).SourceID;
        //
        //     return SourceCache<T>.GetSource(sourceID);
        // }

        /// <summary>
        /// 列表中的几率总和少于10000
        /// 例如: A: 3000, B: 2000 则有50%的概率随机出null
        /// extraChance: 用于按照比率提升总体爆率, 例如原先爆率为1000|2000|3000的掉落组, 提升600的爆率后, 实际为1100|2200|3300
        /// </summary>
        public static T RandomOrDefault<T>(this List<T> list, Random ran = null, int extraChance = 0) where T : IRandom
        {
            var randomList = new List<RandomDice<T>>();
            var pointer = 0;
            var dropList = list.Where(d => d.Chance != 0).ToList();
            var sumChance = dropList.Sum(d => d.Chance); // 总爆率

            foreach (var drop in dropList)
            {
                var extra = 0;
                if (extraChance > 0)
                {
                    // 按照比率提升实际掉率
                    extra = Convert.ToInt32(extraChance * 1d * drop.Chance / sumChance);
                }

                // 添加至随机组
                var dice = new RandomDice<T>();
                dice.MinRange = pointer + 1;
                dice.MaxRange = dice.MinRange + drop.Chance + extra;
                dice.Data = drop;

                randomList.Add(dice);
                pointer = dice.MaxRange;
            }

            var chance = MathHelper.GetRandom(1, 10000, ran);
            var result = randomList.Find(r => r.MinRange <= chance && r.MaxRange >= chance);

            return result == null ? default(T) : result.Data;
        }

        #endregion

        #region Dequeue/UnStack

        public static T Dequeue<T>(this IList<T> list)
        {
            if (list.Count <= 0)
                return default(T);

            var data = list[0];
            list.RemoveAt(0);

            return data;
        }

        public static T UnStack<T>(this IList<T> list)
        {
            if (list.Count <= 0)
                return default(T);

            var data = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);

            return data;
        }


        #endregion

        /// <summary>
        /// 获取页面数据
        /// </summary>
        /// <param name="page">第几页(从1开始)</param>
        /// <param name="count">每页几个</param>
        public static List<T> GetPageList<T>(this List<T> list, int page, int count)
        {
            if (list.Count == 0)
                return new List<T>();

            var startIndex = (page - 1) * count;
            if (startIndex >= list.Count)
                return new List<T>();

            var takeNum = Math.Min(count, list.Count - startIndex);
            return list.GetRange(startIndex, takeNum);
        }

        /// <summary>
        /// 倒序查找
        /// </summary>
        public static List<T> TakeLast<T>(this List<T> list, int num)
        {
            if (list.Count == 0)
                return list;

            var startIndex = list.Count - num;
            var delta = 0 - startIndex;
            if (delta > 0)
                return list.GetRange(0, num - delta);
            else
                return list.GetRange(startIndex, num);
        }

        public static List<List<T>> Splice<T>(this List<T> list, int num)
        {
            var result = new List<List<T>>();
            var len = list.Count;

            if (len <= num)
                return new List<List<T>> { list };

            int i = 0;
            while (i < len)
            {
                var takeNum = (i + num > len) ? len - i : num;
                result.Add(list.GetRange(i, takeNum));
                i += num;
            }

            return result;
        }
    }

    public interface IRandom
    {
        int Chance
        {
            get;
            set;
        }
    }

    public class RandomDice<T>
        where T : IRandom
    {
        public int MinRange
        {
            get;
            set;
        }

        public int MaxRange
        {
            get;
            set;
        }

        public T Data
        {
            get;
            set;
        }
    }
}