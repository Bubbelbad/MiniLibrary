using Microsoft.VisualBasic;
using MiniLibrary.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MiniLibrary
{
    class DatabaseConnection
    {
        string server = "localhost";
        string database = "MiniLibrary"; 
        string username = "user";
        string password = "password";

        string connectionString = "";

        public DatabaseConnection() 
        {
            connectionString =
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";";
        }

        public DatabaseConnection(bool admin)
        {
            if (admin == true)
            {
                connectionString =
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + "admin" + ";" +
                "PASSWORD=" + "admin" + ";";
            }
        }


        public Dictionary<int, Book> GetBooks()
        {
            Dictionary<int, Book> books = new Dictionary<int, Book>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "SELECT * FROM book;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Book book = new Book((int)reader["Id"], (string)reader["Title"], (string)reader["Author"], (bool)reader["available"]);
                books.Add(book.Id, book);
            }
            try
            {
                //Watch the catch
            }
            catch (Exception ex) { }
            connection.Close();
            return books;
        }

        public Dictionary<int, Customer> GetCustomers()
        {
            Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "SELECT * FROM customer;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Customer customer = new Customer((int)reader["id"], (string)reader["first_name"], (string)reader["last_name"], (string)reader["email"], (string)reader["customer_password"], (string)reader["state"], (bool)reader["admin"]);
                customers.Add(customer.Id, customer);
            }
            try
            {
                //Watch the catch
            }
            catch (Exception ex) { }
            connection.Close();
            return customers;
        }

        //Importing all the past borrow periods
        public List<BorrowPeriod> GetBorrowPeriods(int customerId)
        {
            List<BorrowPeriod> borrowPeriodList = new List<BorrowPeriod>();
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string query = "SELECT * FROM borrow_period " +
                           "WHERE customer_id = " + customerId + " && " + "is_returned = true;";
            MySqlCommand command = new MySqlCommand(@query, con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                BorrowPeriod bp = new BorrowPeriod((int)reader["id"], (DateTime)reader["start_time"], (DateTime)reader["end_time"], (bool)reader["is_returned"], (int)reader["book_id"], (int)reader["customer_id"]);
                borrowPeriodList.Add(bp);
            }
            con.Close();
            return borrowPeriodList;
        }

        public List<CustomerBorrowedBooks> GetCustomerBorrowedBooks(int id)
        {
            List<CustomerBorrowedBooks> customerBorrowedBooks = new List<CustomerBorrowedBooks>();
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string query = "SELECT * FROM customer_borrowed_books " + 
                           "WHERE id = " + id + ";";
            MySqlCommand command = new MySqlCommand(@query, con);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerBorrowedBooks cbb = new CustomerBorrowedBooks((int)reader["id"], (string)reader["Book"], (DateTime)reader["Start_Time"], (DateTime)reader["Deadline"], (bool)reader["Returned"]);
                customerBorrowedBooks.Add(cbb);
            }
            con.Close();
            return customerBorrowedBooks;
        }

        public void CustomerReturnsBook(int bookId, int customerId)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string query = "CALL customer_returns_book(@bookId, @customerId);";
            MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@bookId", bookId);
            command.Parameters.AddWithValue("@customerId", customerId);
            command.ExecuteNonQuery();
            con.Close();
        }

        public Book AddNewBook(string bookTitle, string bookAuthor, bool bookAvailable)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "CALL create_new_book(\"" + bookTitle + "\", \"" + bookAuthor + "\", \"" + bookAvailable + "\")";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int bookId = (int)reader["new_id"];
            Book book = new Book(bookId, bookTitle, bookAuthor, bookAvailable);
            connection.Close();
            return book;
        }

        public int EditBook(int bookId, string bookTitle, string bookAuthor)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "CALL edit_book(" + bookId + ", \"" + bookTitle + "\", \"" + bookAuthor + "\")";
            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected;
        }

        public Customer AddNewCustomer(string customerName, string customerLastname, string customerEmail, string customerPassword, string customerStatus, bool customerAdmin)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "CALL create_new_customer(\"" + customerName + "\", \"" + customerLastname + "\", \"" + "\", \"" + customerEmail + "\", \"" +  customerPassword + "\", \"" + customerStatus + "\", \"" + customerAdmin + "\")";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int customerId = (int)reader["new_id"];
            Customer customer = new Customer(customerId, customerName, customerLastname, customerEmail, customerPassword, customerStatus, customerAdmin);
            connection.Close();
            return customer;
        }

        public bool DeleteBook(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "DELETE FROM book " +
                           "WHERE id = " + id + ";";
            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }

        public bool DeleteCustomer(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "DELETE FROM customer " +
                           "WHERE id = " + id + ";";
            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected > 0;
        }

        public void AssignBookToCustomer(int bookKey, int customerKey)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();  
            string query = "CALL customer_borrows_book (" + bookKey + ", " + customerKey + ")";
            MySqlCommand command = new MySqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
        }

        public Dictionary<int, Book> SearchBooks(string search)
        {
            Dictionary<int, Book> books = new Dictionary<int, Book>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "SELECT * FROM book WHERE title LIKE \"%" + search +  "%\"";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Book book = new Book((int)reader["Id"], (string)reader["Title"], (string)reader["Author"], (bool)reader["Avaliable"]);
                books.Add(book.Id, book);
            }
            try
            {
                //Watch the catch
            }
            catch (Exception ex) { }
            connection.Close();
            return books;
        }
    }
}