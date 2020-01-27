using System.Windows;
using System.Windows.Controls;

namespace BD2_P_Biblioteka
{
    public partial class UC_buttons_logged : UserControl
    {
        public UC_buttons_logged()
        {
            InitializeComponent();

            UpdateBorrowsCount();
        }

        private void UpdateBorrowsCount()
        {
            if (DatabaseManager.user == null) return;

            lblBasketsCount.Content = DatabaseManager.GetBasketCount(DatabaseManager.user);
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            DatabaseManager.user = null;

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.ShowBooksCatalog();
        }

        private void BtnBooksCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.ShowBooksCatalog();
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
            }
            else
            {
                mainWindow.ShowAccountPage();
            }
        }

        private void BtnBorrows_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
            }
            else
            {
                mainWindow.ShowBorrowPage();
            }
        }

        private void BtnBasket_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
            }
            else
            {
                mainWindow.ShowBasket();
            }
        }

        /*private void SetUserContol()
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
            }
        }*/
    }
}
