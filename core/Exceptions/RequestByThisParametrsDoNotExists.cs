using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RequestByThisParametrsDoNotExists:Exception
    {
        public RequestByThisParametrsDoNotExists() : base("Request by thid Parametrs do not exists") { }
        public RequestByThisParametrsDoNotExists(string message) : base(message) { }
        public RequestByThisParametrsDoNotExists(string message, Exception inner) : base(message, inner) { }

    }
}
