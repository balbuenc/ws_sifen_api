using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class PW_Lotes_Libres
    {
        public decimal ID_FRACCION { get; set; }
        public string NOMBRE { get; set; }
        public string BARRIO { get; set; }
        public string CIUDAD { get; set; }

        public string DEPARTAMENTO { get; set; }
        public int CANT_LOTES { get; set; }
        public int DISPONIBLES { get; set; }

        public DateTime FECHA_APERTURA { get; set; }
        public int ID_PROPIETARIO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string  TELEFONO { get; set; }
        public string  EMAIL { get; set; }
        public DateTime ACTUALIZADO_AL { get; set; }
    }
}
