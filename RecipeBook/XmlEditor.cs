using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    /// XmlEditor is the class responsible for manipulating 
    /// </summary>
    class XmlEditor : XmlLoader
    {
        public XmlEditor(Dictionary<String, object> dict)
        {
            _recipeDictionary = dict;
            _id = FindHighestId();
        }
        private readonly Dictionary<String, object> _recipeDictionary;
        private int _id;
        private readonly XmlElement _date = Doc.CreateElement("creationDate");
        private readonly XmlElement _name = Doc.CreateElement("name");
        private readonly XmlElement _category = Doc.CreateElement("category");
        private XmlElement _image = Doc.CreateElement("image");
        private readonly XmlElement _ingredients = Doc.CreateElement("ingredientList");
        private readonly XmlElement _instructions = Doc.CreateElement("instruction");
        public static XmlNode FindbyId(int providedId)
        {
            XmlNodeList recipeNodeList = Doc.GetElementsByTagName("recipe");
            return recipeNodeList.Cast<XmlNode>()
                  .Where(xmlNode => xmlNode.Attributes != null)
                  .FirstOrDefault(xmlNode => Convert.ToInt32(xmlNode.Attributes["id"].Value)
                   == providedId);
        }
        public void WriteNodeToFile(XmlNode node)
        {
            XmlNode recipesNode = Doc.LastChild.LastChild;
            if (node.Attributes != null)
            {
                int id = Convert.ToInt32(node.Attributes["id"].Value);
                XmlNode xnode = FindbyId(id);
                if (xnode != null)
                {
                    Doc.LastChild.LastChild.RemoveChild(xnode);
                }
            }
            recipesNode.AppendChild(node);
            Doc.Save(Filename);
        }
        public XmlNode MakeNode(bool isEdit = false)
        {
            XmlNode rcp = Doc.CreateElement("recipe");
            XmlAttribute idAtt = Doc.CreateAttribute("id");
            idAtt.Value = isEdit ? _id.ToString() : _id++.ToString();
            rcp.Attributes.Append(idAtt);
            if (!_recipeDictionary["source"].Equals("source"))
            {
                XmlAttribute source = Doc.CreateAttribute("cite");
                source.Value = _recipeDictionary["source"].ToString();
                rcp.Attributes.Append(source);
            }
            _date.InnerText = _recipeDictionary["date"].ToString();
            _name.InnerText = _recipeDictionary["name"].ToString();
            _category.InnerText = _recipeDictionary["category"].ToString();
            //image.InnerText = recipeDictionary["image"].ToString();
            rcp.AppendChild(_date);
            rcp.AppendChild(_name);
            rcp.AppendChild(_category);
            // rcp.AppendChild(image);
            var ingrList = (IEnumerable<string>) _recipeDictionary["ingr"];
            NodeFromArray(_ingredients, ingrList, "ingredient");
            rcp.AppendChild(_ingredients);
            var methodList = (IEnumerable<string>) _recipeDictionary["instr"];
            NodeFromArray(_instructions, methodList, "paragraph");
            rcp.AppendChild(_instructions);
            return rcp;
        }

        private void NodeFromArray(XmlNode parent, IEnumerable<string> list, string elementName)
        {
            foreach (string instr in list)
            {
                string para = Regex.Replace(instr, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                para = para.Trim();
                XmlElement item = Doc.CreateElement(elementName);
                item.InnerText = para;
                parent.AppendChild(item);
            }
        }
        private int FindHighestId()
        {
            XmlNodeList recipeNodeList = Doc.GetElementsByTagName("recipe");
            return (from XmlNode node in recipeNodeList
                    where node.Attributes != null
                    select Convert
                   .ToInt32(node.Attributes["id"].Value)).Concat(new[] { 0 }).Max();
        }
    }
}
