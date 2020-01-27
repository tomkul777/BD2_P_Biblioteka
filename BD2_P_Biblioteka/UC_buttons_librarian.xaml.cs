using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD2_P_Biblioteka
{
    public partial class UC_buttons_librarian : UserControl
    {
        public UC_buttons_librarian()
        {
            InitializeComponent();
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

        private void BtnBorrows_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
            }
            else
            {
                mainWindow.ShowBorrowPageLibrarian();
            }
        }
    }
}
