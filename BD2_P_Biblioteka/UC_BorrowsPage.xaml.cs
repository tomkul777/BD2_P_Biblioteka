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
    public partial class UC_BorrowsPage : UserControl
    {
        int page = 0;
        int countPerPage = 7;

        public UC_BorrowsPage()
        {
            InitializeComponent();

            ConfigureBorrowsCatalog();
            UpdatePage(0);
        }

        private void ConfigureBorrowsCatalog()
        {
            dgBorrows.IsReadOnly = true;
            dgBorrows.CanUserResizeColumns = false;
            dgBorrows.CanUserResizeRows = false;
            dgBorrows.CanUserReorderColumns = false;
            dgBorrows.HorizontalAlignment = HorizontalAlignment.Center;
            dgBorrows.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBorrows.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBorrows.RowHeight = 50;
            dgBorrows.FontSize = 18;

            DataGridTextColumn column1 = new DataGridTextColumn();
            column1.Header = "Autor";
            column1.Binding = new Binding("Autor");
            column1.Width = 200;
            dgBorrows.Columns.Add(column1);

            DataGridTextColumn column2 = new DataGridTextColumn();
            column2.Header = "Tytuł";
            column2.Binding = new Binding("Tytul");
            column2.Width = 200;
            dgBorrows.Columns.Add(column2);

            DataGridTextColumn column3 = new DataGridTextColumn();
            column3.Header = "Data wypozyczenia";
            column3.Binding = new Binding("DataWypozyczenia");
            column3.Width = 160;
            dgBorrows.Columns.Add(column3);

            DataGridTextColumn column4 = new DataGridTextColumn();
            column4.Header = "Data odbioru";
            column4.Binding = new Binding("DataOdbioru");
            column4.Width = 130;
            dgBorrows.Columns.Add(column4);

            DataGridTextColumn column5 = new DataGridTextColumn();
            column5.Header = "Data oddania";
            column5.Binding = new Binding("DataOddania");
            column5.Width = 130;
            dgBorrows.Columns.Add(column5);

            DataGridTextColumn column6 = new DataGridTextColumn();
            column6.Header = "Ilość przedluzen";
            column6.Binding = new Binding("IloscPrzedluzen");
            column6.Width = 140;
            dgBorrows.Columns.Add(column6);
        }

        public void UpdateBorrows()
        {
            dgBorrows.Items.Clear();

            List<BorrowedBook> borrowedBooks = DatabaseManager.GetBorrowedBooks(DatabaseManager.user, page, countPerPage);
            foreach (BorrowedBook borrowedBook in borrowedBooks)
            {
                dgBorrows.Items.Add(borrowedBook);
            }
        }

        public void UpdatePage(int page)
        {
            this.page = page;
            lblPage.Content = page + 1;
            UpdateBorrows();
        }

        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            UpdatePage(0);
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBorrowedBooksCount(DatabaseManager.user) / ((double)(countPerPage + 1));
            UpdatePage((int)pageRatio);
        }

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (page < 1) return;

            UpdatePage(page - 1);
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBorrowedBooksCount(DatabaseManager.user) / ((double)(countPerPage + 1));
            if (page >= (int)pageRatio) return;

            UpdatePage(page + 1);
        }
    }
}
