using SudokuSolver.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace SudokuSolver.Pages
{
    public partial class Landing : Page
    {
        public Landing()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void navGrid_Click(object sender, RoutedEventArgs e)
        {
            NavButton clickedButton = e.OriginalSource as NavButton;

            NavigationService.Navigate(clickedButton.NavUri);
        }
    }
}
