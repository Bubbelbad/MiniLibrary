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
        string username = "admin"; 
        string password = "admin";

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
                Book book = new Book((int)reader["book_id"], (string)reader["title"], (string)reader["author"]);
                books.Add(book.id, book);
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
                Customer customer = new Customer((int)reader["customer_id"], (string)reader["first_name"], (string)reader["last_name"], (string)reader["email"], (string)reader["status"]);
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
    }
}
