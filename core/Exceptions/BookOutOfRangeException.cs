using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BookOutOfRangeException:Exception
    {
        public BookOutOfRangeException():base("this book is out of range") { }
        public BookOutOfRangeException(string message) : base(message) { }
        public BookOutOfRangeException(string message, Exception inner):base(message, inner) { }
    }
}
