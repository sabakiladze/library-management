using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBorrowRequestRepository
    {
        Task InitializeAsync();

        List<BorrowRequest>? GetAllRequests();
        List<BorrowRequest>? GetPendingRequests();
        BorrowRequest? GetRequestById(int id);
        List<BorrowRequest>? GetRejectedRequests();
        List<BorrowRequest>? GetApprovedRequests();
        List<BorrowRequest>? GetRequestsByUserId(int userId);

        Task AddRequestAsync(BorrowRequest request);
        Task DeleteRequestAsync(int id);
        Task UpdateAsync(BorrowRequest request);
    }
}
