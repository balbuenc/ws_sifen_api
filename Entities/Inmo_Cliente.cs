using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Inmo_Cliente
    {
        public int ID_PERSONA { get; set; }
        public string DIRECCION_EMAIL { get; set; }
        public string DIRECCION_PARTICULAR { get; set; }
        public string DOCUMENTO { get; set; }
        public string ESTADO { get; set; }
        public string ESTADO_CIVIL { get; set; }
        public DateTime FECHA_BAJA { get; set; }
        public DateTime FECHA_INGRESO { get; set; }
        public DateTime FECHA_NACIMIENTO { get; set; }
        public string NACIONALIDAD { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string OBSERVACION { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string PRIMER_NOMBRE  { get; set; }
        public string RUC { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string SEXO { get; set; }
        public string TELEFONO_OFICINA { get; set; }
        public string TELEFONO_PARTICULAR { get; set; }
        public int TIPO_DOCUMENTO { get; set; }
        public string REFERENCIA_DIRECCION { get; set; }
        public int BARRIO { get; set; }
        public string INFORMCONF { get; set; }
        

    }
}
