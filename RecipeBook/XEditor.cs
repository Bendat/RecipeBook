using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RecipeBook
{
    class XEditor : XmlLoader
    {
        private readonly XElement _category = new XElement("category");
        private readonly XElement _date = new XElement("creationDate");
        private readonly XElement _ingredients = new XElement("ingredientList");
        private readonly XElement _instructions = new XElement("instruction");
        private readonly XElement _name = new XElement("name");
        private readonly Dictionary<String, object> _recipeDictionary;
        private XElement _image = new XElement("image");

        public void printstuff()
        {
            Console.WriteLine(_category + " " + _date + " " + _name);
        }
        public XEditor(Dictionary<String, object> dict)
        {
            _recipeDictionary = dict;
        }
        /// <summary>
        /// Saves the provided XmlNode to the Xml file stored in the XmlLoader class.
        /// </summary>
        /// <param name="node">The XmlNode to save to the file.</param>
        public void WriteNodeToFile(XElement node)
        {
            var recipesNode = XDoc.Descendants("recipes").First();
            var xnodes = (from a in XDoc.Descendants("recipe")
                          where a.Attribute("id").Value == node.Attribute("id").Value
                          select a);
            var xElements = xnodes as XElement[] ?? xnodes.ToArray();
            if (xElements.Any())
            {
                xElements.First().Remove();
            }
            recipesNode.Add(node);
            Doc.Save(Filename);
        }
        /// <summary>
        /// Creates a new node with the data provided to the XmlEditorOld instance.
        /// </summary>
        /// <returns>An XmlNode in the Recipe format.</returns>
        public XElement MakeNode()
        {
            int id = FindHighestId() + 1;
            XElement rcp = new XElement("recipe");
            rcp.SetAttributeValue("id", id);
            if (!_recipeDictionary["source"].Equals("source"))
            {
                string sourceValue = _recipeDictionary["source"].ToString();
                rcp.SetAttributeValue("cite", sourceValue);
            }
            AddFieldsToNode(rcp);
            return rcp;
        }
        /// <summary>
        /// Edits an existing XmlNode in XmlLoaders stored file.
        /// </summary>
        /// <param name="node">The existing node to edit</param>
        /// <returns>A reference to the edited node.</returns>
        public XElement EditXmlNode(XElement element)
        {
            if (element == null || element.Attributes("id") == null)
            {
                return MakeNode();
            }
            var idAtt = element.Attributes("id");
            int id = Convert.ToInt32(
                    (from a in idAtt
                     select a.Value)
                    .First());
            element.RemoveAll();
            element.SetAttributeValue("id", id);
            AddFieldsToNode(element);
            return element;
        }
        //Adds all the fields from this object to a new XmlNode
        //Or overwrites the contents of an existing node.
        private void AddFieldsToNode(XElement element)
        {
            var ingrList = (IEnumerable<string>)_recipeDictionary["ingr"];
            var methodList = (IEnumerable<string>)_recipeDictionary["instr"];
            _date.SetValue(_recipeDictionary["date"].ToString());
            _name.SetValue(_recipeDictionary["name"].ToString());
            _category.SetValue(_recipeDictionary["category"].ToString());
            //image.InnerText = recipeDictionary["image"].ToString();
            element.Add(_date);
            element.Add(_name);
            element.Add(_category);
            // rcp.AppendChild(image);
            NodeFromArray(_ingredients, ingrList, "ingredient");
            element.Add(_ingredients);
            NodeFromArray(_instructions, methodList, "paragraph");
            element.Add(_instructions);
        }
        private void NodeFromArray(XElement parent, IEnumerable<string> list, string elementName)
        {
            foreach (var instr in list)
            {
                var para = instr;
                para = para.Trim();
                var item = new XElement(elementName);
                item.SetValue(para);
                parent.Add(item);
            }
        }

        private int FindHighestId()
        {
            int maxId = XDoc.Descendants("recipe")
                .Max(x => (int) x.Attribute("id"));
            return maxId;
        }
    }
}
