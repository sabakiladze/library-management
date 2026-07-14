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
        Book? GetBookById(int id);
        List<Book> GetBookByName(string name);
        BookCopy? GetBookCopyByGuid(Guid id);
        void DeleteBookById(int id);
        List<Book> GetAllBooksByAuthor(Author author);
        int GetAllBookCount();
        int GetToTalCopiesCount();
        void AddBook(Book book);
        int Count(Book book);
        bool Exists(Book book);
        public void Update(Book book);
        public Book GetBookContainingCopy(Guid id);
        public List<Book> GetBooksByPublishedYear(int year);


    }
}
