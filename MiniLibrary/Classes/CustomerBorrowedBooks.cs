using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class CustomerBorrowedBooks
    {
        public CustomerBorrowedBooks(int id, string book, DateTime start_time, DateTime deadline, bool returned)
        {
            this.Id = id;
            this.Book = book;
            this.Start_Time = start_time;
            this.Deadline = deadline;
            this.Returned = returned;
        }
        public int Id { get; set; }
        public string Book { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime Deadline {  get; set; }
        public bool Returned { get; set; }
    }
}
