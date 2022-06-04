using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class Account
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string HashOfPassword { get; set; }
        public Roles Role { get; set; }
    }
}
