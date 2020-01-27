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
    public partial class UC_Basket : UserControl
    {
        int page = 0;
        int countPerPage = 7;

        public UC_Basket()
        {
            InitializeComponent();

            ConfigureBasket();
            UpdatePage(0);
        }

        public UC_Basket(int page)
        {
            InitializeComponent();

            ConfigureBasket();
            UpdatePage(page);
        }

        private void ConfigureBasket()
        {
            dgBasket.IsReadOnly = true;
            dgBasket.CanUserResizeColumns = false;
            dgBasket.CanUserResizeRows = false;
            dgBasket.CanUserReorderColumns = false;
            dgBasket.HorizontalAlignment = HorizontalAlignment.Center;
            dgBasket.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBasket.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBasket.RowHeight = 50;
            dgBasket.FontSize = 18;

            DataGridTextColumn column1 = new DataGridTextColumn();
            column1.Header = "Autor";
            column1.Binding = new Binding("Autor");
            column1.Width = 200;
            dgBasket.Columns.Add(column1);

            DataGridTextColumn column2 = new DataGridTextColumn();
            column2.Header = "Tytuł";
            column2.Binding = new Binding("Tytul");
            column2.Width = 200;
            dgBasket.Columns.Add(column2);

            DataGridTextColumn column3 = new DataGridTextColumn();
            column3.Header = "Wydawnictwo";
            column3.Binding = new Binding("Wydawnictwo");
            column3.Width = 130;
            dgBasket.Columns.Add(column3);

            DataGridTextColumn column4 = new DataGridTextColumn();
            column4.Header = "Rok wydania";
            column4.Binding = new Binding("RokWydania");
            column4.Width = 120;
            dgBasket.Columns.Add(column4);

            DataGridTextColumn column5 = new DataGridTextColumn();
            column5.Header = "Opis";
            column5.Binding = new Binding("Opis");
            column5.Width = 260;
            dgBasket.Columns.Add(column5);

            DataTemplate dataTemplate = new DataTemplate();
            FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Button.ContentProperty, "X");
            button.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnClickEvent));
            dataTemplate.VisualTree = button;
            DataGridTemplateColumn column7 = new DataGridTemplateColumn();
            column7.CellTemplate = dataTemplate;
            column7.Width = 50;
            dgBasket.Columns.Add(column7);
        }

        public void UpdateBasket()
        {
            dgBasket.Items.Clear();

            List<Book> books = DatabaseManager.GetBasketCatalog(DatabaseManager.user, page, countPerPage);
            foreach (Book book in books)
            {
                dgBasket.Items.Add(book);
            }
        }

        public void UpdatePage(int page)
        {
            this.page = page;
            lblPage.Content = page + 1;
            UpdateBasket();
        }

        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            UpdatePage(0);
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBasketCount(DatabaseManager.user) / ((double)(countPerPage + 1));
            UpdatePage((int)pageRatio);
        }

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (page < 1) return;

            UpdatePage(page - 1);
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBasketCount(DatabaseManager.user) / ((double)(countPerPage + 1));
            if (page >= (int)pageRatio) return;

            UpdatePage(page + 1);
        }

        private void BtnClickEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            Book book = ((FrameworkElement)sender).DataContext as Book;
            DatabaseManager.RemoveFromBasket(DatabaseManager.user, book);
            mainWindow.ShowBasket(page);
        }

        private void BtnBorrow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            User user = DatabaseManager.user;
            if (user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            DatabaseManager.BorrowBooks(user, DatabaseManager.GetBasketCatalog(DatabaseManager.user, 0, DatabaseManager.GetBasketCount(user)));
            DatabaseManager.ClearBasketCatalog(DatabaseManager.user, false);
            mainWindow.ShowBasket(page);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (DatabaseManager.user == null)
            {
                mainWindow.ShowLoginPage();
                return;
            }

            DatabaseManager.ClearBasketCatalog(DatabaseManager.user, true);
            mainWindow.ShowBasket(page);
        }
    }
}
