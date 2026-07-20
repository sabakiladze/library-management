using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public  interface IBorrowRequestRepository
    {
        void AddRequest(BorrowRequest request);

        List<BorrowRequest> ?GetAllRequests();

        List<BorrowRequest>? GetPendingRequests();

        BorrowRequest? GetRequestById(int id);

        void DeleteRequest(int id);
        public List<BorrowRequest>? GetRejectedRequests();
        public List<BorrowRequest>? GetApprovedRequests();

        void Update(BorrowRequest request);
        List<BorrowRequest> ? GetRequestsByUserId(int userId);
    }
}
