using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Domain.Models
{
    public class BorrowRequest
    {
        private static int _count = 0;


        public int Id { get; private set; }

        public int UserId { get; private set; }

        public int BookId { get; private set; }

        public Guid BookCopyId { get; private set; }

        public BookBorrowStatus Status { get; private set; }

        public DateTime RequestDate { get; private set; }



        // ახალი მოთხოვნის შექმნა
        public BorrowRequest(
            int userId,
            int bookId,
            Guid bookCopyId)
        {
            Id = ++_count;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            Status = BookBorrowStatus.Pending;

            RequestDate = DateTime.UtcNow;
        }



        // JSON-დან აღდგენა
        [JsonConstructor]
        public BorrowRequest(
            int id,
            int userId,
            int bookId,
            Guid bookCopyId,
            BookBorrowStatus status,
            DateTime requestDate)
        {
            Id = id;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            Status = status;

            RequestDate = requestDate;
        }



        public void Approve()
        {
            Status = BookBorrowStatus.Approved;
        }


        public void Reject()
        {
            Status = BookBorrowStatus.Rejected;
        }


        public void Cancel()
        {
            Status = BookBorrowStatus.Cancelled;
        }



        public static void SyncIdCounter(List<BorrowRequest> requests)
        {
            int maxId = requests
                .Select(x => x.Id)
                .DefaultIfEmpty(0)
                .Max();


            if (maxId > _count)
                _count = maxId;
        }
    }
}