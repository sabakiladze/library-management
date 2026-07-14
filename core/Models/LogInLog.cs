using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LogInLog
    {
        public string Email { get; private set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;

        [JsonConstructor]
        public LogInLog(string email)
        {
            Email = email;
        }
        public LogInLog()
        {
            
        }
    }
}
