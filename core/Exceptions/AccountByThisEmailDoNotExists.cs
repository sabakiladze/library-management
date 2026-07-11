using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AccountByThisEmailDoNotExists:Exception
    {

        public AccountByThisEmailDoNotExists(string message) : base(message) { }

        public AccountByThisEmailDoNotExists() : base("Account by this email do not exists.") { }
     
        public AccountByThisEmailDoNotExists(string message, Exception inner) : base(message, inner) { }
    }

}
