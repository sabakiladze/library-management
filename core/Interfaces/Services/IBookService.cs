using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IBookService
    {
        Task AddBookAsync(Book book);
        Task AddCopyAsync(BookCopy copy, int bookId);
        Task DeleteBookAsync(int id);
        Task DeleteCopyAsync(Guid copyId);

        List<Book> GetAllBooks();
        Book? GetBookById(int id);
        BookCopy? GetCopyById(Guid copyId);
        List<Book> GetBooksByAuthor(Author author);
        List<Book> GetBooksByName(string name);
        List<Book> GetBooksByPublishedYear(int year);
        int GetTotalBookCount();
        int GetTotalCopieCount();
        int Count(Book book);
    }
}
