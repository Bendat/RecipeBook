using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for Fractions.xaml
    /// </summary>
    public partial class Fractions
    {
        private Window _parentWindow;
        public Fractions()
        {
            InitializeComponent();
        }
        private void Fraction_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            Label fraction = (Label) sender;
            Clipboard.SetText(fraction.Content.ToString());
            MessageBox.Show(fraction.Content +
                            " has been copied to the clipboad. " +
                            "\nYou can now paste it (ctrl-v or right-click and choose paste)");
            if (_parentWindow != null) _parentWindow.Close();
        }

        private void Exit_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null) _parentWindow.Close();
        }
    }
}
