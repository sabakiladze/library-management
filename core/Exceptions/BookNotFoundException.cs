using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BookNotFoundException:Exception
    {
        public BookNotFoundException() : base("Bokk by this Id do not exists") { }
        public BookNotFoundException(string message) : base(message) { }
        public BookNotFoundException(string message,  Exception inner) : base(message, inner) { }
        


    }
}
