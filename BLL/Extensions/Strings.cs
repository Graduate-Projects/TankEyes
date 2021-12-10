using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
    public static class Strings
    {
        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(newVal, temp);
        }
    }
}
