using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BookOutOfRange:Exception
    {
        public BookOutOfRange():base("this book is out of range") { }
        public BookOutOfRange(string message) : base(message) { }
        public BookOutOfRange(string message, Exception inner):base(message, inner) { }
    }
}
