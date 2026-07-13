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
            _books = _filerepository.GetAllLine() ?? new List<Book>();
        }

        public void AddBook(Book book)
        {
            if(_books.Contains(book)) 
                throw new BookAlreadyExistsException();
            _books.Add( book );
            _filerepository.SaveAll(_books);
            
        }

        public void AddCopyBook(BookCopy book, int bookId)
        {
            Book? existingBook = _books.FirstOrDefault(x => x.Id == bookId) ?? throw new BookNotFoundException();
            existingBook.Copies.Add(book);

            _filerepository.SaveAll(_books);

           
        }

        public List<Book> AllBookByPublishedYear( int date)=>_books.Where(x => x.PublicationYear == date).ToList();

        public void DeleteBookById(int id)
        {
            Book? book = _books.FirstOrDefault(x => x.Id == id) ?? throw new BookNotFoundException();
            _books.Remove(book);
            _filerepository.SaveAll(_books);
        }

        public bool DeleteBookCopyByGuidId(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Copies.Any(c => c.Id == id));
            if(book!=null)
            {
                BookCopy ? copy   = book.Copies.FirstOrDefault(c => c.Id == id);
                book.Copies.Remove(copy);
                _filerepository.SaveAll(_books);
                return true; 
            }
            return false; /// ეს კარგად უნდა გავიგო,
        }

        public List<Book> GetAllBook()=>_books;
        public List<Book> GetAllBookCopyByAuthor(Author author)
        {
            return _books.Where(x => x.Author.Equals(author)).ToList();
        }
        public Book? GetBookById(int id)=> _books.FirstOrDefault(x => x.Id == id);

        public List<Book> GetBookByName(string name)=> _books.Where(x => x.Title.Trim().ToLower() == name.Trim().ToLower()).ToList();

        public BookCopy? GetBookCopyByGuid(Guid id) => _books.SelectMany(x => x.Copies).FirstOrDefault(y => y.Id == id);

        public int GetAllBookCount() => _books.Count;

        public int GetBookCountOfEveryExemplar() => _books.SelectMany(x=>x.Copies).Count();

       public int BookCount(Book book) => _books.Count(x => x.Equals(book));

       

       
    }
}
