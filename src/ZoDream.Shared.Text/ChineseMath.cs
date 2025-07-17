using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Text
{
    public static class ChineseMath
    {
        private static readonly string[] ChineseNumbers = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"];
        private static readonly string[] ChineseUnits = ["", "十", "百", "千"];
        private static readonly string[] ChineseBigUnits = ["", "万", "亿", "兆"];

        private static readonly Dictionary<char, long> ChineseNumberMap = new()
        {
            {'零', 0}, {'一', 1}, {'二', 2}, {'三', 3}, {'四', 4},
            {'五', 5}, {'六', 6}, {'七', 7}, {'八', 8}, {'九', 9},
            {'十', 10}, {'百', 100}, {'千', 1000}, {'万', 10000},
            {'亿', 100000000}, {'兆', 1000000000000}
        };

        private static readonly Dictionary<char, long> ChineseBigUnitMap = new()
        {
            {'万', 10000}, {'亿', 100000000}, {'兆', 1000000000000}
        };

        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Format(int val)
        {
            if (val == 0)
            {
                return ChineseNumbers[0];
            }

            bool isNegative = false;
            if (val < 0)
            {
                isNegative = true;
                val = Math.Abs(val);
            }

            var result = new StringBuilder();
            var unitPos = 0;
            var needZero = false;

            while (val > 0)
            {
                var section = val % 10000;
                if (needZero)
                {
                    result.Insert(0, ChineseNumbers[0]);
                }

                var sectionChinese = ConvertSection(section);
                if (section != 0)
                {
                    sectionChinese += ChineseBigUnits[unitPos];
                }

                result.Insert(0, sectionChinese);

                needZero = (section < 1000 && section > 0 && val > 1000);
                val /= 10000;
                unitPos++;
            }

            // 处理"一十"开头的特殊情况，通常说"十"
            if (result.Length > 1 && result[0] == '一' && result[1] == '十')
            {
                result.Remove(0, 1);
            }

            if (isNegative)
            {
                result.Insert(0, "负");
            }
            return result.ToString();
        }

        private static string ConvertSection(int section)
        {
            var sectionStr = new StringBuilder();
            var unitPos = 0;
            var zero = true;

            while (section > 0)
            {
                var digit = section % 10;
                if (digit == 0)
                {
                    if (!zero)
                    {
                        zero = true;
                        sectionStr.Insert(0, ChineseNumbers[digit]);
                    }
                }
                else
                {
                    zero = false;
                    sectionStr.Insert(0, ChineseNumbers[digit] + ChineseUnits[unitPos]);
                }

                section /= 10;
                unitPos++;
            }

            return sectionStr.ToString();
        }

        /// <summary>
        /// 中文转数字
        /// </summary>
        /// <param name="val"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(ReadOnlySpan<char> val, out long result)
        {
            result = 0;
            if (val.IsEmpty)
            {
                return true;
            }
            if (val[0] is '-' or >= '0' and <= '9' )
            {
                return long.TryParse(val, out result);
            }
            var isNegative = false;
            var index = 0;
            if (val[0] == '负')
            {
                isNegative = true;
                index = 1;
            }
            var currentSection = 0L;
            var currentNumber = 0L;

            for (; index < val.Length; index++)
            {
                var c = val[index];

                if (ChineseNumberMap.TryGetValue(c, out long value))
                {
                    if (value < 10) // 数字
                    {
                        currentNumber = value;
                    }
                    else if (value < 10000) // 单位：十、百、千
                    {
                        if (currentNumber == 0)
                        {
                            currentNumber = 1;
                        }
                        currentSection += currentNumber * value;
                        currentNumber = 0;
                    }
                    else // 大单位：万、亿、兆
                    {
                        if (currentNumber != 0)
                        {
                            currentSection += currentNumber;
                        }

                        currentSection *= value;
                        result += currentSection;
                        currentSection = 0;
                        currentNumber = 0;
                    }
                }
                else
                {
                    return false;
                }
            }

            result += currentSection + currentNumber;
            result = isNegative ? -result : result;
            return true;
        }
    }
}
