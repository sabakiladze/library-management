using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Enums
{
   public class BookStatus
    {
        public enum Book_Status
        {
            Available,
            Pending,
            Borrowed,
            Returned,
            Damaged
        }
    }
}
