using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
    /// Class responsible for loading and maintaining the XML document being
    /// worked with.
    /// </summary>
    class XmlLoader
    {
        /// <summary>
        /// A XML document that can be read or written to from within the RecipeBook namespace.
        /// </summary>
        public static readonly XmlDocument Doc = new XmlDocument();
        protected static string Filename; 
        /// <summary>
        /// Loads an XML file into Doc.
        /// </summary>
        /// <param name="location"></param>
        public static void LoadRecipeBook(String location)
        {
            Filename = location;
            Doc.Load(location);
        }
    }
}
