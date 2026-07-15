using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Domain.Models
{
    public class BorrowRecord
    {
        private static int _count = 0;


        public int Id { get; private set; }

        public int UserId { get; private set; }

        public int BookId { get; private set; }

        public Guid BookCopyId { get; private set; }

        public DateTime BorrowDate { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime? ActualReturnDate { get; private set; }

        public decimal Fee { get; private set; }

        // არ ჩაიწერება JSON-ში, გამოითვლება
        public bool IsReturned => ActualReturnDate != null;


        // არ ჩაიწერება JSON-ში, გამოითვლება
        public bool IsOverdue =>
            !IsReturned && DateTime.UtcNow > DueDate ||
            IsReturned && ActualReturnDate > DueDate;



        // ახალი BorrowRecord-ის შექმნა
        // როდესაც მომხმარებელი იღებს წიგნს
        public BorrowRecord(
            int userId,
            int bookId,
            Guid bookCopyId,
            int loanDays = 14)
        {
            Id = ++_count;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            BorrowDate = DateTime.UtcNow;
            DueDate = BorrowDate.AddDays(loanDays);

            ActualReturnDate = null;
        }



        // JSON-დან აღდგენა
        [JsonConstructor]
        public BorrowRecord(
            int id,
            int userId,
            int bookId,
            Guid bookCopyId,
            DateTime borrowDate,
            DateTime dueDate,
            DateTime? actualReturnDate)
        {
            Id = id;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            BorrowDate = borrowDate;
            DueDate = dueDate;

            ActualReturnDate = actualReturnDate;
        }



        public void ReturnBook()
        {
            if (IsReturned)
                throw new InvalidOperationException("Book already returned.");

            ActualReturnDate = DateTime.UtcNow;
        }



        public static void SyncIdCounter(List<BorrowRecord> records)
        {
            int maxId = records
                .Select(x => x.Id)
                .DefaultIfEmpty(0)
                .Max();


            if (maxId > _count)
                _count = maxId;
        }

        public decimal CalculateFee()
        {
            DateTime endDate;

            if (ActualReturnDate != null)
            {
                endDate = ActualReturnDate.Value;
            }
            else
            {
                endDate = DateTime.UtcNow;
            }

            if (endDate <= DueDate)
                return 0;

            int overdueDays = (endDate.Date - DueDate.Date).Days;

            return overdueDays * 1m;
        }
    }
}