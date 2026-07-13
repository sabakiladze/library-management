using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RequestByThisIdDoNotExists : Exception
    {
        public RequestByThisIdDoNotExists() : base("Request bu this ID do not exists") { }
        public RequestByThisIdDoNotExists(string message) : base(message) { }
        public RequestByThisIdDoNotExists(string message, Exception inner) : base(message, inner) { }
    }
}
