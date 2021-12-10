using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
    public static class Tags
    {
        public static string Validation(this string s)
        {
            return s.Replace(new char[] { '_', '@', '-', '.', ':', '#', ' ' }, "").ToLower();
        }
    }
}
