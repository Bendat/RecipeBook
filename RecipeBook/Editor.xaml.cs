using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.XPath;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {

        public Editor()
        {
            isEdit = false;
            InitializeComponent();
        }

        public Editor(int recipeId)
        {
            RecipeID = recipeId;
            isEdit = true;
            InitializeComponent();
            AddNodeToEditor();
        }

        private readonly int RecipeID;
        private readonly Boolean isEdit;
        private string imageFileName;
        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Normal;
            DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CatBox_OnMouseLeftButtonUp(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            if (CatBox.Text.Trim().Equals("Category"))
            {
                CatBox.Clear();
            }
        }

        private void RecNameBox_OnMouseLeftButtonUp(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            if (RecNameBox.Text.Trim().Equals("Recipe Name"))
            {
                RecNameBox.Clear();
            }
        }

        private void SourceBox_OnMouseLeftButtonUp(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            if (SourceBox.Text.Trim().Equals("Source"))
            {
                SourceBox.Clear();
            }
        }

        private void ImageDialog_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|" +
                         "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                imageFileName = dlg.FileName;
                ImageDialog.Content = imageFileName;
            }

        }

        private Dictionary<String, Object> RetrieveData()
        {
            Dictionary<String, Object> items = new Dictionary<string, object>();
            var textBoxes = this.UserFormGrid.Children.OfType<TextBox>();
            var textBoxs = textBoxes as TextBox[] ?? textBoxes.ToArray();
            string recipeName = (from textBox in textBoxs
                                 where textBox.Name == RecNameBox.Name
                                 select textBox.Text).First();

            string source = (from textBox in textBoxs
                             where textBox.Name == SourceBox.Name
                             select SourceBox.Text).First();

            string category = (from textBox in textBoxs
                               where textBox.Name == CatBox.Name
                               select CatBox.Text).First();

            string date = DateTime.Now.ToString("dd MMMM yyyy");
            string[] ingredients = (new TextRange(
                            IngredientsInpuTextBox.Document.ContentStart,
                            IngredientsInpuTextBox.Document.ContentEnd
                            )).Text.Split('\n');

            string[] instructions = (new TextRange(
                            InstructionInputBox.Document.ContentStart,
                            InstructionInputBox.Document.ContentEnd
                            )).Text.Split('\n');
            items.Add("name", recipeName);
            items.Add("image", imageFileName);
            items.Add("source", source);
            items.Add("category", category);
            items.Add("date", date);
            items.Add("ingr", ingredients);
            items.Add("instr", instructions);
            return items;
        }
        private void AddNodeToEditor()
        {
            XmlNode node = XmlEditor.FindbyId(RecipeID);
            if (node == null)
            {
                return;
            }
            if (node["name"] != null)
            {
                RecNameBox.Text = node["name"].InnerXml.Trim();
            }
            if (node["category"] != null)
            {
                CatBox.Text = node["category"].InnerXml.Trim();
            }
            if (node["ingredientList"] != null)
            {
                foreach (XmlNode subnode in node["ingredientList"])
                {
                    string para = subnode.InnerXml;
                    para = Regex.Replace(para, @"^\s+$[\r\n]*", "", RegexOptions.Multiline).Trim();
                    IngredientsInpuTextBox.Document.Blocks.Add(new Paragraph(new Run(para)));
                }
            }
            if (node["instruction"] != null)
            {
                foreach (XmlNode subnode in node["instruction"])
                {
                    string para = subnode.InnerXml;
                    para = Regex.Replace(para, @"^\s+$[\r\n]*", "", RegexOptions.Multiline).Trim();
                    InstructionInputBox.Document.Blocks.Add(new Paragraph(new Run(para)));
                }
            }
        }
        private void OkButton_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Dictionary<String, Object> data = RetrieveData();
            XmlEditor xmle = new XmlEditor(data);
            var newNode = isEdit ? xmle.MakeNode(true) : xmle.MakeNode();
            xmle.WriteNodeToFile(newNode);
            Thread.Sleep(200);
            this.Close();
            ((MainWindow)System.Windows.Application.Current.MainWindow).Run();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
