using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
   public class UserIsAlreadyAdmin:Exception
    {
        public UserIsAlreadyAdmin () : base("User is already Admin") { }
        public UserIsAlreadyAdmin(string message) : base(message) { }
        public UserIsAlreadyAdmin(string message, Exception inner) : base(message, inner) { }
    }
}
