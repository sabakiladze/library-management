using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryManagementSystem.Domain.Enums.BookStatus;

namespace Domain.Models
{
    public class BookCopy
    {

        public Guid Id { get; private set; }
        public int BookId { get; private set; }

        public Book_Status Status { get; set; } = Book_Status.Available;
        public BookCopy()
        {

        }
        public BookCopy(int bookId)
        {
            Id = Guid.NewGuid();

            BookId = bookId;
            Status = Book_Status.Available;
        }

    }
}
