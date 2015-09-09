using System;
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
            string filename;
            Console.WriteLine(Directory.Exists(Path.GetDirectoryName(location)));
            if (Directory.Exists(Path.GetDirectoryName(location)))
            {
               filename = Path.GetFileName(location);
            }
            else
            {
                return Constants.Cookbook;
            }
             
            try {
                if (Util.IsImageTooLarge(location, 500))
                {
                    Image img = Util.MakeThumbnail(location);
                    img.Save("images/userimages/" + filename);
                    Console.WriteLine("7.1");
                }
                else
                {
                    File.Copy(location, "images/userimages/" + filename);
                }
            }catch(Exception e)
            {
                filename = Constants.Cookbook;
            }
            return filename;
        }

        
    }
}
