using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidUserIdOrUserHasNotHaveAnyRecordYetException:Exception
    {
        public InvalidUserIdOrUserHasNotHaveAnyRecordYetException() : base("Invalid UserId Or User Do Not Have Any Record Yet") { }
        public InvalidUserIdOrUserHasNotHaveAnyRecordYetException(string message) : base(message) { }
        public InvalidUserIdOrUserHasNotHaveAnyRecordYetException(string message, Exception inner):base(message, inner) { }
    }
}
