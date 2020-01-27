using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class UC_BooksCatalog : UserControl
    {
        int page = 0;
        int countPerPage = 7;

        public UC_BooksCatalog()
        {
            InitializeComponent();

            ConfigureBooksCatalog();
            UpdateBooksCatalog();
        }

        private void ConfigureBooksCatalog()
        {
            dgBooksCatalog.IsReadOnly = true;
            dgBooksCatalog.CanUserResizeColumns = false;
            dgBooksCatalog.CanUserResizeRows = false;
            dgBooksCatalog.CanUserReorderColumns = false;
            dgBooksCatalog.HorizontalAlignment = HorizontalAlignment.Center;
            dgBooksCatalog.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBooksCatalog.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dgBooksCatalog.RowHeight = 50;
            dgBooksCatalog.FontSize = 18;

            DataGridTextColumn column1 = new DataGridTextColumn();
            column1.Header = "Autor";
            column1.Binding = new Binding("Autor");
            column1.Width = 200;
            dgBooksCatalog.Columns.Add(column1);

            DataGridTextColumn column2 = new DataGridTextColumn();
            column2.Header = "Tytuł";
            column2.Binding = new Binding("Tytul");
            column2.Width = 200;
            dgBooksCatalog.Columns.Add(column2);

            DataGridTextColumn column3 = new DataGridTextColumn();
            column3.Header = "Wydawnictwo";
            column3.Binding = new Binding("Wydawnictwo");
            column3.Width = 130;
            dgBooksCatalog.Columns.Add(column3);

            DataGridTextColumn column4 = new DataGridTextColumn();
            column4.Header = "Rok wydania";
            column4.Binding = new Binding("RokWydania");
            column4.Width = 120;
            dgBooksCatalog.Columns.Add(column4);

            DataGridTextColumn column5 = new DataGridTextColumn();
            column5.Header = "Opis";
            column5.Binding = new Binding("Opis");
            column5.Width = 210;
            dgBooksCatalog.Columns.Add(column5);

            DataGridTextColumn column6 = new DataGridTextColumn();
            column6.Header = "Ilość";
            column6.Binding = new Binding("Ilosc");
            column6.Width = 50;
            dgBooksCatalog.Columns.Add(column6);

            if (DatabaseManager.user != null && DatabaseManager.user.levelAccess == "Bibliotekarz") return;
            
            DataTemplate dataTemplate = new DataTemplate();
            FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
            FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.SourceProperty, new BitmapImage(new Uri("pack://application:,,,/basket.jpg")));
            button.AppendChild(image);
            button.AddHandler(Button.ClickEvent, new RoutedEventHandler(BtnClickEvent));
            dataTemplate.VisualTree = button;
            DataGridTemplateColumn column7 = new DataGridTemplateColumn();
            column7.CellTemplate = dataTemplate;
            column7.Width = 50;
            dgBooksCatalog.Columns.Add(column7);
        }

        public void UpdateBooksCatalog()
        {
            dgBooksCatalog.Items.Clear();

            List<Book> books = DatabaseManager.GetBooksCatalog(page, countPerPage);
            foreach (Book book in books)
            {
                dgBooksCatalog.Items.Add(book);
            }
        }

        public void UpdatePage(int page)
        {
            this.page = page;
            lblPage.Content = page + 1;
            UpdateBooksCatalog();
        }

        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            UpdatePage(0);
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBooksCount() / ((double)(countPerPage + 1));
            UpdatePage((int)pageRatio);
        }

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (page < 1) return;

            UpdatePage(page - 1);
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            double pageRatio = DatabaseManager.GetBooksCount() / ((double)(countPerPage + 1));
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
            DatabaseManager.AddToBasket(DatabaseManager.user, book);
            mainWindow.ShowBooksCatalog();
        }
    }
}
