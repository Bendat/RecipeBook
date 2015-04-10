using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeBook
{
    static class Util
    {
        private static string FirstToUpperCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            char[] charArray = text.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);
            return new String(charArray);
        }
    }
}
