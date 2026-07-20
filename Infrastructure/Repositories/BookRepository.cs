using Application.Interfaces.Repositories;
using Domain.Exceptions;
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
        private readonly IFileRepository<Book> _fileRepository;
        private readonly List<Book> _books;

        public BookRepository(IFileRepository<Book> fileRepository)
        {
            _fileRepository = fileRepository;
            _books = _fileRepository.GetAllLine() ?? new List<Book>();
            Book.SyncIdCounter(_books);
        }


        public void AddBook(Book book)
        {
            if (Exists(book))
                throw new BookAlreadyExistsException();

            _books.Add(book);
            Save();
        }


        public void DeleteBookById(int id)
        {
            var book = GetBookById(id) ?? throw new BookNotFoundException();

            _books.Remove(book);
            Save();
        }


        public List<Book> GetAllBook() => _books;


        public Book? GetBookById(int id) =>
            _books.FirstOrDefault(x => x.Id == id);


        public List<Book> GetBookByName(string name) =>
            _books.Where(x =>
                x.Title.Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();


        public List<Book> GetAllBooksByAuthor(Author author) =>
            _books.Where(x => x.Author.Equals(author)).ToList();


        public List<Book> GetBooksByPublishedYear(int year) =>
            _books.Where(x => x.PublicationYear == year).ToList();


        public BookCopy? GetBookCopyByGuid(Guid id) =>
            _books.SelectMany(x => x.Copies)
                  .FirstOrDefault(x => x.Id == id);


        public Book GetBookContainingCopy(Guid id) =>
            _books.FirstOrDefault(x =>
                x.Copies.Any(c => c.Id == id))
            ?? throw new BookNotFoundException();


        public int GetAllBookCount() =>
            _books.Count;


        public int GetTotalCopiesCount() =>
            _books.SelectMany(x => x.Copies).Count();


        public int Count(Book book) =>
            _books.Count(x => x.Equals(book));


        public bool Exists(Book book) =>
            _books.Contains(book);


        public void Update(Book book)
        {
            var index = _books.FindIndex(x => x.Id == book.Id);

            if (index == -1)
                throw new BookNotFoundException();

            _books[index] = book;

            Save(); // რადგან copu ს სტატუსი შეიცვალა და copy არის book ის ელემენტი მთლიანი წიგნის ობიექტი უნდა დავააფდეითოდ. 
        }
        public int GetToTalCopiesCount()
        {
            return _books
                .SelectMany(x => x.Copies)
                .Count();
        }
        private void Save() =>
            _fileRepository.SaveAll(_books);

       
    }
}
