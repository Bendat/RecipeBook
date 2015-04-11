using System;
using System.Drawing;

/*  Copyright (C) 2015 Ben Aherne

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
namespace RecipeBook
{
    /// <summary>
    /// A collection of potentially useful methods.
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

        public static Image MakeThumbnail(string file)
        {
            Image image = Image.FromFile(file);
            int width = 500;
            double ratio = (double)image.Height/(double)image.Width;
            int height = (int)(500*ratio);
            Console.WriteLine(ratio);
            Image thumb = image.GetThumbnailImage(width,height,()=>false, IntPtr.Zero);
            image.Dispose();
            return thumb;
        }
        /// <summary>
        /// Checks if an image file is larger than the permitted size.
        /// </summary>
        /// <param name="img">The Image object to check.</param>
        /// <param name="maxWidth">The size the image should be no wider than.</param>
        /// <returns>Boolean true if the image is too big, false otherwise</returns>
        public static bool IsImageTooLarge(Image img, int maxWidth)
        {
            return img.Width > maxWidth;
        }
        /// <summary>
        /// Checks if an image file is larger than the permitted size.
        /// </summary>
        /// <param name="location">The location of the image file to check.</param>
        /// <param name="maxWidth">The size the image should be no wider than.</param>
        /// <returns>Boolean true if the image is too big, false otherwise</returns>
        public static bool IsImageTooLarge(string location, int maxWidth)
        {
            Image img = new Bitmap(location);
            return img.Width > maxWidth;
        }
    }
}
