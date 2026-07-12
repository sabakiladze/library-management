using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public  interface IBorrowRequestRepository
    {
        void AddRequest(BorrowRequest request);

        List<BorrowRequest> GetAllRequests();

        List<BorrowRequest> GetPendingRequests();

        BorrowRequest? GetRequestById(int id);

        void UpdateRequest(BorrowRequest request);

        void DeleteRequest(int id);
    }
}
