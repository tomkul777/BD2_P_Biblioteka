using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ShowBooksCatalog();
        }

        public void ShowBooksCatalog()
        {
            gridMainContent.Children.Clear();
            UserControl booksCatalog = new UC_BooksCatalog();
            gridMainContent.Children.Add(booksCatalog);

            ShowButton();
        }

        public void ShowLoginPage()
        {
            if (DatabaseManager.user != null) return;

            gridMainContent.Children.Clear();
            UserControl loginPage = new UC_LoginPage();
            gridMainContent.Children.Add(loginPage);

            ShowButton();
        }

        public void ShowRegisterPage()
        {
            if (DatabaseManager.user != null) return;

            gridMainContent.Children.Clear();
            UserControl registerPage = new UC_RegisterPage();
            gridMainContent.Children.Add(registerPage);

            ShowButton();
        }

        public void ShowAccountPage()
        {
            if (DatabaseManager.user == null) return;

            gridMainContent.Children.Clear();
            UserControl accountPage = new UC_AccountPage();
            gridMainContent.Children.Add(accountPage);

            ShowButton();
        }

        public void ShowBorrowPage()
        {
            if (DatabaseManager.user == null) return;

            gridMainContent.Children.Clear();
            UserControl borrowPage = new UC_BorrowsPage();
            gridMainContent.Children.Add(borrowPage);

            ShowButton();
        }

        public void ShowBasket(int page = 0)
        {
            if (DatabaseManager.user == null) return;

            gridMainContent.Children.Clear();
            UserControl basket = new UC_Basket(page);
            gridMainContent.Children.Add(basket);

            ShowButton();
        }

        public void ShowBorrowPageLibrarian()
        {
            if (DatabaseManager.user == null) return;

            gridMainContent.Children.Clear();
            UserControl basket = new UC_BorrowsPage_librarian();
            gridMainContent.Children.Add(basket);

            ShowButton();
        }

        public void ShowButton()
        {
            if (DatabaseManager.user == null)
            {
                ShowButtonsGuest();
            }
            else
            {
                if (DatabaseManager.user.levelAccess == "Bibliotekarz")
                {
                    ShowButtonsLibrarian();
                }
                else
                {
                    ShowButtonsLogged();
                }
            }
        }

        private void ShowButtonsGuest()
        {
            gridButtons.Children.Clear();
            UserControl buttons = new UC_buttons_guest();
            gridButtons.Children.Add(buttons);
        }

        private void ShowButtonsLogged()
        {
            gridButtons.Children.Clear();
            UserControl buttons = new UC_buttons_logged();
            gridButtons.Children.Add(buttons);
        }

        private void ShowButtonsLibrarian()
        {
            gridButtons.Children.Clear();
            UserControl buttons = new UC_buttons_librarian();
            gridButtons.Children.Add(buttons);
        }
    }
}
