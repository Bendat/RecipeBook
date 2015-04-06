using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for RecipeEditor.xaml
    /// </summary>
    public partial class RecipeEditor
    {
        private Window _parentWindow;
        private readonly int _recipeId;
        private readonly Boolean _isEdit;
        private string _imageFileName;
        public RecipeEditor()
        {
            _isEdit = false;
            InitializeComponent();
        }
        

        public RecipeEditor(int recipeId)
        {
            _recipeId = recipeId;
            _isEdit = true;
            InitializeComponent();
            AddNodeToEditor();
        }

        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null)
            {
                _parentWindow.WindowState = WindowState.Normal;
                _parentWindow.DragMove();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null) _parentWindow.Close();
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
        //Currently useless.
        private void ImageDialog_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|" +
                         "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                _imageFileName = dlg.FileName;
                ImageDialog.Content = _imageFileName;
            }

        }

        private Dictionary<String, Object> RetrieveData()
        {
            Dictionary<String, Object> items = new Dictionary<string, object>();
            var textBoxes = MainDataGrid.Children.OfType<TextBox>();
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
                            IngredientsInputTextBox.Document.ContentStart,
                            IngredientsInputTextBox.Document.ContentEnd
                            )).Text.Split('\n');

            string[] instructions = (new TextRange(
                            InstructionInputBox.Document.ContentStart,
                            InstructionInputBox.Document.ContentEnd
                            )).Text.Split('\n');

            items.Add("name", recipeName);
            items.Add("image", _imageFileName);
            items.Add("source", source);
            items.Add("category", category);
            items.Add("date", date);
            items.Add("ingr", ingredients);
            items.Add("instr", instructions);
            return items;
        }
        private void AddNodeToEditor()
        {
            XmlNode node = XmlLoader.FindbyId(_recipeId);
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
                    para = para.Trim();
                    if (para != "")
                    {
                        IngredientsInputTextBox.Document.Blocks.Add(new Paragraph(new Run(para)));
                    }
                }
            }
            if (node["instruction"] != null)
            {
                foreach (XmlNode subnode in node["instruction"])
                {
                    string para = subnode.InnerXml;
                    para = para.Trim();
                    if (para != "")
                    {
                        InstructionInputBox.Document.Blocks.Add(new Paragraph(new Run(para)));
                    }
                }
            }
        }
        private void OkButton_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            Dictionary<String, Object> data = RetrieveData();
            XmlEditor xmle = new XmlEditor(data);
            var newNode = _isEdit ? xmle.EditXmlNode(XmlLoader.FindbyId(_recipeId)) : xmle.MakeNode();
            xmle.WriteNodeToFile(newNode);
            if (_parentWindow != null) _parentWindow.Close();
            ((MainWindow)Application.Current.MainWindow).Run();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null) _parentWindow.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Fractions fraction = new Fractions();
            Window window = new Window
            {
                Content = fraction,
                Height = 196, ShowInTaskbar = false,
                Width = 176, AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }
        
    }
}
