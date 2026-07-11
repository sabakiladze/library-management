using Application.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Repositories;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IFileRepository<Book> _filerepository;
        private readonly List<Book> _books;

        public BookRepository(IFileRepository<Book> filerepository)
        {
            _filerepository = filerepository;
            _books = _filerepository.GetAllLine();
        }

        public void AddBook(Book book)
        {
            _books.Add( book );
            _filerepository.SaveAll(_books);
            
        }

        public BookCopy AddCopyBook(BookCopy book, int BookId)
        {
            Book bookcopy=_books.FirstOrDefault(x=>x.Id==BookId);
            bookcopy.Copies.Add(book);
            _filerepository.SaveAll(_books);
            return book;
        }

        public List<Book> AllBookByPublishedYear( int date)=>_books.Where(x => x.PublicationYear == date).ToList();

        public void DeleteBookById(int id)
        {
            Book book = _books.FirstOrDefault(x => x.Id == id);
            _books.Remove(book);
            _filerepository.SaveAll(_books);
            
        }

        public bool DeleteBookCopyByGuidId(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Copies.Any(c => c.Id == id));
            if(book!=null)
            {
                var copy = book.Copies.FirstOrDefault(c => c.Id == id);
                book.Copies.Remove(copy);
                _filerepository.SaveAll(_books);
                return true; 
            }
            return false; /// ეს კარგად უნდა გავიგო,
        }

        public List<Book> GetAllBook()=>_books;
        public List<Book> GetAllBookCopyByAuthor(string name)=> _filerepository.GetAllLine().Where(x => x.Author == name.Trim().ToLower()).ToList();

        public Book GetBookById(int id)=> _books.FirstOrDefault(x => x.Id == id);

        public List<Book> GetBookByName(string name)=> _books.Where(x => x.Title == name).ToList();

        public BookCopy GetBookCopyByGuid(Guid id) => _books.SelectMany(x => x.Copies).FirstOrDefault(y => y.Id == id);

        public int GetBookCount() => _books.Count;

        public int GetBookCountOfEveryExemplar() => _books.SelectMany(x=>x.Copies).Count();

       public int BookCountById(int id) => _books.Count(x => x.Id == id);

    }
}
