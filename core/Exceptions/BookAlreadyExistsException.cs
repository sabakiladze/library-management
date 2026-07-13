using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BookAlreadyExistsException:Exception
    {
        public BookAlreadyExistsException() : base("book by this id already exists") { }
        public BookAlreadyExistsException(string message) : base(message) { }
        public BookAlreadyExistsException(string message, Exception inner):base(message, inner) { }
        
    }
}
