using Application.Interfaces.Repositories;
using Domain.Exceptions;
using LibraryManagementSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class BorrowRecordRepository : IBorrowReqordRepository
    {
        private readonly IFileRepository<BorrowRecord> _fileRepository;
        private List<BorrowRecord> _borrowRecords = new();

        public BorrowRecordRepository(IFileRepository<BorrowRecord> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task InitializeAsync()
        {
            _borrowRecords = await _fileRepository.GetAllLineAsync() ?? new List<BorrowRecord>();
            BorrowRecord.SyncIdCounter(_borrowRecords);
        }

        public async Task AddAsync(BorrowRecord record)
        {
            _borrowRecords.Add(record);
            await SaveAsync();
        }

        public List<BorrowRecord> GetAll() => _borrowRecords;

        public BorrowRecord GetById(int id) =>
            _borrowRecords.FirstOrDefault(x => x.Id == id)
                ?? throw new RecordsDoNotExistsException();

        public List<BorrowRecord> GetByUserId(int userId) =>
            _borrowRecords.Where(x => x.UserId == userId).ToList();

        public async Task UpdateAsync(BorrowRecord record)
        {
            int index = _borrowRecords.FindIndex(x => x.Id == record.Id);

            if (index == -1)
                throw new RecordsDoNotExistsException();

            _borrowRecords[index] = record;

            await SaveAsync();
        }

        private Task SaveAsync() => _fileRepository.SaveAllAsync(_borrowRecords);
    }
}
