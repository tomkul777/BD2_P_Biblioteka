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
    public partial class UC_AccountPage : UserControl
    {
        public UC_AccountPage()
        {
            InitializeComponent();

            Configure();
        }

        private void Configure()
        {
            tbEmail.Text = DatabaseManager.user.Email;
            tbMoney.Text = DatabaseManager.user.Fees.ToString();
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            if (double.Parse(tbMoney.Text) < 0.01) return;

            DatabaseManager.Pay(DatabaseManager.user);
            Configure();
        }

        private void BtnChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            tbInfo.Text = "";
            tbInfo.Foreground = Brushes.Red;
            if (tbPassword.Password == null || tbPassword.Password.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Aby zmienić adres E-Mail podaj aktualne hasło!";
                return;
            }
            else if(!DatabaseManager.CheckPassword(DatabaseManager.user, tbPassword.Password))
            {
                tbInfo.Text = "[BŁĄD] Podane aktualne hasło jest niepoprawne!";
                return;
            }
            else if (tbNewEmail.Text == null || tbNewEmail.Text.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Aby zmienić adres E-Mail podaj nowy adres E-Mail!";
                return;
            }

            bool isEmailExist = DatabaseManager.CheckEmail(tbNewEmail.Text);
            if(isEmailExist) tbInfo.Text = "[BŁĄD] Ten adres E-Mail jest zajęty!";
            else
            {
                DatabaseManager.ChangeEmail(DatabaseManager.user, tbNewEmail.Text);

                Configure();

                tbInfo.Foreground = Brushes.Green;
                tbInfo.Text = "[ZMIANA] Zmiana adresu E-Mail została przeprowadzona pomyślnie.";
            }
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            tbInfo.Text = "";
            tbInfo.Foreground = Brushes.Red;
            if (tbPassword.Password == null || tbPassword.Password.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Aby zmienić hasło podaj aktualne hasło!";
                return;
            }
            else if (!DatabaseManager.CheckPassword(DatabaseManager.user, tbPassword.Password))
            {
                tbInfo.Text = "[BŁĄD] Podane aktualne hasło jest niepoprawne!";
                return;
            }
            else if (tbNewPassword.Password == null || tbNewPassword.Password.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Aby zmienić hasło podaj nowe hasło!";
                return;
            }

            DatabaseManager.ChangePassword(DatabaseManager.user, tbNewPassword.Password);

            tbInfo.Foreground = Brushes.Green;
            tbInfo.Text = "[ZMIANA] Zmiana hasła została przeprowadzona pomyślnie.";
        }
    }
}
