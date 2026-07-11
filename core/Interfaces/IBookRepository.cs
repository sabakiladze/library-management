using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using LibraryManagementSystem.Domain.Models;


namespace Application.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBook();
        Book GetBookById(int id);
        List<Book> GetBookByName(string name);
        BookCopy GetBookCopyByGuid(Guid id);
        void DeleteBookById(int id);
        bool DeleteBookCopyByGuidId(Guid id);

        List<Book> GetAllBookCopyByAuthor(string name);

        int GetBookCount();
        int GetBookCountOfEveryExemplar();
        void AddBook(Book book);
        BookCopy AddCopyBook(BookCopy book, int BookId);
        List<Book> AllBookByPublishedYear(int date);
        int BookCountById(int id);
        
    }
}
