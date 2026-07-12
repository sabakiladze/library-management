using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBookService
    {
        void AddBook(Book book);

        void AddCopy(BookCopy copy, int bookId);

        int BookCountBy(Book book);

        void DeleteBook(int id);

        bool DeleteBookCopy(Guid id);

        List<Book> GetAllBooks();

        Book? GetBookById(int id);

        BookCopy? GetBookCopyByGuid(Guid id);

        List<Book> GetBooksByAuthor(Author author);

        List<Book> GetBooksByName(string name);

        List<Book> GetBooksByYear(int year);

        int GetTotalBookCount();

        int GetTotalCopiesCount();
    }
}
