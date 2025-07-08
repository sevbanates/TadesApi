using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TadesApi.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsFilter(this string key, string srcKey)
        {
            int length = srcKey.Length - 1;
            return srcKey.Trim()[0] == '*' ? key.ToLower().Contains(srcKey.Remove(0, 1).ToLower()) : srcKey.Trim()[length] == '%' ? key.ToLower().StartsWith(srcKey.Remove(length).ToLower()) : key.ToLower().Contains(srcKey.ToLower());
        }
    }
}
