using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Customer
    {
        public Customer(int id, string firstName, string lastName, string email, string state)
        {
            Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.State = state;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
    }
}
