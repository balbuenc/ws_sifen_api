using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Inmo_Fraccion
    {
        public decimal ID_FRACCION { get; set; }
        public string NOMBRE_FRACCION { get; set; }
        public int ID_CIUDAD { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
        public DateTime ACTUALIZADO_AL { get; set; }
    }
}
