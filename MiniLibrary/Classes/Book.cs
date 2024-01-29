using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLibrary.Classes
{
    internal class Book
    {
        public Book(int id, string title, string author, bool available)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Available = available;
        }

        public int Id {  get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }
    }
}
