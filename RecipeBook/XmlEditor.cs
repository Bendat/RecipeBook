using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Annotations;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
    ///     XmlEditor is the class responsible for manipulating
    /// </summary>
    internal class XmlEditor : XmlLoader
    {
        private readonly XmlElement _category = Doc.CreateElement("category");
        private readonly XmlElement _date = Doc.CreateElement("creationDate");
        private readonly XmlElement _ingredients = Doc.CreateElement("ingredientList");
        private readonly XmlElement _instructions = Doc.CreateElement("instruction");
        private readonly XmlElement _name = Doc.CreateElement("name");
        private readonly Dictionary<String, object> _recipeDictionary;
        private XmlElement _image = Doc.CreateElement("image");

        public XmlEditor(Dictionary<String, object> dict)
        {
            _recipeDictionary = dict;
        }
        /// <summary>
        /// Saves the provided XmlNode to the Xml file stored in the XmlLoader class.
        /// </summary>
        /// <param name="node">The XmlNode to save to the file.</param>
        public void WriteNodeToFile(XmlNode node)
        {
            var recipesNode = Doc.LastChild.LastChild;
            if (node.Attributes != null && node.Attributes["id"] != null)
            {
                var id = Convert.ToInt32(node.Attributes["id"].Value);
                var xnode = FindbyId(id);
                if (xnode != null)
                {
                    Doc.LastChild.LastChild.RemoveChild(xnode);
                }
            }
            recipesNode.AppendChild(node);
            Doc.Save(Filename);
        }
        /// <summary>
        /// Creates a new node with the data provided to the XmlEditor instance.
        /// </summary>
        /// <returns>An XmlNode in the Recipe format.</returns>
        public XmlNode MakeNode()
        {
            int id = FindHighestId() + 1;
            XmlNode rcp = Doc.CreateElement("recipe");
            var idAtt = Doc.CreateAttribute("id");
            idAtt.Value = id.ToString();
            if (rcp.Attributes != null)
            {
                rcp.Attributes.Append(idAtt);
                if (!_recipeDictionary["source"].Equals("source"))
                {
                    var source = Doc.CreateAttribute("cite");
                    source.Value = _recipeDictionary["source"].ToString();
                    rcp.Attributes.Append(source);
                }
            }
            AddFieldsToNode(rcp);
            return rcp;
        }
        /// <summary>
        /// Edits an existing XmlNode in XmlLoaders stored file.
        /// </summary>
        /// <param name="node">The existing node to edit</param>
        /// <returns>A reference to the edited node.</returns>
        public XmlNode EditXmlNode(XmlNode node)
        {
            if (node == null || node.Attributes["id"] == null)
            {
                return MakeNode();
            }
            XmlAttribute idAtt = node.Attributes["id"];
            node.RemoveAll();
            node.Attributes.Append(idAtt);
            Console.WriteLine(node.Attributes["id"]);
            AddFieldsToNode(node);
            return node;
        }
        //Adds all the fields from this object to a new XmlNode
        //Or overwrites the contents of an existing node.
        private void AddFieldsToNode(XmlNode node)
        {
            var ingrList = (IEnumerable<string>)_recipeDictionary["ingr"];
            var methodList = (IEnumerable<string>)_recipeDictionary["instr"];
            _date.InnerText = _recipeDictionary["date"].ToString();
            _name.InnerText = _recipeDictionary["name"].ToString();
            _category.InnerText = _recipeDictionary["category"].ToString();
            if(_recipeDictionary["image"] != null)
            {
                _image.InnerText = FileHandler.AddImageToApp(_recipeDictionary["image"].ToString());
            }
            else
            {
                _image.InnerText = Constants.Cookbook;
            }
            node.AppendChild(_date);
            node.AppendChild(_name);
            node.AppendChild(_category);
            node.AppendChild(_image);
            NodeFromArray(_ingredients, ingrList, "ingredient");
            node.AppendChild(_ingredients);
            NodeFromArray(_instructions, methodList, "paragraph");
            node.AppendChild(_instructions);
        }
        private void NodeFromArray(XmlNode parent, IEnumerable<string> list, string elementName)
        {
            foreach (var instr in list)
            {
                var para = instr;
                para = para.Trim();
                var item = Doc.CreateElement(elementName);
                item.InnerText = para;
                parent.AppendChild(item);
            }
        }
        private int FindHighestId()
        {
            var recipeNodeList = Doc.GetElementsByTagName("recipe");
            if (recipeNodeList.Count == 0) return 0;
            var xdoc = XDocument.Load("xmlData/recipe.rcpbk");
            int maxId = xdoc.Descendants("recipe")
                        .Max(x => (int)x.Attribute("id"));
            return maxId;
        }
    }
}