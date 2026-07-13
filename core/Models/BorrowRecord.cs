using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Models
{
    public class BorrowRecord
    {
        public int Id { get; private set; }

        public int UserId { get; private set; }

        public int BookId { get; private set; }

        public Guid BookCopyId { get; private set; }

        public DateTime BorrowDate { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime? ActualReturnDate { get; private set; }


        public bool IsReturned => ActualReturnDate != null;


        public bool IsOverdue =>
            !IsReturned && DateTime.UtcNow > DueDate
            ||
            IsReturned && ActualReturnDate > DueDate;



        public BorrowRecord(
            int id,
            int userId,
            int bookId,
            Guid bookCopyId,
            int loanDays = 14)
        {
            Id = id;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            BorrowDate = DateTime.UtcNow;
            DueDate = BorrowDate.AddDays(loanDays);
        }


        // JSON Serializer-სთვის
        private BorrowRecord()
        {

        }
    }
}