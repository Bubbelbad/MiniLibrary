using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class BorrowPeriod
    {
        public BorrowPeriod() { }

        public int id { get; set; }

        public DateTime startTime {  get; set; }

        public DateTime endTime {  get; set; }

        public bool isReturned { get; set; }

        public int bookId { get; set; }

        public int customerId {  get; set; }
    }
}
