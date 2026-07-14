using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Author
    {

        private string? _firstname;
        private string ?_lastname;

        public string ? FirstName
        {
            get => _firstname;
            set
            {
                if (!string.IsNullOrEmpty(_firstname))
                    _firstname = value;
            }

        }
        public string ? LastName
        {
            get => _lastname;
            set
            {
                if (!String.IsNullOrEmpty(value))
                    _lastname = value;
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Author other)
                return false;

            return FirstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase)
                && LastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName.ToLower(), LastName.ToLower());
        }
    }
}
