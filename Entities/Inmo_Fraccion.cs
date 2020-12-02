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

    
        public int CANT_MANZANA {get; set;} 
        
        public decimal DIMENSION {get; set;} 
        public string ESTADO {get; set;} 
        
        public DateTime FECHA_BAJA {get; set;} 
        
        public DateTime FECHA_INGRESO {get; set;} 
        
        public string NRO_FINCA {get; set;} 
        public string OBSERVACION {get; set;} 
        public string TIPO_FRACCION {get; set;}  
        
    }
}
