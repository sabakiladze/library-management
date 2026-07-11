using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RoleException : Exception
    {
        public RoleException():base("you do not have permission to do this.") { }
        public RoleException(string message) : base(message) { }
        public RoleException(string message, Exception inner):base( message,  inner) { }

    }
}
