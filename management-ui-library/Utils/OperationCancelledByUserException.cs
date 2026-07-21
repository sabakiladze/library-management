using System;

namespace management_ui_library.Utils
{
    public class OperationCancelledByUserException : Exception
    {
        public OperationCancelledByUserException() : base("Cancelled by user.") { }
    }
}
