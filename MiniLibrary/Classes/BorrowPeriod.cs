using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class BorrowPeriod
    {
        public BorrowPeriod(int id, DateTime startTime, DateTime endTime, bool isReturned, int bookId, int customerId)
        {
            this.Id = id;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.IsReturned = isReturned;
            this.BookId = bookId;
            this.CustomerId = customerId;
        }

        public int Id { get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime {  get; set; }
        public bool IsReturned { get; set; }
        public int BookId { get; set; }
        public int CustomerId {  get; set; }
    }
}
