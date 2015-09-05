using System;
using System.Collections;
using System.Collections.Generic;
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
namespace RecipeBook {
    /// <summary>
    /// The Recipe class defines a Recipe object which contains all data associated with a particular recipe from
    /// a recipe book XML file or a .rcpbk.
    /// </summary>
    public class Recipe {
        protected Recipe()
        {
        }
        /// <summary>
        /// Creates a new recipe object using an XmlNode which contains the recipe data.
        /// </summary>
        /// <param name="node">The XmlNode to parse.</param>
        public Recipe(XmlNode node)
        {
            CreateRecipe(node);
        }
        /// <summary>
        /// The recipes unique id number.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The date a recipe was created.
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// The name or title of the recipe.
        /// </summary>
        public string RecipeName { get; set; }
        /// <summary>
        /// The category under which the recipe fits best.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// The absolute url to the website where this recipe was taken from.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// The image file associated with the recipe
        /// </summary>
        
        //unimplemented------------------------------
        public string ImageLocation { get; set; }
        /// <summary>
        /// An set of ingredients needed for the recipe.
        /// </summary>
        public List<string> Ingredients = new List<string>();
        /// <summary>
        /// A set of instructions needed for the recipe.
        /// </summary>
        public List<string> Instructions = new List<string>();

        //Converts an XmlNode object into a Recipe object.
        private void CreateRecipe(XmlNode node)
        {
            if (node.Attributes == null) return;
            if (node.Attributes["id"] != null)
            {
                Id = Convert.ToInt32(node.Attributes["id"].Value);
                if (node.Attributes["cite"] != null)
                {
                    Source = node.Attributes["cite"].Value;
                }
            }
            CreationDate = node["creationDate"] != null 
                ? node["creationDate"].InnerText : "No creation date found";

            RecipeName = node["name"] != null
                ? node["name"].InnerText.ToLower() : "No Name Found";

            Category = node["category"] != null 
                ? node["category"].InnerText : "No Category Found";

            ImageLocation = node["image"] != null 
                ? node["image"].InnerText : " ";

            if (node["ingredientList"] != null)
            {
                foreach (XmlNode subnode in node["ingredientList"])
                {
                    Ingredients.Add(subnode.InnerXml);
                }
            }
            else
            {
                Ingredients.Add("No ingredients found");
            }
            if (node["instruction"] != null)
            {
                foreach (XmlNode subnode in node["instruction"])
                {
                    Instructions.Add(subnode.InnerXml);
                }
            }
            else
            {
                Instructions.Add("No instructions found");
            }
        }
    }
}
