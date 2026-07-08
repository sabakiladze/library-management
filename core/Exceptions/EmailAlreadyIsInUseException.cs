using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class EmailAlreadyIsInUseException : Exception
    {
        public EmailAlreadyIsInUseException() : base("Account by this email already exists.") { }

        public EmailAlreadyIsInUseException(string message) : base(message) { }

        public EmailAlreadyIsInUseException(string message, Exception inner) : base(message, inner) { }
    }
}
