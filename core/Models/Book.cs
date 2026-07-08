using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Models
{
    internal class Book
    {
        private static int _id = 0;
        public int Id { get;  private set; }
        private string _title = string.Empty;
        private string _author = string.Empty;
        public bool Available { get; set; } = true;
        public int PublicationYear { get; set; }
        public List<BookCopy> Copies { get; set; } = new List<BookCopy>();


        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Book title cannot be empty!");
                _title = value;
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Author name cannot be empty!");
                _author = value;
            }
        }



        //public Book() { }

        public Book(string title, string author, int publicationYear)
        {
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
            _id++;
            Id = _id;
        }
    }
}
