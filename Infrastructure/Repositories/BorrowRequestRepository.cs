using Application.Interfaces.Repositories;
using Domain.Exceptions;
using Domain.Models;
using System.Linq;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Infrastructure.Repositories
{
    public class BorrowRequestRepository : IBorrowRequestRepository
    {
        private readonly IFileRepository<BorrowRequest> _fileRepository;
        private readonly List<BorrowRequest> _borrowRequests;


        public BorrowRequestRepository(IFileRepository<BorrowRequest> fileRepository)
        {
            _fileRepository = fileRepository;
            _borrowRequests = _fileRepository.GetAllLine() ?? new List<BorrowRequest>();

            BorrowRequest.SyncIdCounter(_borrowRequests);
        }



        public void AddRequest(BorrowRequest request)
        {
            _borrowRequests.Add(request);
            Save();
        }



        public void DeleteRequest(int id)
        {
            var request = GetRequestById(id);

            _borrowRequests.Remove(request);

            Save();
        }



        public List<BorrowRequest> GetAllRequests()
        {
            return _borrowRequests;
        }



        public List<BorrowRequest> GetApprovedRequests()
        {
            return _borrowRequests
                .Where(x => x.Status == BookBorrowStatus.Approved)
                .ToList();
        }



        public List<BorrowRequest> GetPendingRequests()
        {
            return _borrowRequests
                .Where(x => x.Status == BookBorrowStatus.Pending)
                .ToList();
        }



        public List<BorrowRequest> GetRejectedRequests()
        {
            return _borrowRequests
                .Where(x => x.Status == BookBorrowStatus.Rejected)
                .ToList();
        }



        public List<BorrowRequest> GetRequestsByUserId(int userId)
        {
            return _borrowRequests
                .Where(x => x.UserId == userId)
                .ToList();
        }



        public BorrowRequest GetRequestById(int id)
        {
            return _borrowRequests
                .FirstOrDefault(x => x.Id == id)
                ?? throw new RequestByThisIdDoNotExists();
        }



        public void Update(BorrowRequest request)
        {
            var index = _borrowRequests
                .FindIndex(x => x.Id == request.Id);


            if (index == -1)
                throw new RequestByThisIdDoNotExists();


            _borrowRequests[index] = request;

            Save();
        }



        private void Save()
        {
            _fileRepository.SaveAll(_borrowRequests);
        }
    }
}