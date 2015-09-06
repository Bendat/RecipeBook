using System;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
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
    /// Loads a theme from a theme.xml.
    /// Assigns properties depending on which theme is active.
    /// </summary>
    public class ThemeReader
    {
        private readonly XDocument _doc = XDocument.Load("xmlData/theme.xml");

        private readonly XElement _root;
        private readonly XElement _themeNode;
        private readonly string _activeTheme;
        public Brush BackgroundColor { get; private set; }
        public Brush UiTextColor { get; private set; }
        public Brush HeaderColor { get; private set; }
        public Brush SelectionColor { get; private set; }
        public Brush TextHoverColor { get; private set; }
        public Brush MainTextColor { get; private set; }
        public ThemeReader()
        {
            _root =_doc.Root;
            _activeTheme = GetTheme().Value;
            _themeNode = GetThemeNode();
            BackgroundColor = GetBackground();
            UiTextColor = GetUiText();
            HeaderColor = GetHeader();
            SelectionColor = GetSelection();
            TextHoverColor = GetTextHover();
            MainTextColor = GetMainText();
        }
        /// <summary>
        /// Changes the theme to use.
        /// </summary>
        /// <param name="newTheme">A string that corresponds to a theme in the theme.xml file</param>
        public void SetActive(string newTheme)
        {
            Console.WriteLine(_root.Attribute("active").Value);
            _root.Attribute("active")
                .SetValue(newTheme);
            _doc.Save("xmlData/theme.xml");
        }

        private XAttribute GetTheme()
        {
            return _root.Attribute("active");
        }

        private XElement GetThemeNode()
        {
            return (from a in _doc.Descendants("theme")
                    where a.Attribute("name").Value == _activeTheme
                    select a).First();
        }
        private Brush GetBackground()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                           where a.Name == "background"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }
        private Brush GetUiText()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                            where a.Name == "uiText"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }
        private Brush GetHeader()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                            where a.Name == "header"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }
        private Brush GetSelection()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                            where a.Name == "selection"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }
        private Brush GetTextHover()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                            where a.Name == "textHover"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }
        private Brush GetMainText()
        {
            BrushConverter converter = new BrushConverter();
            string color = (from a in _themeNode.Descendants()
                            where a.Name == "mainText"
                            select a.Attribute("color").Value).FirstOrDefault();
            return (Brush)converter.ConvertFromString(color);
        }

    }
}
