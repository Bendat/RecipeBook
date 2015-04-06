using System.Windows.Media;
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
    /// A class that describes a theme.
    /// </summary>
    static class Theme
    {
        public static Brush BackgroundColor = Brushes.DarkGoldenrod;
        public static Brush UiTextColor = Brushes.Cornsilk;
        public static Brush HeaderColor = Brushes.DarkGoldenrod;
        public static Brush SelectionColor = Brushes.Goldenrod;
        public static Brush TextHoverColor = Brushes.Goldenrod;
        public static Brush MainTextColor = Brushes.Cornsilk;
        private static ThemeReader _themeReader; 

        /// <summary>
        /// The UseTheme method will override the default theme and use the
        /// active theme from theme.xml.
        /// </summary>
        /// <param name="doUseTheme">Tells the class if it should use the theme file.</param>
        
        //doUseTheme is an ugly fix for a path error caused by binding this class to xaml controls.
        public static void UseTheme(bool doUseTheme)
        {
            if (doUseTheme)
            {
                _themeReader = new ThemeReader();
                BackgroundColor = _themeReader.BackgroundColor;
                UiTextColor = _themeReader.UiTextColor;
                HeaderColor = _themeReader.HeaderColor;
                SelectionColor = _themeReader.SelectionColor;
                TextHoverColor = _themeReader.TextHoverColor;
                MainTextColor = _themeReader.MainTextColor;
            }
        }
    }
}
