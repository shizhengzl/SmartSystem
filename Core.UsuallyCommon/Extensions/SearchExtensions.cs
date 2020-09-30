using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Core.UsuallyCommon
{
    /// <summary>
    /// 字符串查询
    /// </summary>
    public static class SearchExtensions
    {
        /// <summary>
        /// 查找匹配字符串
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="toCheck">查找字符串</param>
        /// <param name="comparison">OrdinalIgnoreCase 不区分大小写</param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return source.IndexOf(toCheck, comparison) >= 0;
        }

        /// <summary>
        /// 根据类容字符串自动切割
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<String> GetStringSingleColumn(string context)
        {
            string[] separatingChars = new string[] { "\r\n", "\n", "\r", "\t", " " };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();
        }

        /// <summary>
        /// 查询列表近似值
        /// </summary>
        /// <param name="param"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Boolean SearchWordExists(this string param, string[] items)
        {
            return Search(param, items).Length > 0;
        }

        /// <summary>
        /// 根据字符串返回查询列表匹配的字符
        /// </summary>
        /// <param name="param">查询的字符串</param>
        /// <param name="items">查询的列表</param>
        /// <returns></returns>
        public static string[] Search(string param, string[] items)
        {
            if (string.IsNullOrWhiteSpace(param) || items == null || items.Length == 0)
                return new string[0];

            string[] words = param
                                .Split(new char[] { ' ', '\u3000' }, StringSplitOptions.RemoveEmptyEntries)
                                .OrderBy(s => s.Length)
                                .ToArray();

            var q = from sentence in items.AsParallel()
                    let MLL = Mul_LnCS_Length(sentence, words)
                    where MLL >= 0
                    orderby (MLL + 0.5) / sentence.Length, sentence
                    select sentence;

            return q.ToArray();
        }

        /// <summary>
        /// 字符模糊匹配
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="words">多个关键字。长度必须大于0，必须按照字符串长度升序排列。</param>
        /// <returns>int</returns>
        public static int Mul_LnCS_Length(string sentence, string[] words)
        {
            int sLength = sentence.Length;
            int result = sLength;
            bool[] flags = new bool[sLength];
            int[,] C = new int[sLength + 1, words[words.Length - 1].Length + 1];
            //int[,] C = new int[sLength + 1, words.Select(s => s.Length).Max() + 1];
            foreach (string word in words)
            {
                int wLength = word.Length;
                int first = 0, last = 0;
                int i = 0, j = 0, LCS_L;
                //foreach 速度会有所提升，还可以加剪枝
                for (i = 0; i < sLength; i++)
                    for (j = 0; j < wLength; j++)
                        if (sentence[i] == word[j])
                        {
                            C[i + 1, j + 1] = C[i, j] + 1;
                            if (first < C[i, j])
                            {
                                last = i;
                                first = C[i, j];
                            }
                        }
                        else
                            C[i + 1, j + 1] = Math.Max(C[i, j + 1], C[i + 1, j]);

                LCS_L = C[i, j];
                if (LCS_L <= wLength >> 1)
                    return -1;

                while (i > 0 && j > 0)
                {
                    if (C[i - 1, j - 1] + 1 == C[i, j])
                    {
                        i--;
                        j--;
                        if (!flags[i])
                        {
                            flags[i] = true;
                            result--;
                        }
                        first = i;
                    }
                    else if (C[i - 1, j] == C[i, j])
                        i--;
                    else// if (C[i, j - 1] == C[i, j])
                        j--;
                }

                if (LCS_L <= (last - first + 1) >> 1)
                    return -1;
            }

            return result;
        }
    }
}
