using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LogInLog
    {
        public string Email { get; private set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;
        public LogInLog(string email)
        {
            Email = email;
        }
        public LogInLog()
        {
            
        }
    }
}
