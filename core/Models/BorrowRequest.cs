using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Domain.Models
{
    public  class BorrowRequest
    {
        private static int _count = 0;
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int BookId { get; private set; }
        public Guid BookCopyId { get; private set; }
        public BookBorrowStatus Status { get;  set; }
        public DateTime RequestDate {  get; private set; }

        public BorrowRequest(int userId, int bookId, Guid bookCopyId)
        {
            Id = ++_count;

            UserId = userId;
            BookId = bookId;
            BookCopyId = bookCopyId;

            Status = BookBorrowStatus.Pending;

            RequestDate = DateTime.UtcNow;
        }
        public BorrowRequest()
        {
            
        }
        public void Approve()
        {
            Status = BookBorrowStatus.Approved;
        }

        public void Reject()
        {
            Status = BookBorrowStatus.Rejected;
        }
    }
}
