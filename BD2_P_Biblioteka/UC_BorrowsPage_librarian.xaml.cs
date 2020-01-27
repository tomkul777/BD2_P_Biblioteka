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
    public partial class UC_BorrowsPage_librarian : UserControl
    {
        int page = 0;
        int countPerPage = 7;

        public UC_BorrowsPage_librarian()
        {
            InitializeComponent();

            DatabaseManager.user_other = null;

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
            column4.Width = 80;
            dgBorrows.Columns.Add(column4);

            DataGridTextColumn column5 = new DataGridTextColumn();
            column5.Header = "Data oddania";
            column5.Binding = new Binding("DataOddania");
            column5.Width = 80;
            dgBorrows.Columns.Add(column5);

            DataGridTextColumn column6 = new DataGridTextColumn();
            column6.Header = "Ilość przedluzen";
            column6.Binding = new Binding("IloscPrzedluzen");
            column6.Width = 90;
            dgBorrows.Columns.Add(column6);

            DataTemplate dataTemplate = new DataTemplate();
            FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Button.ContentProperty, "X");
            button.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnGetEvent));
            dataTemplate.VisualTree = button;
            DataGridTemplateColumn column7 = new DataGridTemplateColumn();
            column7.CellTemplate = dataTemplate;
            column7.Width = 50;
            dgBorrows.Columns.Add(column7);

            DataTemplate dataTemplate1 = new DataTemplate();
            FrameworkElementFactory button1 = new FrameworkElementFactory(typeof(Button));
            button1.SetValue(Button.ContentProperty, "X");
            button1.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnReturnEvent));
            dataTemplate1.VisualTree = button1;
            DataGridTemplateColumn column8 = new DataGridTemplateColumn();
            column8.CellTemplate = dataTemplate1;
            column8.Width = 50;
            dgBorrows.Columns.Add(column8);

            DataTemplate dataTemplate2 = new DataTemplate();
            FrameworkElementFactory button2 = new FrameworkElementFactory(typeof(Button));
            button2.SetValue(Button.ContentProperty, "+");
            button2.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnExtendEvent));
            dataTemplate2.VisualTree = button2;
            DataGridTemplateColumn column9 = new DataGridTemplateColumn();
            column9.CellTemplate = dataTemplate2;
            column9.Width = 50;
            dgBorrows.Columns.Add(column9);
        }

        public void UpdateBorrows()
        {
            dgBorrows.Items.Clear();

            if (DatabaseManager.user_other == null) return;

            List<BorrowedBook> borrowedBooks = DatabaseManager.GetBorrowedBooks(DatabaseManager.user_other, page, countPerPage);
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
            if (DatabaseManager.user_other == null) return;

            UpdatePage(0);
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseManager.user_other == null) return;

            double pageRatio = DatabaseManager.GetBorrowedBooksCount(DatabaseManager.user_other) / ((double)(countPerPage + 1));
            UpdatePage((int)pageRatio);
        }

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseManager.user_other == null) return;
            if (page < 1) return;

            UpdatePage(page - 1);
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if(DatabaseManager.user_other == null) return;

            double pageRatio = DatabaseManager.GetBorrowedBooksCount(DatabaseManager.user_other) / ((double)(countPerPage + 1));
            if (page >= (int)pageRatio) return;

            UpdatePage(page + 1);
        }

        private void BtnSearchId_Click(object sender, RoutedEventArgs e)
        {
            dgBorrows.Items.Clear();
            lblEmail.Content = "";

            int id;
            if(!int.TryParse(tbId.Text, out id))
            {
                return;
            }

            DatabaseManager.GetUser(id);
            UpdateBorrows();
            if(DatabaseManager.user_other != null) lblEmail.Content = DatabaseManager.user_other.Email;
        }

        private void BtnGetEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            BorrowedBook borrowedBook = ((FrameworkElement)sender).DataContext as BorrowedBook;
            if (DatabaseManager.IsBorrowAccepted(borrowedBook)) return;
            DatabaseManager.AcceptBorrow(borrowedBook);
            UpdateBorrows();
        }

        private void BtnReturnEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            BorrowedBook borrowedBook = ((FrameworkElement)sender).DataContext as BorrowedBook;
            if (DatabaseManager.IsBorrowReturned(borrowedBook)) return;
            DatabaseManager.ReturnBorrow(borrowedBook);
            UpdateBorrows();
        }

        private void BtnExtendEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            BorrowedBook borrowedBook = ((FrameworkElement)sender).DataContext as BorrowedBook;
            if (!DatabaseManager.IsBorrowAccepted(borrowedBook)) return;
            if (DatabaseManager.IsBorrowReturned(borrowedBook)) return;
            if (DatabaseManager.BorrowExtendsCount(borrowedBook) >= 2) return;
            DatabaseManager.ExtendBorrow(borrowedBook);
            UpdateBorrows();
        }
    }
}
