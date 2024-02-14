using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Notification
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public int Borrow_period { get; set; }

        public Notification(int id, DateTime created, string message, int borrow_period)
        {
            this.Id = id;
            this.Created = created;
            this.Message = message;
            this.Borrow_period = borrow_period;
        }
    }
}
