using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Interfaces
{
    public interface IBorrowService
    {
        void RequestBook(int bookId, Guid copyId);
        List<BorrowRequest> GetMyRequests();
        List<BorrowRecord> GetMyRecords();
        void ReturnBook(int recordId);

        List<BorrowRequest> GetPendingRequests();
        void ApproveRequest(int requestId);
        void RejectRequest(int requestId);
    }
}
