using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Propietario
    {
        public int Id { get; set; }
        public string nombres { get; set; }
        public string  apellidos { get; set; }
        public string  telefono { get; set; }
        public string  email { get; set; }
    }
}
