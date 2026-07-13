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

        void Add(BorrowRecord record);

        BorrowRecord? GetById(int id);

        List<BorrowRecord> ?  GetAll();

        List<BorrowRecord> ? GetByUserId(int userId);
        void Update(BorrowRecord record);


        ///////// ჯერ უნდა გავაკეთო borrow request და მერე შეიქმნება რექორდი
    }
}
