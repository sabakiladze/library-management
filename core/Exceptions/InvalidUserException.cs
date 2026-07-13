using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidUserException:Exception
    {

        public InvalidUserException() : base("invalid User") { }
        public InvalidUserException(string message) : base(message) { }
        public InvalidUserException(string message, Exception inner) : base(message, inner) { }
    }
}
