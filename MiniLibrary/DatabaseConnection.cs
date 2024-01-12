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
        string password = "Abc123!";

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
    }
}
