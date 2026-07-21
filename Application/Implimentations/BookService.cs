using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Validations;
using Domain.Exceptions;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddBookAsync(Book book)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(book);

            await _repository.AddBookAsync(book);
        }

        public async Task AddCopyAsync(BookCopy copy, int bookId)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(copy);
            _validations.EnsureId(bookId);

            Book book = _repository.GetBookById(bookId)
                ?? throw new BookNotFoundException();

            book.AddCopy(copy);

            await _repository.UpdateAsync(book);
        }

        public int Count(Book book)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureInput(book);

            return _repository.Count(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            _validations.EnsureAdmin(_userSession);
            _validations.EnsureId(id);

            await _repository.DeleteBookByIdAsync(id);
        }

        public async Task DeleteCopyAsync(Guid copyId)
        {
            _validations.EnsureAdmin(_userSession);

            Book book = _repository.GetBookContainingCopy(copyId);

            book.RemoveCopy(copyId);

            await _repository.UpdateAsync(book);
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
