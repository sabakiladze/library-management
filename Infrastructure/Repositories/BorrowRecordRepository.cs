using Domain.Exceptions;
using Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class BorrowRecordRepository : IBorrowReqordRepository
    {
        private readonly IFileRepository<BorrowRecord> _fileRepository;
        private readonly List<BorrowRecord> _borrowRecord;

        public BorrowRecordRepository(IFileRepository<BorrowRecord> borrow)
        {
            _fileRepository = borrow;
            _borrowRecord = _fileRepository.GetAllLine() ?? new List<BorrowRecord>();
        }
        public void Add(BorrowRecord record)
        {
            _borrowRecord.Add(record);
            _fileRepository.SaveAll(_borrowRecord);

        }

        public List<BorrowRecord> ? GetAll()
        {
            List<BorrowRecord> records = _borrowRecord ?? throw new RecordsDoNotExistsException();
            return records;
        }

        public BorrowRecord? GetById(int id)
        {
            BorrowRecord records = _borrowRecord.First(x => x.Id == id) ?? throw new RecordsDoNotExistsException();
            return records;
        }

        public List<BorrowRecord> ? GetByUserId(int userId)
        {
            //List < BorrowRecord > records= _borrowRecord.Where(x => x.User_Id == userId).ToList() ?? throw new InvalidUserIdOrUserHasNotHaveAnyRecordYet();
            return records;


        }


        public void Update(BorrowRecord record)
        {
            var existingRecord = _borrowRecord.FirstOrDefault(x => x.Id == record.Id) ?? throw new RecordsDoNotExistsException();
            //existingRecord.ActualReturnDate = record.ActualReturnDate;

            _fileRepository.SaveAll(_borrowRecord);
            // ამას გამოვიყენებ სერვისში რათა დაბრუნებული წიგნის დრო შევიყვანო, როდის მოიტანა
        }
    }
}
