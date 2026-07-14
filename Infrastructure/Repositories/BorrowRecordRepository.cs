using Domain.Exceptions;
using Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class BorrowRecordRepository : IBorrowReqordRepository
    {
        private readonly IFileRepository<BorrowRecord> _fileRepository;
        private readonly List<BorrowRecord> _borrowRecords;


        public BorrowRecordRepository(IFileRepository<BorrowRecord> fileRepository)
        {
            _fileRepository = fileRepository;

            _borrowRecords = _fileRepository.GetAllLine()
                ?? new List<BorrowRecord>();

            BorrowRecord.SyncIdCounter(_borrowRecords);
        }



        public void Add(BorrowRecord record)
        {
            _borrowRecords.Add(record);

            Save();
        }



        public List<BorrowRecord> GetAll()
        {
            return _borrowRecords;
        }



        public BorrowRecord GetById(int id)
        {
            return _borrowRecords
                .FirstOrDefault(x => x.Id == id)
                ?? throw new RecordsDoNotExistsException();
        }



        public List<BorrowRecord> GetByUserId(int userId)
        {
            return _borrowRecords
                .Where(x => x.UserId == userId)
                .ToList();
        }



        public void Update(BorrowRecord record)
        {
            int index = _borrowRecords
                .FindIndex(x => x.Id == record.Id);


            if (index == -1)
                throw new RecordsDoNotExistsException();


            _borrowRecords[index] = record;

            Save();
        }



        private void Save()
        {
            _fileRepository.SaveAll(_borrowRecords);
        }
    }
}