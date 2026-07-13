using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Infrastructure.Repositories
{
    public class BorrowRequestRepository : IBorrowRequestRepository
    {
        private readonly IFileRepository<BorrowRequest> _fileRepository;
        private readonly List<BorrowRequest> _borrowRequests;
        
        public BorrowRequestRepository(IFileRepository<BorrowRequest> filerepository)
        {
            _fileRepository = filerepository;
            _borrowRequests = _fileRepository.GetAllLine() ?? new List<BorrowRequest>();
        }
        public void AddRequest(BorrowRequest request)
        {
            _borrowRequests.Add(request);
            _fileRepository.SaveAll(_borrowRequests);
        }

        public void DeleteRequest(int id)
        {
            BorrowRequest request = _borrowRequests.First(x => x.Id == id) ?? throw new RequestByThisIdDoNotExists();
            _borrowRequests.Remove(request);
        }

        public List<BorrowRequest>? GetAllRequests()
        {
            List<BorrowRequest> requests = _borrowRequests;
            if (requests.Count == 0) throw new RequestByThisParametrsDoNotExists();
            return requests;
        }

        public List<BorrowRequest>? GetApprovedRequests()
        {
            List<BorrowRequest> requsts = _borrowRequests.Where(x => x.Status == BookBorrowStatus.Approved).ToList();
            if (requsts.Count == 0)
                throw new RequestByThisParametrsDoNotExists();
            return requsts;
        }

        public List<BorrowRequest>? GetRequestsByUser(int UserId)
        {
            var request = _borrowRequests.Where(x => x.UserId == UserId).ToList() ?? throw new InvalidUserException();
            return request;
        }

        public List<BorrowRequest>? GetPendingRequests()
        {
            List<BorrowRequest> requsts = _borrowRequests.Where(x => x.Status == BookBorrowStatus.Pending).ToList();
            if (requsts.Count == 0)
                throw new RequestByThisParametrsDoNotExists();
            return requsts;
        }
        public List<BorrowRequest>? GetRejectedRequests()
        {
            List<BorrowRequest> requsts = _borrowRequests.Where(x => x.Status == BookBorrowStatus.Rejected).ToList();
            if (requsts.Count == 0)
                throw new RequestByThisParametrsDoNotExists();
            return requsts;
        }

        public BorrowRequest? GetRequestById(int id)
        {
            BorrowRequest? request = _borrowRequests.FirstOrDefault(x => x.Id == id);
            return request ?? throw new RequestByThisIdDoNotExists();
        }
        public void Update(BorrowRequest request)
        {
            var existingRequest = _borrowRequests
                .FirstOrDefault(x => x.Id == request.Id) ?? throw new RequestByThisParametrsDoNotExists() ;

            existingRequest.Status=BookBorrowStatus.Approved;


            _fileRepository.SaveAll(_borrowRequests);
            // ამას გამოვიყენებ approve მეთოდშ რომ ადმინა დაადასტუროს მოთხოვნა


        }

        public List<BorrowRequest>? GetRequestsByUserId(int userId)
        {
            var existingRequest = _borrowRequests
                .Where(x => x.UserId == userId).ToList() ?? throw new RequestByThisIdDoNotExists();
            return existingRequest;
        }
    }
}
