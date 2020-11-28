using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Fraccion
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string barrio { get; set; }
        public string ciudad { get; set; }
        public string departamento { get; set; }
        public int cant_lotes { get; set; }
        public int disponibles { get; set; }
        public DateTime fecha_apertura { get; set; }
        public DateTime actualizado_al { get; set; }

        public ICollection<Propietario> Propietarios {get; set; }
        
    }
}
