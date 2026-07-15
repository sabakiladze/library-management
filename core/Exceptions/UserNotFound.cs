using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFound:Exception
    {
        public UserNotFound() : base("User could not found") { }
        public UserNotFound(string message) : base(message) { }
        public UserNotFound(string message, Exception inner):base(message, inner) { }
        
    }
}
