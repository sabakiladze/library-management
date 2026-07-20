using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Validations;
using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;

namespace Application.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly UserSession _userSession;
        private readonly Validation _validations;


        public BookService(
            IBookRepository repository,
            UserSession userSession,
            Validation validation)
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
            _validations.EnsureId(bookId);


            Book book = _repository.GetBookById(bookId)
                ?? throw new BookNotFoundException();


            book.AddCopy(copy);


            _repository.Update(book);
        }



        public int Count(Book book)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(book);

            return _repository.Count(book);
        }



        public void DeleteBook(int id)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureId(id);

            _repository.DeleteBookById(id);
        }



        public void DeleteCopy(Guid copyId)
        {
            _validations.EnsureAdmin(_userSession);


            Book book = _repository.GetBookContainingCopy(copyId);


            book.RemoveCopy(copyId);


            _repository.Update(book);
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



        public BookCopy? GetCopyById(Guid copyId)
        {
            return _repository.GetBookCopyByGuid(copyId);
        }



        public List<Book> GetBooksByAuthor(Author author)
        {
            _validations.EnsureInput(author);

            return _repository.GetAllBooksByAuthor(author);
        }



        public List<Book> GetBooksByName(string name)
        {
            _validations.EnsureString(name);

            return _repository.GetBookByName(name);
        }



        public List<Book> GetBooksByPublishedYear(int year)
        {
            if (year <= 0)
                throw new ArgumentException("Invalid year");


            return _repository.GetBooksByPublishedYear(year);
        }



        public int GetTotalBookCount()
        {
            _validations.EnsureAdmin(_userSession);

            return _repository.GetAllBookCount();
        }



        public int GetTotalCopieCount()
        {
            _validations.EnsureAdmin(_userSession);

            return _repository.GetToTalCopiesCount();
        }
    }
}