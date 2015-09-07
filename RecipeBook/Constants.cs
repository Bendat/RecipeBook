using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeBook
{
    public static class Constants
    {
        /// <summary>
        /// The folder images are located in relative to the programs root directory.
        /// </summary>
        public const string ImageFolder = "/images/userimages/";
        /// <summary>
        /// The help screen that is presented on program start.
        /// </summary>
        public const string WelcomeFile = "WelcomeScreen/welcome.rtf";
        /// <summary>
        /// The default image recipes fall back to if none is provided.
        /// </summary>
        public const string Cookbook = ImageFolder + "cookbookicon2.ico";
    }
}
