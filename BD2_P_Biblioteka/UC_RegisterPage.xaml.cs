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
    public partial class UC_RegisterPage : UserControl
    {
        public UC_RegisterPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            tbInfo.Text = "";
            tbInfo.Foreground = Brushes.Red;

            String name = tbName.Text;
            String surname = tbSurname.Text;
            String phoneNumber = tbPhoneNumber.Text;
            String email = tbEmail.Text;
            String password = tbPassword.Password;

            if (name == null || name.Trim() == "" || surname == null || surname.Trim() == "" ||
                phoneNumber == null || phoneNumber.Trim() == "" || email == null || email.Trim() == "" ||
                password == null || password.Trim() == "")
            {
                tbInfo.Text = "[BŁĄD] Uzupelnij wszystkie dane!";
            }
            else if(!int.TryParse(tbPhoneNumber.Text, out _))
            {
                tbInfo.Text = "[BŁĄD] Wpisany Nr Telefonu jest niepoprawny!";
            }
            else if(DatabaseManager.CheckEmail(email))
            {
                tbInfo.Text = "[BŁĄD] Podany E-Mail już jest w użyciu!";
            }
            else if(cbAccept.IsChecked == false)
            {
                tbInfo.Text = "[BŁĄD] Musisz zaakceptować regulamin!";
            }
            else
            {
                DatabaseManager.RegisterUser(name, surname, phoneNumber, email, password);

                tbInfo.Foreground = Brushes.Green;
                tbInfo.Text = "[REJESTRACJA POMYŚLNA] Możesz się zalogować za pomocą adresu E-Mail oraz hasła.";
            }
        }
    }
}
