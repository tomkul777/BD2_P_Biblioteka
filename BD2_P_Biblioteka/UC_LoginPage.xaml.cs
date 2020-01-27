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
    public partial class UC_LoginPage : UserControl
    {
        public UC_LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            tbInfo.Text = "";
            tbInfo.Foreground = Brushes.Red;

            if (tbEmail.Text == null || tbEmail.Text.Trim() == "" || tbPassword.Password == null || tbPassword.Password.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Uzupelnij wszystkie dane!";
            }
            else
            {
                String email = tbEmail.Text;
                String password = tbPassword.Password;
                bool isLogged = DatabaseManager.LoginUser(email, password);

                if (!isLogged) tbInfo.Text = "[BŁĄD] Konto nie istnieje!";
                else
                {
                    tbInfo.Foreground = Brushes.Green;
                    tbInfo.Text = "[LOGOWANIE POMYŚLNE] Zostałeś zalogowany do konta.\nZa 3s nastąpi przekierowanie.";

                    LoginDelay();
                }
            }
        }

        private async void LoginDelay()
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            await Task.Delay(TimeSpan.FromSeconds(3));

            mainWindow.ShowBooksCatalog();
        }
    }
}
