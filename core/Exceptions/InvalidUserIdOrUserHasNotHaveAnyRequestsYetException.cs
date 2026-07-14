using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidUserIdOrUserHasNotHaveAnyRequestsYetException:Exception
    {
        public InvalidUserIdOrUserHasNotHaveAnyRequestsYetException() : base("Invalid UserId Or User Do Not Have Any Record Yet") { }
        public InvalidUserIdOrUserHasNotHaveAnyRequestsYetException(string message) : base(message) { }
        public InvalidUserIdOrUserHasNotHaveAnyRequestsYetException(string message, Exception inner) : base(message, inner) { }
    }
}

