using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeBook
{
    /// <summary>
    /// A collection of potentially useful utilities.
    /// </summary>
    static class Util
    {
        /// <summary>
        /// Converts the first letter of a string to upper case.
        /// </summary>
        /// <param name="text">The string of which to make the first letter uppcase.</param>
        /// <returns>A string with an uppercase first letter.</returns>
        public static string FirstToUpperCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            char[] charArray = text.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);
            return new String(charArray);
        }
        /// <summary>
        /// Attempts to make a url valid.
        /// </summary>
        /// <param name="url">the absolute url to validify</param>
        /// <returns>The validated Uri</returns>
        public static Uri ToValidUri(string url)
        {
            return new UriBuilder(url).Uri;
        }
        /// <summary>
        /// Strips everything but the name and top level domain of a url.
        /// </summary>
        /// <param name="url">A Uri object to parse into a simplified url.</param>
        /// <returns>The simplified url.</returns>
        public static string ToSimpleUrl(Uri url)
        {
            return url.Host;
        }
        /// <summary>
        /// Strips everything but the name and top level domain of a url.
        /// </summary>
        /// <param name="url">A string representing a url to parse into a simplified url.</param>
        /// <returns>The simplified url.</returns>
        public static string ToSimpleUrl(String url)
        {
            var uri = new Uri(url);
            return uri.Host;
        }
    }
}
