using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Infrastructure.Repositories
{
    public  class BorrowRequestRepository : IBorrowRequestRepository
    {
        private readonly IFileRepository<BorrowRequest> _fileRepository;
        private readonly List<BorrowRequest> _borrowRequests;
        public BorrowRequestRepository(IFileRepository<BorrowRequest> filerepository, List<BorrowRequest> borrowRequest)
        {
            _fileRepository= filerepository;
            _borrowRequests= _fileRepository.GetAllLine()??new List<BorrowRequest>();
        }
        public void AddRequest(BorrowRequest request)
        {
            _borrowRequests.Add(request);
            _fileRepository.SaveAll(_borrowRequests);
        }

        public void DeleteRequest(int id)
        {
            BorrowRequest request = _borrowRequests.First(x=>x.Id==id) ?? throw new RequestByThisIdDoNotExists();
            _borrowRequests.Remove(request);
        }

        public List<BorrowRequest>? GetAllRequests() => _borrowRequests;

        public List<BorrowRequest>? GetApprovedRequests()=>_borrowRequests.Where(x => x.Status == BookBorrowStatus.Approved).ToList();

        public List<BorrowRequest> ?GetPendingRequests()=> _borrowRequests.Where(x => x.Status == BookBorrowStatus.Pending).ToList();

        public List<BorrowRequest>? GetRejectedRequests()
        {
            List<BorrowRequest> requsts= _borrowRequests.Where(x => x.Status == BookBorrowStatus.Rejected).ToList();
            if(requsts.Count==0)
                throw new RequestByThisParametrsDoNotExists();
            return requsts;
        }

        public BorrowRequest? GetRequestById(int id)
        {
            BorrowRequest ? request = _borrowRequests.FirstOrDefault(x => x.Id == id);
            return request == null ? throw new RequestByThisIdDoNotExists() : request;
        }

       

       
    }
}
