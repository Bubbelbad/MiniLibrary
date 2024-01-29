using MiniLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        // 1. Läs på om users och deras befogenheter. De ska få lämpliga rättigheter direkt i databasen (Bara kolla igenon)
        // 2. - Fixa try/catch där normal user inte kan genomföra funktion


        // Saker jag behöver lösa för att få G:
        //
        // - Ett LinkTable (customer_has_book, där man länkar två tabeller). 
        //
        // - CHECK - Update-function (uppdatera befintlig data)
        // - CHECK - addBook / addCustomer (lägga till data)
        // - CHECK - Söka efter information i databasen (sökfältet) 
        // - CHECK - Ta bort data från databasen

        // Saker jag behöver lösa för att få VG:
        //
        // - Minst en VIEW
        // - Indexering på en kolumn som används för att söka efter specifika rader
        // - CHECK - Användare med olika grants
        // - CHECK - Databasen ska vara i minst 3NF
        // - CHECK - Minst en STORED PROCEDURE som ska användas i programmet

        // Saker som vore roligt att fixa:

        // - Säkerhet i systemet med transactions. Kanske kan jag göra transactions på ID med FK?


        DatabaseConnection databaseConnection = new DatabaseConnection();
        Dictionary<int, Book> books = new Dictionary<int, Book>();
        List<Book> bookList = new List<Book>();
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        List<Customer> customerList = new List<Customer>();
        Dictionary<int, Book> searchedBooks = new Dictionary<int, Book>();
        List<Book> searchedBookList = new List<Book>();
        Dictionary<int, BorrowPeriod> borrowPeriods = new Dictionary<int, BorrowPeriod>();
        List<BorrowPeriod> borrowPeriodList = new List<BorrowPeriod>();

        int selectedId = 1;
        Customer loggedInCustomer;

        public MainWindow()
        {
            InitializeComponent();
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            bookListBox.Items.Refresh();

            customers = databaseConnection.GetCustomers();
            customerList = customers.Values.ToList();
            customerListBox.ItemsSource = customers.Values;
            customerListBox.Items.Refresh();

            //Här hade jag tänkt att ta in resultatet från customer_borrowed_books. 
            //Men jag vill ju också som vanlig user barta kunna se mina, men som admin kunna se valfri users boklån. 
            // selectedId = databaseConnection.GetCurrentUser();
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
            editCanvas.Visibility = Visibility.Hidden;
            currentBooksCanvas.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bookCanvas.Visibility = Visibility.Visible;
            customerCanvas.Visibility = Visibility.Hidden;
            addCanvas.Visibility = Visibility.Hidden;
            editCanvas.Visibility = Visibility.Hidden;
            currentBooksCanvas.Visibility = Visibility.Hidden;
        }

        private void currentLoansBtn_Click(object sender, RoutedEventArgs e)
        {
            currentBooksLB.Items.Refresh();
            currentBooksCanvas.Visibility = Visibility.Visible;
            addCanvas.Visibility = Visibility.Hidden;
            editCanvas.Visibility = Visibility.Hidden;
            customerCanvas.Visibility = Visibility.Hidden;
            bookCanvas.Visibility = Visibility.Hidden;
        }

        //Söka efter böcker:
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (bookCanvas.Visibility == Visibility.Visible)
            {
                customerCanvas.Visibility = Visibility.Hidden;
                addCanvas.Visibility = Visibility.Hidden;
                currentBooksCanvas.Visibility = Visibility.Hidden;
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
            Book book = bookList[bookListBox.SelectedIndex];
            selectedId = book.Id;
            if (editCanvas.Visibility == Visibility.Hidden)
            {
                editCanvas.Visibility = Visibility.Visible;
                bookCanvas.Visibility = Visibility.Hidden;
                customerCanvas.Visibility = Visibility.Hidden;
                addCanvas.Visibility = Visibility.Hidden;
                currentBooksCanvas.Visibility = Visibility.Hidden;
                
                if (book != null)
                {
                    titleEditTB.Text = book.Title;
                    authorEditTB.Text = book.Author;
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

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = titleTB.Text;
            string author = authorTB.Text;
            Book book = databaseConnection.AddNewBook(title, author);
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            bookListBox.Items.Refresh();
            titleTB.Text = "";
            authorTB.Text = "";
        }

        private void editBookBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = titleEditTB.Text;
            string author = authorEditTB.Text;
            int row = databaseConnection.EditBook(selectedId, title, author);
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            bookListBox.Items.Refresh();
            titleEditTB.Text = "";
            authorEditTB.Text = "";
        }

        //The user chooses a book and borrows it. 
        //A borrow period of a month is created in AssignBookToCustomer();
        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {
            Book book = bookList[bookListBox.SelectedIndex];
            int bookKey = book.Id;
            int customerKey = selectedId;
            databaseConnection.AssignBookToCustomer(bookKey, customerKey);
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = emailLoginTB.Text;
            string password = passwordLoginTB.Text;
            foreach (Customer cus in customerList)
            {
                if (email == cus.Email)
                {
                    loggedInCustomer = cus;
                    borrowPeriods = databaseConnection.GetBorrowPeriods(loggedInCustomer.Id);
                    borrowPeriodList = borrowPeriods.Values.ToList();
                    currentBooksLB.ItemsSource = borrowPeriodList;
                    currentBooksLB.Items.Refresh();
                    if (loggedInCustomer.Admin == true)
                    {
                        databaseConnection = new DatabaseConnection(true);
                    }
                    loginCanvas.Visibility = Visibility.Hidden;
                    bookCanvas.Visibility = Visibility.Visible;
                    ShowMenu();
                    return;
                }
            }
            MessageBox.Show("Wrong credentials, try again.");
        }

        private void ShowMenu()
        {
            menuBooksBtn.Visibility = Visibility.Visible;
            menuBorrowHistoryBtn.Visibility = Visibility.Visible;
            menuCustomersBtn.Visibility = Visibility.Visible;
            currentLoansBtn.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            tbPlaceHolder.Visibility = Visibility.Visible;
            btnClear.Visibility = Visibility.Visible;

        }
    }
}
