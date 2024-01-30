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
        // SAKER ATT GÖRA:
        //
        // - 1. - Se till att indexeringen är klar och att det räcker
        // - 2. - Se till att en borrow_period skapas och läggs till i historiken när man lämnar tillbaka bok
        // - 3. - Fixa funktion för att ta bort och redigera customers
        // - 4. - Fixa en thread som sover till Deadline är nära och skickar en notis till customer
        // - 5. - Skapa en förklaring till varför jag har skapat databasen så som jag har gjort. 
        // - 6. - Kolla över 3NF så att det räcker till: Fungerar min bool i borrowPeriod? Jag tycker själv att den är individuell, men men...

        // Saker jag behöver lösa för att få G:
        //
        // - CHECK - Ett LinkTable (customer_has_book, där man länkar två tabeller). 
        // - CHECK - Update-function (uppdatera befintlig data)
        // - CHECK - addBook / addCustomer (lägga till data)
        // - CHECK - Söka efter information i databasen (sökfältet) 
        // - CHECK - Ta bort data från databasen

        // Saker jag behöver lösa för att få VG:
        //
        // - CHECK - Indexering på en kolumn som används för att söka efter specifika rader
        // - CHECK - Minst en VIEW
        // - CHECK - Användare med olika grants
        // - CHECK - Databasen ska vara i minst 3NF
        // - CHECK - Minst en STORED PROCEDURE som ska användas i programmet


        DatabaseConnection databaseConnection = new DatabaseConnection();
        Dictionary<int, Book> books = new Dictionary<int, Book>();
        List<Book> bookList = new List<Book>();
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        List<Customer> customerList = new List<Customer>();
        Dictionary<int, Book> searchedBooks = new Dictionary<int, Book>();
        List<Book> searchedBookList = new List<Book>();
        List<BorrowPeriod> borrowPeriodList = new List<BorrowPeriod>();
        List<CustomerBorrowedBooks> customerBorrowedBooksList = new List<CustomerBorrowedBooks>();

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
            MessageBox.Show("For admin login: \nadmin\nadmin\n\nFor regular user login:\nuser\nuser");
        }


        //SEARCH BAR FUNCTIONS:

        //Removes the text in the search-field
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        //Searches for books when there is input in textbox
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

        //Button for book-view
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bookCanvas.Visibility = Visibility.Visible;
            borrowBtn.Visibility = Visibility.Visible;
            borrowIcon.Visibility = Visibility.Visible;
            borrowPeriodCanvas.Visibility = Visibility.Hidden;
            returnBtn.Visibility = Visibility.Hidden;
            returnIcon.Visibility = Visibility.Hidden;
            customerCanvas.Visibility = Visibility.Hidden;
            addCanvas.Visibility = Visibility.Hidden;
            editCanvas.Visibility = Visibility.Hidden;
            currentBooksCanvas.Visibility = Visibility.Hidden;
        }

        //Button for customer-view
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            customerCanvas.Visibility = Visibility.Visible;
            bookCanvas.Visibility = Visibility.Hidden;
            addCanvas.Visibility = Visibility.Hidden;
            editCanvas.Visibility = Visibility.Hidden;
            borrowPeriodCanvas.Visibility = Visibility.Hidden;
            currentBooksCanvas.Visibility = Visibility.Hidden;
        }

        //Button for current loans-view
        private void currentLoansBtn_Click(object sender, RoutedEventArgs e)
        {
            currentBooksLB.Items.Refresh();
            currentBooksCanvas.Visibility = Visibility.Visible;
            returnBtn.Visibility = Visibility.Visible;
            returnIcon.Visibility = Visibility.Visible;
            borrowBtn.Visibility = Visibility.Hidden;
            borrowPeriodCanvas.Visibility = Visibility.Hidden;
            borrowIcon.Visibility = Visibility.Hidden;
            addCanvas.Visibility = Visibility.Hidden;
            editCanvas.Visibility = Visibility.Hidden;
            customerCanvas.Visibility = Visibility.Hidden;
            bookCanvas.Visibility = Visibility.Hidden;
        }

        private void menuBorrowHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            borrowPeriodsLB.Items.Refresh();
            borrowPeriodCanvas.Visibility = Visibility.Visible;
            currentBooksCanvas.Visibility = Visibility.Hidden;
            returnBtn.Visibility = Visibility.Hidden;
            returnIcon.Visibility = Visibility.Hidden;
            borrowBtn.Visibility = Visibility.Hidden;
            borrowIcon.Visibility = Visibility.Hidden;
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
            //Här tänkte jag lägga till en searchCustomer senare. (NOT YET IMPLEMENTED)
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

        //Functions to delete a book from library
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bookCanvas.Visibility == Visibility.Visible)
            {
                try
                {
                    int bookIndexToDelete = bookListBox.SelectedIndex;
                    if (bookIndexToDelete != -1)
                    {
                        Book book = bookList[bookIndexToDelete];
                        bookListBox.SelectedIndex = -1;
                        bool success = databaseConnection.DeleteBook(book.Id);
                        if (success)
                        {
                            txtInput.Text = "";
                            bookList.Remove(book);
                            books.Remove(book.Id);
                            searchedBookList.Remove(book);
                            searchedBooks.Remove(book.Id);
                            books = databaseConnection.GetBooks();
                            bookList = books.Values.ToList();
                            bookListBox.ItemsSource = books.Values;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("You don't have permission to delete books from the database");
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


        //Button to edit book in library
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
                //Yet to be implemented...
            }
        }


        //Button for add-view
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addCanvas.Visibility == Visibility.Hidden || addCanvas.Visibility == Visibility.Visible)
            {
                addCanvas.Visibility = Visibility.Visible;
                customerCanvas.Visibility = Visibility.Hidden;
                bookCanvas.Visibility = Visibility.Hidden;
            }
        }

        //Button to commit the added book
        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = titleTB.Text;
            string author = authorTB.Text;
            bool available = true;
            Book book = databaseConnection.AddNewBook(title, author, available);
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            bookListBox.Items.Refresh();
            titleTB.Text = "";
            authorTB.Text = "";
        }

        //Button to commit the edited book
        private void editBookBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = titleEditTB.Text;
            string author = authorEditTB.Text;
            int row = databaseConnection.EditBook(selectedId, title, author);
            books = databaseConnection.GetBooks();
            bookList = books.Values.ToList();
            bookListBox.ItemsSource = books.Values;
            titleEditTB.Text = "";
            authorEditTB.Text = "";
        }

        //The user chooses a book and borrows it. 
        //A borrow period of a month is created in AssignBookToCustomer();
        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = bookList[bookListBox.SelectedIndex];
                int bookKey = book.Id;
                int customerKey = loggedInCustomer.Id;
                if (book.Available == true)
                {
                    databaseConnection.AssignBookToCustomer(bookKey, customerKey); //HERE I ALSO NEED TO CHANGE THE AVAILABILITY IN THE DATABASE !!
                    CustomerBorrowedBooks cbd = new CustomerBorrowedBooks(customerKey, book.Title, DateTime.Now, DateTime.Now.AddMonths(+1), false);
                    book.Available = false;
                    customerBorrowedBooksList.Add(cbd);
                    currentBooksLB.Items.Refresh();
                    bookListBox.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("This book is already borrowed");
                }
            }
            catch
            {
                MessageBox.Show("You have to select a book");
            }
        }

        //Button to return a book that user has borrowed.
        private void returnBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerBorrowedBooks cbb = customerBorrowedBooksList[currentBooksLB.SelectedIndex];
                string cbbBookTitle = cbb.Book;
                int customerKey = loggedInCustomer.Id;
                foreach (Book book in bookList)
                {
                    if (book.Title == cbbBookTitle)
                    {
                        int bookKey = book.Id;
                        customerBorrowedBooksList.Remove(cbb);
                        book.Available = true;
                        currentBooksLB.Items.Refresh();
                        currentBooksLB.SelectedIndex = -1;
                        databaseConnection.CustomerReturnsBook(bookKey, customerKey);
                        borrowPeriodList = databaseConnection.GetBorrowPeriods(customerKey);
                        borrowPeriodsLB.ItemsSource = borrowPeriodList;
                        borrowPeriodsLB.Items.Refresh();
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("You have to select a book");
            }
        }

        //Button for login
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = emailLoginTB.Text;
            string password = passwordLoginTB.Password;
            foreach (Customer cus in customerList)
            {
                if (email == cus.Email)
                {
                    loggedInCustomer = cus;
                    customerBorrowedBooksList = databaseConnection.GetCustomerBorrowedBooks(loggedInCustomer.Id);
                    currentBooksLB.ItemsSource = customerBorrowedBooksList;
                    currentBooksLB.Items.Refresh();
                    borrowPeriodList = databaseConnection.GetBorrowPeriods(loggedInCustomer.Id);
                    borrowPeriodsLB.ItemsSource = borrowPeriodList;
                    borrowPeriodsLB.Items.Refresh();

                    if (loggedInCustomer.Admin == true)
                    {
                        databaseConnection = new DatabaseConnection(true);
                        AdminButtonReveal();
                    }
                    loginCanvas.Visibility = Visibility.Hidden;
                    bookCanvas.Visibility = Visibility.Visible;
                    ShowMenu();
                    return;
                }
            }
            MessageBox.Show("Wrong credentials, try again.");
        }

        //Function to show the menus after login
        private void ShowMenu()
        {
            menuBooksBtn.Visibility = Visibility.Visible;
            menuBorrowHistoryBtn.Visibility = Visibility.Visible;
            menuCustomersBtn.Visibility = Visibility.Visible;
            currentLoansBtn.Visibility = Visibility.Visible;
            txtInput.Visibility = Visibility.Visible;
            tbPlaceHolder.Visibility = Visibility.Visible;
            btnClear.Visibility = Visibility.Visible;
            borrowBtn.Visibility = Visibility.Visible;
            borrowIcon.Visibility = Visibility.Visible;
            saveBtn.Visibility = Visibility.Visible;
            saveIcon.Visibility = Visibility.Visible;
        }

        //Function to show the admin-buttons after login
        private void AdminButtonReveal()
        {
            deleteBtn.Visibility = Visibility.Visible;
            editBtn.Visibility = Visibility.Visible;
            addBtn.Visibility = Visibility.Visible;
            deleteIcon.Visibility = Visibility.Visible;
            editIcon.Visibility = Visibility.Visible;
            addIcon.Visibility = Visibility.Visible;
        }
    }
}
