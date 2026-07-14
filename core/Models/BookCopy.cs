using LibraryManagementSystem.Domain.Enums;
using System;
using System.Text.Json.Serialization;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace Domain.Models
{
    public class BookCopy
    {
        public Guid Id { get; private set; }

        public int BookId { get; private set; }

        public Book_Status Status { get; private set; }



        // ახალი ეგზემპლარის შექმნა
        public BookCopy(int bookId)
        {
            Id = Guid.NewGuid();

            BookId = bookId;

            Status = Book_Status.Available;
        }



        // JSON-დან აღდგენა
        [JsonConstructor]
        public BookCopy(
            Guid id,
            int bookId,
            Book_Status status)
        {
            Id = id;

            BookId = bookId;

            Status = status;
        }



        public void Borrow()
        {
            if (Status != Book_Status.Available)
                throw new InvalidOperationException("Copy is already borrowed");


            Status = Book_Status.Borrowed;
        }



        public void Return()
        {
            if (Status == Book_Status.Available)
                throw new InvalidOperationException("Copy is already available");


            Status = Book_Status.Available;
        }
    }
}