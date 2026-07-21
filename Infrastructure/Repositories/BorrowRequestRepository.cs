using Application.Interfaces.Repositories;
using Domain.Exceptions;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Enums.BookBorrowRequestStatus;

namespace Infrastructure.Repositories
{
    public class BorrowRequestRepository : IBorrowRequestRepository
    {
        private readonly IFileRepository<BorrowRequest> _fileRepository;
        private List<BorrowRequest> _borrowRequests = new();

        public BorrowRequestRepository(IFileRepository<BorrowRequest> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task InitializeAsync()
        {
            _borrowRequests = await _fileRepository.GetAllLineAsync() ?? new List<BorrowRequest>();
            BorrowRequest.SyncIdCounter(_borrowRequests);
        }

        public async Task AddRequestAsync(BorrowRequest request)
        {
            _borrowRequests.Add(request);
            await SaveAsync();
        }

        public async Task DeleteRequestAsync(int id)
        {
            var request = GetRequestById(id) ?? throw new RequestByThisIdDoNotExists();

            _borrowRequests.Remove(request);

            await SaveAsync();
        }

        public List<BorrowRequest> GetAllRequests() => _borrowRequests;

        public List<BorrowRequest> GetApprovedRequests() =>
            _borrowRequests.Where(x => x.Status == BookBorrowStatus.Approved).ToList();

        public List<BorrowRequest> GetPendingRequests() =>
            _borrowRequests.Where(x => x.Status == BookBorrowStatus.Pending).ToList();

        public List<BorrowRequest> GetRejectedRequests() =>
            _borrowRequests.Where(x => x.Status == BookBorrowStatus.Rejected).ToList();

        public List<BorrowRequest> GetRequestsByUserId(int userId) =>
            _borrowRequests.Where(x => x.UserId == userId).ToList();

        public BorrowRequest GetRequestById(int id) =>
            _borrowRequests.FirstOrDefault(x => x.Id == id)
                ?? throw new RequestByThisIdDoNotExists();

        public async Task UpdateAsync(BorrowRequest request)
        {
            var index = _borrowRequests.FindIndex(x => x.Id == request.Id);

            if (index == -1)
                throw new RequestByThisIdDoNotExists();

            _borrowRequests[index] = request;

            await SaveAsync();
        }

        private Task SaveAsync() => _fileRepository.SaveAllAsync(_borrowRequests);
    }
}
