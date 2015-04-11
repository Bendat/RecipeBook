using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for Fractions.xaml
    /// </summary>
    public partial class Fractions : UserControl
    {
        private Window parent;
        public Fractions()
        {
            InitializeComponent();
        }
        private void Fraction_Click(object sender, RoutedEventArgs e)
        {
            parent = Window.GetWindow(this);
            Label fraction = (Label) sender;
            Clipboard.SetText(fraction.Content.ToString());
            MessageBox.Show(fraction.Content.ToString() +
                            " has been copied to the clipboad. " +
                            "\nYou can now paste it (ctrl-v or right-click and choose paste)");
            parent.Close();
        }

        private void Exit_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
