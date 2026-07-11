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
        //admini
        void AddBook(Book book);
        void DeleteBook(int id);
        bool DeleteBookCopy(Guid id);

        // მომხმარებლის/საჯარო მეთოდები
        void AddCopy(BookCopy copy, int bookId);

        List<Book> GetAllBooks();
        Book GetBookById(int id);
        List<Book> GetBooksByName(string name);
        List<Book> GetBooksByYear(int year);
        List<Book> GetBooksByAuthor(string author);
        BookCopy GetBookCopyByGuid(Guid id);

        // სტატისტიკა
        int GetTotalBookCount();
        int GetTotalCopiesCount();
        int BookCountById(int id);

    }
}
