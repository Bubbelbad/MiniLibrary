using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Book
    {
        public Book(int id, string title, string author)
        {
            this.id = id;
            this.title = title;
            this.author = author;
        }

        public int id {  get; set; }
        public string title { get; set; }
        public string author { get; set; }
    }
}
