using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static readonly ThemeReader ThemeReader = new ThemeReader();
        /// <summary>
        /// The SetTheme method will override the default theme and use the
        /// active theme from theme.xml.
        /// </summary>
        public static void SetTheme()
        {
            if (ThemeReader != null)
            {
                BackgroundColor = ThemeReader.BackgroundColor;
                UiTextColor = ThemeReader.UiTextColor;
                HeaderColor = ThemeReader.HeaderColor;
                SelectionColor = ThemeReader.SelectionColor;
                TextHoverColor = ThemeReader.TextHoverColor;
                MainTextColor = ThemeReader.MainTextColor;
            }
        }
    }
}
