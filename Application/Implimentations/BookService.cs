using Application.Interfaces;
using Application.Validations;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System.Net;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace Application.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly UserSession _userSession;
        private readonly Validation _validations;

        public BookService(IBookRepository repository, UserSession userSession, Validation validation)
        {
            _repository = repository;
            _userSession = userSession;
            _validations = validation;
        }


        public void AddBook(Book book)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(book);

            _repository.AddBook(book);
        }


        public void AddCopy(BookCopy copy, int bookId)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(copy);
           _validations. EnsureId(bookId);

            _repository.AddCopyBook(copy, bookId);
        }


        public int BookCountBy(Book book)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(book);

            return _repository.BookCount(book);
        }


        public void DeleteBook(int id)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureId(id);

            _repository.DeleteBookById(id);
        }


        public bool DeleteBookCopy(Guid id)
        {
            _validations.EnsureAdmin(_userSession);
            return _repository.DeleteBookCopyByGuidId(id);
        }


        public List<Book> GetAllBooks()
        {
            return _repository.GetAllBook();
        }


        public Book? GetBookById(int id)
        {
            _validations.EnsureId(id);
            return _repository.GetBookById(id);
        }


        public BookCopy? GetBookCopyByGuid(Guid id)
        {
            return _repository.GetBookCopyByGuid(id);
        }


        public List<Book> GetBooksByAuthor(Author author)
        {
            _validations.EnsureInput(author);
            return _repository.GetAllBookCopyByAuthor(author);
        }


        public List<Book> GetBooksByName(string name)
        {
           _validations.EnsureString(name);

            return _repository.GetBookByName(name);
        }


        public List<Book> GetBooksByYear(int year)
        {
            if (year <= 0)
                throw new ArgumentException("Invalid year");

            return _repository.AllBookByPublishedYear(year);
        }


        public int GetTotalBookCount()
        {
            _validations.EnsureAdmin(_userSession);
            return _repository.GetAllBookCount();
        }


        public int GetTotalCopiesCount()
        {
            _validations.EnsureAdmin(_userSession);
            return _repository.GetBookCountOfEveryExemplar();
        }

    }
}