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
        DatabaseConnection databaseConnection = new DatabaseConnection();

        Dictionary<int, Book> books = new Dictionary<int, Book>();
        List<Book> bookList = new List<Book>();
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        List<Customer> customerList = new List<Customer>();

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
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            customerCanvas.Visibility = Visibility.Hidden;
            bookCanvas.Visibility = Visibility.Visible;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            customerCanvas.Visibility = Visibility.Hidden;
            bookCanvas.Visibility = Visibility.Visible;
           // databaseConnection.
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
                        bookList.Remove(book);
                        books.Remove(book.Id);
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
    }
}
