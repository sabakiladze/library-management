using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LibraryManagementSystem.Domain.Enums.BookStatus;
namespace LibraryManagementSystem.Domain.Models
{
    public class Book
    {
        private static int _idCounter = 0;
        public int Id { get; private set; }
        private string _title = string.Empty;
        public Author Author { get; set; }
        private int _publicationYear;
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
        public int PublicationYear
        {
            get => _publicationYear;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Publication year must be a positive number!");
                if (value > DateTime.Now.Year)
                    throw new ArgumentException($"Publication year can't be in the future (current year: {DateTime.Now.Year})!");
                _publicationYear = value;
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Book other) return false;
            return Title.Equals(other.Title, StringComparison.OrdinalIgnoreCase)
                && PublicationYear == other.PublicationYear
                && Author.Equals(other.Author);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Title.ToLower(), PublicationYear, Author);
        }

        [JsonConstructor]
        public Book(
            int id,
            string title,
            Author author,
            int publicationYear,
            List<BookCopy>? copies)
        {
            Id = id;

            Title = title;
            Author = author;
            PublicationYear = publicationYear;

            Copies = copies ?? new List<BookCopy>();
        }

        public Book(
            string title,
            Author author,
            int publicationYear)

        {
            Id = ++_idCounter;
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
        }
        public static void SyncIdCounter(List<Book> books)
        {
            int maxId = books.Select(x => x.Id).
                DefaultIfEmpty(0).Max();
            if (maxId > _idCounter)
                _idCounter = maxId;
        }
        public void AddCopy(BookCopy copy)
        {
            if (copy == null)
                throw new BookNotFoundException();

            Copies.Add(copy);
        }
        public void RemoveCopy(Guid copyId)
        {
            BookCopy? copy = Copies.FirstOrDefault(x => x.Id == copyId) ?? throw new BookNotFoundException();
            Copies.Remove(copy);
        }
        public bool HasAvailableCopy()
        {
            return Copies.Any(x => x.Status == Book_Status.Available);
        }
    }
}

