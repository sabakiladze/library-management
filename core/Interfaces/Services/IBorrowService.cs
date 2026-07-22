using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IBorrowService
    {
        Task RequestBookAsync(int bookId);
        List<BorrowRequest>? GetMyRequests();
        List<BorrowRecord>? GetMyRecords();
        Task ReturnBookAsync(int recordId);

        List<BorrowRequest>? GetPendingRequests();
        Task ApproveRequestAsync(int requestId);
        Task RejectRequestAsync(int requestId);
        Task CancelRequestAsync(int requestId);
    }
}
