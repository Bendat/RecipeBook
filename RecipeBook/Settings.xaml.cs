using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private Window parent;
        public Settings()
        {
            InitializeComponent();
        }
        //retrieves the selected option. 
        private string ProcessData()
        {
            RadioButton checkedButton = 
                (from a in MainGrid.Children.OfType<RadioButton>()
                 where a.IsChecked == true
                 select a).First();
            return checkedButton.Content.ToString().ToLower();
        }
        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            parent = Window.GetWindow(this);
            parent.DragMove();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            parent = Window.GetWindow(this);
            parent.Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            parent = Window.GetWindow(this);
            string theme = ProcessData();
            Theme.ThemeReader.SetActive(theme);
            MessageBox.Show("Changes will take effect next time you start the program");
            parent.Close();
            ((MainWindow)Application.Current.MainWindow).Run();
        }
    }
}
