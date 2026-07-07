using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Models
{
    internal class Client:User
    {
        public double Fee { get; set; }
        public Client()
        {
            
        }
    }
}
