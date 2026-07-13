using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Models
{
    public class Book
    {
        private static int _id = 0;
        public int Id { get; private set; }
        private string _title = string.Empty;
        public Author Author;
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
        public override bool Equals(object? obj)
        {
            if (obj is not Book other) return false;
            return Title.Equals(other.Title, StringComparison.OrdinalIgnoreCase) &&
                PublicationYear == other.PublicationYear &&
                Author.Equals(other.Author);

        }


        public Book() { }

        public Book(string title, Author author, int publicationYear)
        {
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
            _id++;
            Id = _id;
        }
       

    }
}
