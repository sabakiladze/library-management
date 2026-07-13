using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidUserIdOrUserHasNotHaveAnyRecordYet:Exception
    {
        public InvalidUserIdOrUserHasNotHaveAnyRecordYet() : base("Invalid UserId Or User Do Not Have Any Record Yet") { }
        public InvalidUserIdOrUserHasNotHaveAnyRecordYet(string message) : base(message) { }
        public InvalidUserIdOrUserHasNotHaveAnyRecordYet(string message, Exception inner):base(message, inner) { }
    }
}
