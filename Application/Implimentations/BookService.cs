using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace Application.Implementations
{
    public class BookService :IBookService
    {
        private readonly IBookRepository _repository;
        private readonly UserSession _userSession;
        public BookService(IBookRepository repository, UserSession userSession)
        {
            _repository= repository;
            _userSession=userSession;
        }
        public void AddBook(Book book)
        {
            EnsureAdmin();
            if (book == null) throw new ArgumentNullException(nameof(book));
            _repository.AddBook(book);

        }

        public void AddCopy(BookCopy copy, int bookId)
        {
            EnsureAdmin();
            if(copy == null || bookId<=0) throw new ArgumentNullException( nameof(copy));
            _repository.AddCopyBook(copy, bookId);
        }

        public int BookCountById(int id)
        {
            EnsureAdmin();
            EnsureId(id);
                return _repository.BookCountById(id);// ძებნის ერთი წგინი, მაგ"კაც-ადამ" რამდენია
        }

        public void DeleteBook(int id)
        {
            EnsureAdmin();
            EnsureId(id);
            _repository.DeleteBookById(id);
        }

        public bool DeleteBookCopy(Guid id)
        {
            _repository.DeleteBookCopyByGuidId(id);
            return true;  /// ესეც კარგად უნდა გავიგო, ბოოლ რომ აბრუნებს
        }

        public List<Book> GetAllBooks() => _repository.GetAllBook();

        public Book GetBookById(int id)
        {
            EnsureId(id);
            return _repository.GetBookById(id);
        }

        public BookCopy GetBookCopyByGuid(Guid id)=>_repository.GetBookCopyByGuid(id);

        public List<Book> GetBooksByAuthor(string author)
        {
            EnsureString(author);
           return  _repository.GetAllBookCopyByAuthor(author.Trim().ToLower());
        }

        public List<Book> GetBooksByName(string name)
        {
            EnsureString(name);
            return _repository.GetBookByName(name.Trim().ToLower());
        }

        public List<Book> GetBooksByYear(int year)
        {
            return _repository.AllBookByPublishedYear(year);
        }

        public int GetTotalBookCount(int id)
        {
            EnsureAdmin();
            EnsureId(id);
            return _repository.BookCountById(id);
        }

        public int GetTotalBookCount()
        {
            EnsureAdmin();
            return _repository.GetBookCount();
        }

        public int GetTotalCopiesCount()
        {
            EnsureAdmin();
            return _repository.GetBookCountOfEveryExemplar();
        }


        ///ესენი შეიძ₾ება ვალიდაციებში გადავიტანო
        private void EnsureAdmin()
        {
            if (_userSession.CurrentUser == null || _userSession.CurrentUser.Role != Role.Admin)
                throw new RoleException();

        }
        private void EnsureId(int id)
        {
            if(id<=0) throw new ArgumentOutOfRangeException("invalid input", nameof(id));
        }
        private void EnsureString(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("invalid input", nameof(str));
        }
    }
}
