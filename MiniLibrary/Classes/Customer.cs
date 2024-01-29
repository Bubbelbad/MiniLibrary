using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Customer
    {
        public Customer(int id, string firstName, string lastName, string email, string customerPassword, string state, bool admin)
        {
            Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.CustomerPassword = customerPassword;
            this.State = state;
            this.Admin = admin;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CustomerPassword { get; set; }
        public string State { get; set; }
        public bool Admin { get; set; }
    }
}
