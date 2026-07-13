using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RecordsDoNotExistsException:Exception
    {
        public RecordsDoNotExistsException():base("Records do not exists yes") { }
        public RecordsDoNotExistsException(string message) : base(message) { }
        public RecordsDoNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
