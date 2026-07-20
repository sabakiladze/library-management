using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IBookService
    {
        void AddBook(Book book);

        void AddCopy(BookCopy copy, int bookId);

        void DeleteBook(int id);

        void DeleteCopy(Guid copyId);

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
