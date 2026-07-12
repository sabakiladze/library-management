using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBorrowReqordRepository
    {

        void AddBorrowRecord(BorrowRecord record);
        BorrowRecord? GetById(int id);
        List<BorrowRecord> GetAll();
        List<BorrowRecord> GetUserBorrowHistory(int userId);
        List<BorrowRecord> GetActiveBorrowings(int userId);
        void ReturnBook(int id, DateTime returnDate);
        List<BorrowRecord> GetOverdueBooks();
        ///////// ჯერ უნდა გავაკეთო borrow request და მერე შეიქმნება რექორდი
    }
}
