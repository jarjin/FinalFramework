using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utility
{
   public static class MathHelper
    {
        #region Random

        static Random GlobalRandom = new Random(GetRandomSeed());

        /// <summary>
        /// 获取随机数
        /// </summary>
        public static int GetRandom(int min, int max)
        {
            return GetRandom(min, max, GlobalRandom);
        }

        public static int GetRandom(this IRandomUser user, int min, int max)
        {
            if (user.Random == null)
                user.InitializeRandom();

            return GetRandom(min, max, user.Random);
        }

        public static int GetRandom(int min, int max, Random random)
        {
            if (random == null)
                random = GlobalRandom;

            if (max < min)
                max = min;

            int num = random.Next(min, max + 1);
            return num;
        }

        public static int GetRandom(int min, int max, RandomType randomType)
        {
            switch (randomType)
            {
                case RandomType.RandomDay:
                    return GetRandom(min, max, new Random(DateTime.Now.DayOfYear));
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获取随机等级
        /// - 按照两边较少，中间较多的分布
        /// </summary>
        public static int GetRandomLevel(int maxLevel, int midLevel, int minLevel, Random ran = null)
        {
            var maxValue = 10000;
            var randomValue = GetRandom(0, maxValue, (ran != null) ? ran : GlobalRandom);
            var level = 0.0;
            var minAverageValue = (maxValue / 2) / Math.Pow(midLevel, 2);
            var maxAverageValue = (maxValue / 2) / Math.Pow(maxLevel - midLevel, 2);

            if (randomValue < maxValue / 2)
                level = Math.Sqrt(randomValue / minAverageValue);
            else
                level = maxLevel - Math.Sqrt((maxValue - randomValue) / maxAverageValue);

            return Convert.ToInt32(level).GetBetween(minLevel, maxLevel);
        }

        public static int GetRandomSeed()
        {
            var guid = Guid.NewGuid();
            return GetRandomSeed(guid);
        }

        public static int GetRandomSeed(Guid guid)
        {
            var seed = BitConverter.ToInt32(guid.ToByteArray(), 0);
            return (seed != 0) ? seed : 1;
        }

        public static int GetEncryptRandom(int randomValue, long seed)
        {
            return (int)(((seed = seed * 201413L + 2531011L) >> 16) & 0x7fff) % randomValue;
        }

        #endregion

        #region HasChance

        private static int ChanceRate = 10000;

        public static bool HasChance(int rate)
        {
            return HasChance(rate, GlobalRandom);
        }

        public static bool HasChance(this IRandomUser user, int rate)
        {
            if (user.Random == null)
                user.InitializeRandom();

            return HasChance(rate, user.Random);
        }

        public static bool HasChance(int rate, Random random)
        {
            if (rate <= 0)
                return false;

            if (rate == ChanceRate)
                return true;

            var value = GetRandom(1, ChanceRate, random);
            return (value <= rate);
        }

        public static bool HasChance(int rate, Random random, ref int value)
        {
            if (rate <= 0)
                return false;

            if (rate == ChanceRate)
                return true;

            value = GetRandom(1, ChanceRate, random);
            return (value <= rate);
        }

        #endregion

        #region Divide

        /// <summary>
        /// 有余数则向下取整
        /// </summary>
        public static int DivideLess(double number, double divisor)
        {
            return Convert.ToInt32(DivideExact(number, divisor, true));
        }
        /// <summary>
        /// 有余数则向上取整
        /// </summary>
        public static int DivideMore(double number, double divisor)
        {
            return Convert.ToInt32(DivideExact(number, divisor, false));
        }

        private static double DivideExact(double number, double divisor, bool isExact)
        {
            if (divisor == 0)
                return number;

            double result = 0;

            if (number < divisor)
                return (isExact ? 0 : 1);

            if (number % divisor == 0)
            {
                result = number / divisor;
            }
            else
            {
                result = ((number - (number % divisor)) / divisor);

                if (!isExact)
                    result++;
            }

            return result;
        }

        public static double Divide(this double valueA, double valueB)
        {
            return (valueB != 0) ? valueA / valueB : valueA;
        }

        public static double Divide(this int valueA, double valueB)
        {
            return (valueB != 0) ? (double)valueA / valueB : valueA;
        }

        public static double Compare(this double valueA, double valueB)
        {
            var percentage = valueA.Divide(valueB);
            return (percentage > 0) ? percentage - 1.0 : 0;
        }

        #endregion

        /// <summary>
        /// 比较数字
        /// </summary>
        /// <remarks>格式：valueA op ValueB</remarks>
        public static bool CompareValues(double valueA, double valueB, Operator opt)
        {
            switch (opt)
            {
                case Operator.GreaterThan:
                    return (valueA > valueB);
                case Operator.LessThan:
                    return (valueA < valueB);
                case Operator.GreaterThanOrEqual:
                    return (valueA >= valueB);
                case Operator.LessThanOrEqual:
                    return (valueA <= valueB);
                default:
                    return (valueA == valueB);
            }
        }

        #region Simple Calculate

        /// <summary>
        /// 算术逆波兰表达式计算.
        /// </summary>
        public static string Calculate(string s)
        {
            string S = BuildingRPN(s);
            string tmp = "";
            System.Collections.Stack sk = new System.Collections.Stack();
            char c = ' ';
            System.Text.StringBuilder Operand = new System.Text.StringBuilder();
            double x, y;
            for (int i = 0; i < S.Length; i++)
            {
                c = S[i];
                if (char.IsDigit(c) || c == '.')
                {//数据值收集.
                    Operand.Append(c);
                }
                else if (c == ' ' && Operand.Length > 0)
                {
                    #region 运算数转换
                    try
                    {
                        tmp = Operand.ToString();
                        if (tmp.StartsWith("-"))//负数的转换一定要小心...它不被直接支持.
                        {//现在我的算法里这个分支可能永远不会被执行.
                            sk.Push(-((double)Convert.ToDouble(tmp.Substring(1, tmp.Length - 1))));
                        }
                        else
                        {
                            sk.Push(Convert.ToDouble(tmp));
                        }
                    }
                    catch
                    {
                        return "发现异常数据值.";
                    }
                    Operand = new System.Text.StringBuilder();
                    #endregion
                }
                else if (c == '+'//运算符处理.双目运算处理.
                 || c == '-'
                 || c == '*'
                 || c == '/'
                 || c == '%'
                 || c == '^')
                {
                    #region 双目运算
                    if (sk.Count > 0)/*如果输入的表达式根本没有包含运算符.或是根本就是空串.这里的逻辑就有意义了.*/
                    {
                        y = (double)sk.Pop();
                    }
                    else
                    {
                        sk.Push(0);
                        break;
                    }
                    if (sk.Count > 0)
                        x = (double)sk.Pop();
                    else
                    {
                        sk.Push(y);
                        break;
                    }
                    switch (c)
                    {
                        case '+':
                            sk.Push(x + y);
                            break;
                        case '-':
                            sk.Push(x - y);
                            break;
                        case '*':
                            sk.Push(x * y);
                            break;
                        case '/':
                            sk.Push(x.Divide(y));
                            break;
                        case '%':
                            sk.Push(x % y);
                            break;
                        case '^':
                            //       if(x>0)
                            //       {我原本还想,如果被计算的数是负数,又要开真分数次方时如何处理的问题.后来我想还是算了吧.
                            sk.Push(System.Math.Pow(x, y));
                            //       }
                            //       else
                            //       {
                            //        double t=y;
                            //        string ts="";
                            //        t=1/(2*t);
                            //        ts=t.ToString();
                            //        if(ts.ToUpper().LastIndexOf('E')>0)
                            //        {
                            //         ;
                            //        }
                            //       }
                            break;
                    }
                    #endregion
                }
                else if (c == '!')//单目取反.)
                {
                    sk.Push(-((double)sk.Pop()));
                }
            }

            if (sk.Count > 1)
                return "运算没有完成.";

            if (sk.Count == 0)
                return "结果丢失..";

            return sk.Pop().ToString();
        }

        /// <summary>
        /// 算术逆波兰表达式.生成.
        /// </summary>
        private static string BuildingRPN(string s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(s);
            System.Collections.Stack sk = new System.Collections.Stack();
            System.Text.StringBuilder re = new System.Text.StringBuilder();
            char c = ' ';
            //sb.Replace(" ","");//一开始,我只去掉了空格.后来我不想不支持函数和常量能滤掉的全OUT掉.
            for (int i = 0; i < sb.Length; i++)
            {
                c = sb[i];
                if (char.IsDigit(c))//数字当然要了.
                    re.Append(c);
                //if(char.IsWhiteSpace(c)||char.IsLetter(c))//如果是空白,那么不要.现在字母也不要.
                //continue;
                switch (c)//如果是其它字符...列出的要,没有列出的不要.
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '^':
                    case '!':
                    case '(':
                    case ')':
                    case '.':
                        re.Append(c);
                        break;
                    default:
                        continue;
                }
            }

            sb = new StringBuilder(re.ToString());

            #region 对负号进行预转义处理.负号变单目运算符求反.

            for (int i = 0; i < sb.Length - 1; i++)
                if (sb[i] == '-' && (i == 0 || sb[i - 1] == '('))
                    sb[i] = '!';//字符转义.

            #endregion

            #region 将中缀表达式变为后缀表达式.
            re = new System.Text.StringBuilder();
            for (int i = 0; i < sb.Length; i++)
            {
                if (char.IsDigit(sb[i]) || sb[i] == '.')//如果是数值.
                {
                    re.Append(sb[i]);//加入后缀式
                }
                else if (sb[i] == '+'
                 || sb[i] == '-'
                 || sb[i] == '*'
                 || sb[i] == '/'
                 || sb[i] == '%'
                 || sb[i] == '^'
                 || sb[i] == '!')//.
                {
                    #region 运算符处理
                    while (sk.Count > 0) //栈不为空时
                    {
                        c = (char)sk.Pop(); //将栈中的操作符弹出.
                        if (c == '(') //如果发现左括号.停.
                        {
                            sk.Push(c); //将弹出的左括号压回.因为还有右括号要和它匹配.
                            break; //中断.
                        }
                        else
                        {
                            if (Power(c) < Power(sb[i]))//如果优先级比上次的高,则压栈.
                            {
                                sk.Push(c);
                                break;
                            }
                            else
                            {
                                re.Append(' ');
                                re.Append(c);
                            }
                            //如果不是左括号,那么将操作符加入后缀式中.
                        }
                    }
                    sk.Push(sb[i]); //把新操作符入栈.
                    re.Append(' ');
                    #endregion
                }
                else if (sb[i] == '(')//基本优先级提升
                {
                    sk.Push('(');
                    re.Append(' ');
                }
                else if (sb[i] == ')')//基本优先级下调
                {
                    while (sk.Count > 0) //栈不为空时
                    {
                        c = (char)sk.Pop(); //pop Operator
                        if (c != '(')
                        {
                            re.Append(' ');
                            re.Append(c);//加入空格主要是为了防止不相干的数据相临产生解析错误.
                            re.Append(' ');
                        }
                        else
                            break;
                    }
                }
                else
                    re.Append(sb[i]);
            }
            while (sk.Count > 0)//这是最后一个弹栈啦.
            {
                re.Append(' ');
                re.Append(sk.Pop());
            }
            #endregion
            re.Append(' ');
            return FormatSpace(re.ToString());//在这里进行一次表达式格式化.这里就是后缀式了.
        }

        /// <summary>
        /// 优先级别测试函数.
        /// </summary>
        private static int Power(char opr)
        {
            switch (opr)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '%':
                case '^':
                case '!':
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 规范化逆波兰表达式.
        /// </summary>
        private static string FormatSpace(string s)
        {
            var ret = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (!(s.Length > i + 1 && s[i] == ' ' && s[i + 1] == ' '))
                    ret.Append(s[i]);
                else
                    ret.Append(s[i]);
            }

            return ret.ToString();//.Replace('!','-');
        }

        #endregion

        #region Change

        public static int Change(this int number, int changeValue)
        {
            return Change(number, changeValue, int.MinValue, int.MaxValue);
        }
        public static int Change(this int number, int changeValue, int minValue, int maxValue)
        {
            number += changeValue;

            if (number < minValue)
                number = minValue;

            if (number > maxValue)
                number = maxValue;

            return number;
        }

        #endregion

        #region Increament/Decreament (Thread Safe)

        /// <summary>
        /// 递增
        /// - 线程安全，但比++慢4倍
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Increament(this int number)
        {
            Interlocked.Increment(ref number);

            return number;
        }

        /// <summary>
        /// 递减
        /// - 线程安全，但比--慢4倍
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Decreament(this int number)
        {
            Interlocked.Decrement(ref number);

            return number;
        }

        #endregion

        public static int SafeToInt(this string number, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(number))
                return defaultValue;

            try
            {
                return Convert.ToInt32(number);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool Between(this int number, int min, int max)
        {
            return (number >= min && number <= max);
        }

        public static bool Between(this double number, double min, double max)
        {
            return (number >= min && number <= max);
        }

        public static bool Range(this int number, double min, double max)
        {
            return (number > min && number <= max);
        }

        public static bool Range(this double number, double min, double max)
        {
            return (number > min && number <= max);
        }

        public static int GetBetween(this int number, int min, int max)
        {
            number = Math.Max(number, min);
            number = Math.Min(number, max);

            return number;
        }

        public static double GetBetween(this double number, double min, double max)
        {
            number = Math.Max(number, min);
            number = Math.Min(number, max);

            return number;
        }

        public static long GetBetween(this long number, long min, long max)
        {
            number = Math.Max(number, min);
            number = Math.Min(number, max);

            return number;
        }

        /// <summary>
        /// 获取平均值
        /// 去掉一个最高，一个最低，再求平均值
        /// </summary>
        public static int GetAverageValue(this List<int> list)
        {
            var averageValue = list.Average();
            var newList = list.Where(v => v >= averageValue / 2);

            return (int)newList.Average();
        }

        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Pow(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2), 0.5);
        }

        /// <summary>
        /// 获取list的num个数的所有组合
        /// </summary>
        public static List<List<T>> GetSortList<T>(List<T> list, int num)
        {
            if (num > list.Count)
                return new List<List<T>>();

            var arrows = new List<int>();
            for (var i = 0; i < num; i++)
                arrows.Add(i);

            return _sort(list, arrows, new List<List<T>>());
        }

        private static List<List<T>> _sort<T>(List<T> list, List<int> arrows, List<List<T>> arr)
        {
            var rightArrow = arrows.UnStack();

            while (rightArrow < list.Count)
            {
                var newArr = new List<T>();
                for (var i = 0; i < arrows.Count; i++)
                {
                    var index = arrows[i];
                    newArr.Add(list[index]);
                }

                newArr.Add(list[rightArrow]);
                arr.Add(newArr);

                rightArrow++;
            }

            arrows.Add(rightArrow - 1);

            var newArrows = _getNextArrows(list.Count, arrows);
            if (newArrows == null)
                return arr;

            _sort(list, newArrows, arr);
            return arr;
        }

        private static List<int> _getNextArrows(int length, List<int> currentArrows)
        {
            var len = currentArrows.Count;
            var changedIndex = -1;

            for (var i = 0; i < len; i++)
            {
                var currentIndex = currentArrows[len - 1 - i];
                if (currentIndex + i >= length - 1)
                    continue;

                changedIndex = len - 1 - i;
                break;
            }

            if (changedIndex < 0)
                return null;

            var v = currentArrows[changedIndex] + 1;
            var a = new List<int>();
            for (var j = 0; j < len; j++)
            {
                var cIndex = currentArrows[j];
                if (j < changedIndex)
                    a.Add(cIndex);
                else if (j == changedIndex)
                    a.Add(v);
                else
                    a.Add(v + j - changedIndex);
            }

            return a;
        }
    }

    /// <summary>
    /// 初始化随机
    /// </summary>
    public interface IRandomUser
    {
        Random Random
        {
            get;
            set;
        }

        void InitializeRandom();
    }

    /// <summary>
    /// 随机类别
    /// </summary>
    public enum RandomType
    {
        None = 0,

        /// <summary>
        /// 365日随机 
        /// </summary>
        RandomDay = 1,
    }

    /// <summary>
    /// 运算符
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 1,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 2,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual = 3,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 4,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 5,
    }
}