using System.Drawing;
using System.IO;

namespace RecipeBook
{
    static class FileHandler
    {
        /// <summary>
        /// Copies an image specified in the Editor window to the apps image folder.
        /// Reduces the dimensions if the image is greater than 500px wide.
        /// </summary>
        /// <param name="location">The path to the image file.</param>
        /// <returns></returns>
        public static string AddImageToApp(string location)
        {
            string filename = Path.GetFileName(location);
            if (Util.IsImageTooLarge(location, 500))
            {
                Image img = Util.MakeThumbnail(location);
                img.Save("images/userimages/" + filename);
            }
            else
            {
                File.Copy(location, "images/userimages/" + filename);
            }
            return filename;
        }

        
    }
}
