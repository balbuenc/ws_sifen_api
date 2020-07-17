using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

    }
}
