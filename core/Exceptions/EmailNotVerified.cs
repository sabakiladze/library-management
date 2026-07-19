using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public  class EmailNotVerified:Exception
    {
        public EmailNotVerified():base("Verifie email first") { }
        public EmailNotVerified(string message) : base(message) { }
        public EmailNotVerified(string message, Exception inner) : base(message, inner) { }

    }
}
