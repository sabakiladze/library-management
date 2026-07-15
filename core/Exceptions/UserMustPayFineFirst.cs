using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserMustPayFineFirst :Exception
    {
        public UserMustPayFineFirst():base("User Must Pay Fine First") { }
        public UserMustPayFineFirst(string message) : base(message) { }
        public UserMustPayFineFirst(string message, Exception inner) : base(message, inner) { }
    }
}
