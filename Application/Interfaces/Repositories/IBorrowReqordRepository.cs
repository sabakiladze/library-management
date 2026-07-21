using LibraryManagementSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBorrowReqordRepository
    {
        Task InitializeAsync();

        BorrowRecord? GetById(int id);
        List<BorrowRecord>? GetAll();
        List<BorrowRecord>? GetByUserId(int userId);

        Task AddAsync(BorrowRecord record);
        Task UpdateAsync(BorrowRecord record);
    }
}
