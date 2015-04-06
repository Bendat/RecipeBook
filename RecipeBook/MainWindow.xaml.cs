﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using WinInterop = System.Windows.Interop;
using System.Windows.Navigation;
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
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// A collection of Recipe objects.
        /// </summary>
        private ObservableCollection<Recipe> Recipes { get; set; }
        private const string WelcomeFile = "WelcomeScreen/welcome.rtf";
        private FileStream _fs;
        private int _currentId;

        public MainWindow()
        {
            Theme.UseTheme(true);
            InitializeComponent();
            Run();
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        public void Run()
        {
            Recipes = new ObservableCollection<Recipe>();
            var view = CollectionViewSource.GetDefaultView(Recipes);
            Maximiser winMaximiser = new Maximiser(PrimaryWindow);
            winMaximiser.Run();
            LoadRecipes();
            LoadRichTextBox();
            view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            view.SortDescriptions.Add(new SortDescription("Category", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("RecipeName", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            RecipeList.ItemsSource = view;
            
        }
        //Sets up the welcome screen. Clears all other text fields.
        private void LoadRichTextBox()
        {
            BodyText.Document.Blocks.Clear();
            IngredientsText.Document.Blocks.Clear();
            IngredientsPrompt.Text = null;
            InstructionsPrompt.Text = null;
            TitleBox.Document.Blocks.Clear();
            var tr = new TextRange(BodyText.Document.ContentStart, BodyText.Document.ContentEnd);
            using (_fs = new FileStream(WelcomeFile, FileMode.OpenOrCreate))
            {
                tr.Load(_fs, DataFormats.Rtf);
            }
        }
        //Loads the default recipe file into the XmlLoader class.
        private void LoadRecipes()
        {
            XmlLoader.LoadRecipeBook(@"xmlData\recipe.xml");
            XmlNodeList recipeNodeList = XmlLoader.Doc.GetElementsByTagName("recipe");
            foreach (XmlNode node in recipeNodeList)
            {
                Recipes.Add(new Recipe(node));
            }
        }
#region EventHandlers
        //Determines the recipe that has been clicked on and loads it onto the screen.
        public void ItemLeftClick(object sender, MouseButtonEventArgs e)
        {
            var selectedStockObject = RecipeList.SelectedItems[0] as Recipe;
            if (selectedStockObject == null)
            {
                return;
            }
            int id = selectedStockObject.Id;
            var rec = from recipe in Recipes
                      where recipe.Id == id
                      select recipe;
            Recipe loadedRecipe = rec.FirstOrDefault();

            if (loadedRecipe != null)
            {
                _currentId = loadedRecipe.Id;
                Console.WriteLine(loadedRecipe.Id);
                RecipeFormatter rf = new RecipeFormatter();
                rf.AddContent(TitleBox, loadedRecipe.RecipeName, DocumentSection.Title);
                rf.AddPrompt(InstructionsPrompt, " method", DocumentSection.Prompt, true);
                rf.AddContent(BodyText, loadedRecipe.Instructions, DocumentSection.Text);
                rf.AddPrompt(IngredientsPrompt, "Ingredients", DocumentSection.Prompt);
                rf.AddContent(IngredientsText, loadedRecipe.Ingredients, DocumentSection.Text, .8);
                if (!string.IsNullOrEmpty(loadedRecipe.Source))
                {
                    rf.AddHyperLink(HyperLabel, loadedRecipe.Source);
                }
            }
        }
        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                this.ShowCenteredToMouse();
            }
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Maximise_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void Minimise_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void HomeButton_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoadRichTextBox();
        }
        

        private void NewWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RecipeEditor editor = new RecipeEditor();
            Window window = new Window
            {
                Content = editor,
                Height = 500, ShowInTaskbar = false,
                Width = 760,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        //Opens a new window to edit the current file.
        //Known issue: Multiple edit windows can be opened at once, resulting in the creation of
        //multiple recipes with the same id
        private void EditWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var editor = _currentId >= 0 ? new RecipeEditor(_currentId) : new RecipeEditor();
            Window window = new Window
            {
                Content = editor,
                Height = 500,
                Width = 760,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }


        private void SettingsLabel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Window window = new Window
            {
                Content = new Settings(),
                Height = 390,
                Width = 580, AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }
    }
#endregion    
    
}