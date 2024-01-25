using Microsoft.VisualBasic;
using MiniLibrary.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary
{
    class DatabaseConnection
    {
        string server = "localhost";
        string database = "MiniLibrary"; 
        string username = "root"; 
        string password = "";

        string connectionString;

        public DatabaseConnection()
        {
            connectionString =
                "SERVER=" + server + ";" +
                "DATABASE=" + database + ";" +
                "UID=" + username + ";" +
                "PASSWORD=" + password + ";";
            Console.WriteLine("ConnectionString: " + connectionString);
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
                Book book = new Book((int)reader["Id"], (string)reader["Title"], (string)reader["Author"]);
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
                Customer customer = new Customer((int)reader["id"], (string)reader["first_name"], (string)reader["last_name"], (string)reader["email"], (string)reader["customer_password"], (string)reader["state"]);
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

        public Book AddNewBook(string bookTitle, string bookAuthor)
        {
            MySqlConnection connection = new MySqlConnection (connectionString);
            connection.Open();
            string query = "CALL create_new_book(\"" + bookTitle + "\", \"" + bookAuthor + "\")";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int bookId = (int)reader["new_id"];
            Book book = new Book(bookId, bookTitle, bookAuthor);
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

        public Customer AddNewCustomer(string customerName, string customerLastname, string customerEmail, string customerPassword, string customerStatus)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "CALL create_new_customer(\"" + customerName + "\", \"" + customerLastname + "\", \"" + "\", \"" + customerEmail + "\", \"" +  customerPassword + "\", \"" + customerStatus + "\")";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int customerId = (int)reader["new_id"];
            Customer customer = new Customer(customerId, customerName, customerLastname, customerEmail, customerPassword, customerStatus);
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

            // Adding a new connection is as easy as adding the two keys together to the link table.    
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
                Book book = new Book((int)reader["Id"], (string)reader["Title"], (string)reader["Author"]);
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

        public void GetBorrowedBooks(int customerKey)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string query = "SELECT * FROM customer_has_book" + 
                            
        }
    }
}
