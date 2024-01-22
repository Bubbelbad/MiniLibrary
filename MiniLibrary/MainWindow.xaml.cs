using MiniLibrary.Classes;
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

namespace MiniLibrary
{
    public partial class MainWindow : Window
    {
        // 2. Koppla ihop addBook med funktionen
        // 3. Kolla vad som behövs för att få G och gör de uppgifterna.
        // 4. Läs på om users och deras befogenheter. De ska kunna få rättigheter direkt i databasen. 
        // 5. Börja jobba på UserControls och tänk efter hur programmet ska se ut nu ordentligt. 



        // Saker jag behöver lösa för att få G:
        //
        // - addBook / addCustomer (lägga till data)
        // - Update-function (uppdatera befintlig data)
        // - Ett LinkTable (customer_has_book, där man länkar två tabeller). 
        // - CHECK - Söka efter information i databasen (sökfältet) 
        // - CHECK - Ta bort data från database

        // Saker jag behöver lösa för att få VG:
        //
        // - Minst en VIEW och en STORED PROCEDURE som ska användas i programmet
        // - Användare som bara har tillåtelse att använda CRUD
        // - Indexering på en kolumn som används för att söka efter specifika rader
        // - Databasen ska vara i minst 3NF


        DatabaseConnection databaseConnection = new DatabaseConnection();

        Dictionary<int, Book> books = new Dictionary<int, Book>();
        List<Book> bookList = new List<Book>();
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        List<Customer> customerList = new List<Customer>();
        Dictionary<int, Book> searchedBooks = new Dictionary<int, Book>();
        List<Book> searchedBookList = new List<Book>();

        public MainWindow()
        {
            InitializeComponent();
            LibraryProgram libraryProgram = new LibraryProgram();
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            bookListBox.Items.Refresh();

            customers = databaseConnection.GetCustomers();
            customerList = customers.Values.ToList();
            customerListBox.ItemsSource = customers.Values;
            customerListBox.Items.Refresh();

            Console.WriteLine("Hello world");
        }



        //Search bar functions:
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                tbPlaceHolder.Visibility = Visibility.Visible;
                bookListBox.ItemsSource = books.Values;
                bookListBox.Items.Refresh();
            }
            else
            {
                tbPlaceHolder.Visibility = Visibility.Hidden;
                txtInput.Foreground = Brushes.White;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            customerCanvas.Visibility = Visibility.Visible;
            bookCanvas.Visibility = Visibility.Hidden;
            addCanvas.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            customerCanvas.Visibility = Visibility.Hidden;
            bookCanvas.Visibility = Visibility.Visible;
            addCanvas.Visibility = Visibility.Hidden;
        }

        //Söka efter böcker: FEL EVENT !
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (bookCanvas.Visibility == Visibility.Visible)
            {
                customerCanvas.Visibility = Visibility.Hidden;
                addCanvas.Visibility = Visibility.Hidden;
                bookCanvas.Visibility = Visibility.Visible;
                string search = txtInput.Text;
                searchedBooks = databaseConnection.SearchBooks(search);
                searchedBookList = searchedBooks.Values.ToList();
                bookListBox.ItemsSource = searchedBooks.Values;
                bookListBox.Items.Refresh();
            }
            //Här tänkte jag lägga till en searchCustomer senare. 
            else if (customerCanvas.Visibility == Visibility.Visible)
            {
                customerCanvas.Visibility = Visibility.Hidden;
                addCanvas.Visibility = Visibility.Hidden;
                bookCanvas.Visibility = Visibility.Visible;
                string search = txtInput.Text;
                searchedBooks = databaseConnection.SearchBooks(search);
                searchedBookList = searchedBooks.Values.ToList();
                bookListBox.ItemsSource = searchedBooks.Values;
                bookListBox.Items.Refresh();
            }
            
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bookCanvas.Visibility == Visibility.Visible)
            {
                int bookIndexToDelete = bookListBox.SelectedIndex;
                if (bookIndexToDelete != -1)
                {
                    Book book = bookList[bookIndexToDelete];
                    bookListBox.SelectedIndex = -1;
                    bool success = databaseConnection.DeleteBook(book.Id);

                    if(success)
                    {
                        txtInput.Text = "";
                        bookList.Remove(book);
                        books.Remove(book.Id);
                        searchedBookList.Remove(book);
                        searchedBooks.Remove(book.Id);
                        books = databaseConnection.GetBooks();
                        bookList = books.Values.ToList();
                        bookListBox.ItemsSource = books.Values;
                        bookListBox.Items.Refresh();
                    }
                }
            } 
            else if (customerCanvas.Visibility == Visibility.Visible)
            {
                int customerIndexToDelete = customerListBox.SelectedIndex;
                if (customerIndexToDelete != -1)
                {
                    Customer customer = customerList[customerIndexToDelete];
                    customerListBox.SelectedIndex = -1;
                    bool success = databaseConnection.DeleteCustomer(customer.Id);

                    if (success)
                    {
                        customerList.Remove(customer);
                        customers.Remove(customer.Id);

                        customers = databaseConnection.GetCustomers();
                        customerList = customers.Values.ToList();
                        customerListBox.ItemsSource = customers.Values;
                        customerListBox.Items.Refresh();
                    }
                }
            }
            
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bookCanvas.Visibility == Visibility.Visible)
            {
                int customerIndexToEdit = customerListBox.SelectedIndex;
                if (customerIndexToEdit != -1)
                {
                    Customer customer = customerList[customerIndexToEdit];

                }
            }


            else if (customerCanvas.Visibility == Visibility.Visible)
            {

            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addCanvas.Visibility == Visibility.Hidden || addCanvas.Visibility == Visibility.Visible)
            {
                addCanvas.Visibility = Visibility.Visible;
                customerCanvas.Visibility = Visibility.Hidden;
                bookCanvas.Visibility = Visibility.Hidden;
            }

            //Here I need to open a window / usercontrol to get the arguments for AddBook-function
           // string title = null;
           // string author = null;
           // Book book = databaseConnection.AddNewBook(title, author);
           // books.Add(TabIndex, book);
        }
    }
}
