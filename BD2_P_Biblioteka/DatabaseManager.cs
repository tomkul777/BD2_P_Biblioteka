using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2_P_Biblioteka
{
    static class DatabaseManager
    {
        private static string connectionString = "User Id=c##library_admin;Password=pass;Data Source=localhost:1521/xe";

        public static User user;
        public static User user_other;

        public static List<Book> GetBooksCatalog(int page, int count)
        {
            string command = "SELECT * FROM ksiazki OFFSET " + (page * count) + " ROW FETCH NEXT " + count + " ROWS ONLY";

            DataTable dt = GetDataTable(command);

            List<Book> books = new List<Book>();
            foreach (DataRow row in dt.Rows)
            {
                Book book = new Book();
                book.Id = row["id_ksiazka"] as Nullable<decimal>;
                book.Autor = row["autor"] as string;
                book.Tytul = row["tytul"] as string;
                book.Wydawnictwo = row["wydawnictwo"] as string;
                book.RokWydania = row["rok_Wydania"] as Nullable<decimal>;
                book.Opis = row["opis"] as string;

                string command1 = "SELECT ilosc_dostepna FROM stan_ksiazek WHERE id_ksiazka = " + book.Id;
                book.Ilosc = DoScalar(command1) as Nullable<decimal>;

                books.Add(book);
            }

            return books;
        }

        public static int GetBooksCount()
        {
            string command = "SELECT COUNT(*) FROM ksiazki";
            int count = Convert.ToInt32(DoScalar(command));
            return count;
        }

        public static bool CheckEmail(String email)
        {
            string command = "SELECT COUNT(*) FROM uzytkownicy WHERE email = '" + email + "'";
            bool isEquals = Convert.ToInt32(DoScalar(command)) > 0;
            return isEquals;
        }

        public static bool CheckPassword(User user1, String password)
        {
            string command = "SELECT haslo FROM uzytkownicy WHERE email = '" + user1.Email + "'";
            bool isEquals = DoScalar(command).ToString().Equals(password);
            return isEquals;
        }

        public static void ChangePassword(User user1, String password)
        {
            List<string> commands = new List<string>();

            commands.Add(
                "UPDATE uzytkownicy SET haslo = " + password + "WHERE id_uzytkownik = " + user1.Id
            );

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                 " VALUES (" + user1.Id + ", 'zmiana hasła', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static void ChangeEmail(User user1, String email)
        {
            List<string> commands = new List<string>();

            commands.Add(
                "UPDATE uzytkownicy SET email = " + email + "WHERE id_uzytkownik = " + user1.Id
            );

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + user1.Id + ", 'zmiana emailu', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static void Pay(User user1)
        {
            List<string> commands = new List<string>();

            commands.Add(
                "UPDATE uzytkownicy SET oplaty = 0 WHERE id_uzytkownik = " + user1.Id
            );

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + user1.Id + ", 'zaplata naleznosci', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static void RegisterUser(String name, String surname, String phoneNumber, String email, String password)
        {
            string command =
                    "INSERT INTO uzytkownicy(imie, nazwisko, nr_telefonu, email, haslo, poziom_konta, oplaty) VALUES('" +
                    name + "', '" + surname + "', " + phoneNumber + ", '" + email + "', '" + password +
                    "', 'Czytelnik', 0)";

            DoQuery(command);
        }

        public static bool LoginUser(string email, string password)
        {
            string command = "SELECT * FROM uzytkownicy WHERE email = '" + email + "' AND haslo = '" + password + "'";

            DataTable dt = GetDataTable(command);

            if (dt.Rows.Count < 1) return false;
            else
            {
                User user = new User();
                user.Id = dt.Rows[0]["id_uzytkownik"] as Nullable<decimal>;
                user.Email = dt.Rows[0]["email"] as string;
                user.Fees = dt.Rows[0]["oplaty"] as Nullable<double>;
                user.levelAccess = dt.Rows[0]["poziom_konta"] as string;
                DatabaseManager.user = user;
                return true;
            }
        }

        public static bool GetUser(int id)
        {
            string command = "SELECT * FROM uzytkownicy WHERE id_uzytkownik = " + id;

            DataTable dt = GetDataTable(command);

            if (dt.Rows.Count < 1) return false;
            else
            {
                User user = new User();
                user.Id = dt.Rows[0]["id_uzytkownik"] as Nullable<decimal>;
                user.Email = dt.Rows[0]["email"] as string;
                user.Fees = dt.Rows[0]["oplaty"] as Nullable<double>;
                user.levelAccess = dt.Rows[0]["poziom_konta"] as string;
                DatabaseManager.user_other = user;
                return true;
            }
        }

        public static List<Book> GetBasketCatalog(User user1, int page, int count)
        {
            string command = "SELECT k2.id_ksiazka, k2.autor, k2.tytul, k2.wydawnictwo, k2.rok_wydania, k2.opis, k1.id_koszyk " +
                "FROM koszyk k1, ksiazki k2, uzytkownicy u WHERE k1.id_ksiazka = k2.id_ksiazka AND u.id_uzytkownik = " + user1.Id +
                " OFFSET " + (page * count) + " ROW FETCH NEXT " + count + " ROWS ONLY";

            DataTable dt = GetDataTable(command);

            List<Book> books = new List<Book>();
            foreach (DataRow row in dt.Rows)
            {
                Book book = new Book();
                book.Id = row["id_ksiazka"] as Nullable<decimal>;
                book.Autor = row["autor"] as string;
                book.Tytul = row["tytul"] as string;
                book.Wydawnictwo = row["wydawnictwo"] as string;
                book.RokWydania = row["rok_wydania"] as Nullable<decimal>;
                book.Opis = row["opis"] as string;
                book.IdKoszyk = row["id_koszyk"] as Nullable<decimal>;

                books.Add(book);
            }

            return books;
        }

        public static int GetBasketCount(User user1)
        {
            string command = "SELECT COUNT(*) FROM koszyk WHERE id_uzytkownik = " + user1.Id;
            int count = Convert.ToInt32(DoScalar(command));
            return count;
        }

        public static bool AddToBasket(User user1, Book book)
        {
            string command = "SELECT ilosc_dostepna FROM stan_ksiazek WHERE id_ksiazka = " + book.Id;
            int count = Convert.ToInt32(DoScalar(command));
            if (count < 1) return false;

            List<string> commands = new List<string>();

            commands.Add(
                "INSERT INTO koszyk (id_uzytkownik, id_ksiazka)" +
                " VALUES (" + user1.Id + ", " + book.Id + ")"
            );

            commands.Add(
                "UPDATE stan_ksiazek SET ilosc_dostepna = ilosc_dostepna - 1 WHERE id_ksiazka = " + book.Id
            );

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + user.Id + ", 'dodano do koszyka id_ksiazka = " + book.Id + "', '" + date + "')"
            );

            DoTransaction(commands);
            return true;
        }

        public static void ClearBasketCatalog(User user1, bool takeBack)
        {
            List<string> commands = new List<string>();

            if (takeBack)
            {
                commands.Add(
                    "UPDATE stan_ksiazek SET ilosc_dostepna = ilosc_dostepna + (SELECT COUNT(*) " +
                    "FROM koszyk WHERE id_uzytkownik = " + user1.Id + " AND id_ksiazka = stan_ksiazek.id_ksiazka GROUP BY id_ksiazka)" +
                    " WHERE id_ksiazka IN (SELECT id_ksiazka FROM koszyk WHERE id_uzytkownik = " + user1.Id + ")"
                );
            }

            commands.Add(
                "DELETE FROM koszyk WHERE id_koszyk IN (SELECT id_koszyk FROM koszyk WHERE id_uzytkownik = " + user1.Id + ")"
            );

            if (takeBack)
            {
                String date = DateTime.Now.ToString("yy-MM-dd");
                commands.Add(
                    "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                    " VALUES (" + user1.Id + ", 'czysc koszyk', '" + date + "')"
                );
            }

            DoTransaction(commands);
        }

        public static void RemoveFromBasket(User user1, Book book)
        {
            List<string> commands = new List<string>();

            commands.Add(
                "UPDATE stan_ksiazek SET ilosc_dostepna = ilosc_dostepna + 1 WHERE id_ksiazka = " + book.Id
            );

            commands.Add(
                "DELETE FROM koszyk WHERE id_koszyk = " + book.IdKoszyk
            );

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + user1.Id + ", 'usunieto z  koszyka id_ksiazka = " + book.Id + "', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static List<BorrowedBook> GetBorrowedBooks(User user1, int page, int count)
        {
            string command = "SELECT z.id_zamowienia, z. id_uzytkownik, z.id_ksiazka, k.autor, k.tytul, z.termin_wypozyczenia, z.termin_odbioru, z.termin_oddania, z.ilosc_przedluzen " +
                "FROM zamowienia z, ksiazki k WHERE z.id_uzytkownik = " + user1.Id +
                " AND z.id_ksiazka = k.id_ksiazka ORDER BY termin_wypozyczenia" +
                " OFFSET " + (page * count) + " ROW FETCH NEXT " + count + " ROWS ONLY";

            DataTable dt = GetDataTable(command);

            List<BorrowedBook> borrowedBooks = new List<BorrowedBook>();
            foreach (DataRow row in dt.Rows)
            {
                BorrowedBook borrowedBook = new BorrowedBook();
                borrowedBook.Id = row["id_zamowienia"] as Nullable<decimal>;
                borrowedBook.IdUser = row["id_uzytkownik"] as Nullable<decimal>;
                borrowedBook.IdBook = row["id_ksiazka"] as Nullable<decimal>;
                borrowedBook.Autor = row["autor"] as string;
                borrowedBook.Tytul = row["tytul"] as string;

                Nullable<DateTime> dataWypozyczenia = row["termin_wypozyczenia"] as Nullable<DateTime>;
                borrowedBook.DataWypozyczenia = dataWypozyczenia == null ? null : dataWypozyczenia.Value.ToString("dd-MM-yyyy");

                Nullable<DateTime> dataOdbioru = row["termin_odbioru"] as Nullable<DateTime>;
                borrowedBook.DataOdbioru = dataOdbioru == null ? null : dataOdbioru.Value.ToString("dd-MM-yyyy");

                Nullable<DateTime> dataOddania = row["termin_oddania"] as Nullable<DateTime>;
                borrowedBook.DataOddania = dataOddania == null ? null : dataOddania.Value.ToString("dd-MM-yyyy");

                borrowedBook.IloscPrzedluzen = row["ilosc_przedluzen"] as Nullable<decimal>;

                borrowedBooks.Add(borrowedBook);
            }

            return borrowedBooks;
        }

        public static int GetBorrowedBooksCount(User user1)
        {
            string command = "SELECT COUNT(*) FROM zamowienia WHERE id_uzytkownik = " + user1.Id;
            int count = Convert.ToInt32(DoScalar(command));
            return count;
        }

        public static void BorrowBooks(User user1, List<Book> books)
        {
            List<string> commands = new List<string>();

            String borrowDate = DateTime.Now.ToString("yy-MM-dd");
            foreach (Book book in books)
            {
                commands.Add(
                    "INSERT INTO zamowienia (id_uzytkownik, id_ksiazka, termin_wypozyczenia, termin_odbioru, termin_oddania, ilosc_przedluzen)" +
                    " VALUES (" + user1.Id + ", " + book.Id + ", '" + borrowDate + "', null, null, 0)"
                );


                commands.Add(
                    "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                    " VALUES (" + user1.Id + ", 'wypozyczenie id_ksiazka = " + book.Id + "', '" + borrowDate + "')"
                );
            }

            DoTransaction(commands);
        }

        public static bool IsBorrowAccepted(BorrowedBook borrowedBook)
        {
            string command = "SELECT termin_odbioru FROM zamowienia WHERE id_zamowienia = " + borrowedBook.Id;

            Nullable<DateTime> dataOdbioru = DoScalar(command) as Nullable<DateTime>;
            return dataOdbioru != null;
        }

        public static void AcceptBorrow(BorrowedBook borrowedBook)
        {
            List<string> commands = new List<string>();

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "UPDATE zamowienia SET termin_odbioru = '" + date + "' WHERE id_zamowienia = " + borrowedBook.Id
            );

            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + borrowedBook.IdUser + ", 'odebranie id_zamowienia = " + borrowedBook.Id + "', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static bool IsBorrowReturned(BorrowedBook borrowedBook)
        {
            string command = "SELECT termin_oddania FROM zamowienia WHERE id_zamowienia = " + borrowedBook.Id;

            Nullable<DateTime> dataOddania = DoScalar(command) as Nullable<DateTime>;
            return dataOddania != null;
        }

        public static void ReturnBorrow(BorrowedBook borrowedBook)
        {
            List<string> commands = new List<string>();

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "UPDATE zamowienia SET termin_oddania = '" + date + "' WHERE id_zamowienia = " + borrowedBook.Id
            );

            commands.Add(
                "UPDATE stan_ksiazek SET ilosc_dostepna = ilosc_dostepna + 1 WHERE id_ksiazka = " + borrowedBook.IdBook
            );

            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + borrowedBook.IdUser + ", 'oddanie id_zamowienia = " + borrowedBook.Id + "', '" + date + "')"
            );

            DoTransaction(commands);
        }

        public static int BorrowExtendsCount(BorrowedBook borrowedBook)
        {
            string command = "SELECT ilosc_przedluzen FROM zamowienia WHERE id_zamowienia = " + borrowedBook.Id;
            int count = Convert.ToInt32(DoScalar(command));
            return count;
        }

        public static void ExtendBorrow(BorrowedBook borrowedBook)
        {
            List<string> commands = new List<string>();

            String date = DateTime.Now.ToString("yy-MM-dd");
            commands.Add(
                "UPDATE zamowienia SET ilosc_przedluzen = ilosc_przedluzen + 1 WHERE id_zamowienia = " + borrowedBook.Id
            );

            commands.Add(
                "INSERT INTO logi (id_uzytkownik, operacja, data_wykonania)" +
                " VALUES (" + borrowedBook.IdUser + ", 'przedluzenie id_zamowienia = " + borrowedBook.Id + "', '" + date + "')"
            );

            DoTransaction(commands);
        }

        private static DataTable GetDataTable(string cmd)
        {
            DataTable dataTable = new DataTable();

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.CommandText = cmd;

                OracleDataAdapter adapter = new OracleDataAdapter(command);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        private static void DoQuery(string cmd)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.CommandText = cmd;
                command.ExecuteNonQuery();
            }
        }

        private static object DoScalar(string cmd)
        {
            object value = null;

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.CommandText = cmd;
                value = command.ExecuteScalar();
            }

            return value;
        }

        private static void DoTransaction(List<string> cmds)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Transaction = transaction;

                try
                {
                    foreach (string cmd in cmds)
                    {
                        command.CommandText = cmd;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
