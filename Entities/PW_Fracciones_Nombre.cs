using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class PW_Fracciones_Nombre
    {
        public int ID_FRACCION { get; set; }
        public string NOMBRE_FRACCION { get; set; }
        public int ID_CIUDAD { get; set; }
        public int ID_DEPARTAMENTO { get; set; }
        public string NOMBRE_DEPARTAMENTO { get; set; }
        public DateTime ACTUALIZADO_AL { get; set; }
        public string NOMBRE_CIUDAD { get; set; }
       
    }
}
