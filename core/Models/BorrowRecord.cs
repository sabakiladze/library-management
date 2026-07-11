using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Models
{
    internal class BorrowRecord
    {
        private static int _borrowCount = 0;
        public int Id { get; private set; } // უშუალოდ წაღების
        public  int User_Id { get; set; }
        public  int Book_Id {get;set;} //წიგნის დასახელებისთვის
        public Guid Book_Copy_Id { get;set;}// ეგზემპლარი
        public DateTime BorrowDate { get; private set; }

        public DateTime DueDate { get; private set; }
        public DateTime? ActualReturnDate { get; set; }
        public bool IsOverdue => ActualReturnDate == null
            ? DateTime.UtcNow > DueDate
            : ActualReturnDate > DueDate;
        public BorrowRecord(int userId, int bookId, Guid bookCopyId, int loanDays = 14)
        {
            _borrowCount++;
            Id = _borrowCount;

            User_Id = userId;
            Book_Id = bookId;
            Book_Copy_Id = bookCopyId;

            BorrowDate = DateTime.UtcNow;
            DueDate = DateTime.UtcNow.AddDays(loanDays); 
            ActualReturnDate = null; // თავიდან ცხადია null-ია, რადგან ჯერ არ დაუბრუნებია
        }
    }
}
