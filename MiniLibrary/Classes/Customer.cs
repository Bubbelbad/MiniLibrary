using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Customer
    {
        public Customer(int id, string firstName, string lastName, string email, string status)
        {
            Id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.status = status;
        }

        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }
}
