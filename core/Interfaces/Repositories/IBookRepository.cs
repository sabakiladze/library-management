using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task InitializeAsync();

        List<Book> GetAllBook();
        Book? GetBookById(int id);
        List<Book> GetBookByName(string name);
        BookCopy? GetBookCopyByGuid(Guid id);
        List<Book> GetAllBooksByAuthor(Author author);
        int GetAllBookCount();
        int GetToTalCopiesCount();
        int Count(Book book);
        bool Exists(Book book);
        Book GetBookContainingCopy(Guid id);
        List<Book> GetBooksByPublishedYear(int year);

        Task AddBookAsync(Book book);
        Task DeleteBookByIdAsync(int id);
        Task UpdateAsync(Book book);
    }
}
