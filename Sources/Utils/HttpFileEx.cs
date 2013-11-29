using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSW.Utils
{
    public static class HttpFileEx
    {
        /// <summary>
        /// UTF-8 문자열이 잘못 변환된 것을 올바르게 변환해 준다.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetUtf8FixString(this HttpFile self, string text)
        {
            var bText = new byte[text.Length];
            for (int i = 0; i < bText.Length; i++)
                bText[i] = (byte)text[i];
            var result = Encoding.UTF8.GetString(bText);
            return result;
        }
    }
}
