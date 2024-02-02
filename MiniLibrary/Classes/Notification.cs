using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Notification
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public string type { get; set; }
        public int borrow_period { get; set; }
    }
}
