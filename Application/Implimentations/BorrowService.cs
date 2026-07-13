using Application.Validations;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.DataAccess.Interfaces;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implimentations
{
    public class BorrowService : IBorrowService
    {
        private readonly Validation _validations;
        private readonly UserSession _userSession;
        private readonly IBorrowRequestRepository _borrowRequestRepo;
        public BorrowService(Validation validation, UserSession userSession, IBorrowRequestRepository borrowRequestRepo)
        {
            _validations = validation;
            _userSession = userSession;
            _borrowRequestRepo = borrowRequestRepo;
        }
        public void ApproveRequest(int requestId)
        {
            _validations.EnsureAdmin(_userSession);
            BorrowRequest? request = _borrowRequestRepo.GetRequestById(requestId) ?? throw new RequestByThisIdDoNotExists();
            _borrowRequestRepo.Update(request);
            //BorrowRecord record = new BorrowRecord(_userSession.CurrentUser.Id);

        }

        public List<BorrowRecord> GetMyRecords()
        {
            throw new NotImplementedException();
        }

        public List<BorrowRequest> GetMyRequests()
        {
            throw new NotImplementedException();
        }

        public List<BorrowRequest> GetPendingRequests()
        {
            throw new NotImplementedException();
        }

        public void RejectRequest(int requestId)
        {
            throw new NotImplementedException();
        }

        public void RequestBook(int bookId, Guid copyId)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(int recordId)
        {
            throw new NotImplementedException();
        }
    }
}
