using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class WEB_CUOTAS
    {
        public Int32 NUMERO_CONTRATO { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public Int32 ID_FRACCION { get; set; }
        public string ID_MANZANA { get; set; }
        public int ID_LOTE { get; set; }
        public int NUMERO_CUOTA { get; set; }
        public decimal MONTO_CUOTA { get; set; }
        public int MESES_MORA { get; set; }
        public decimal MONTO_MORA { get; set; }
        public int CODIGO_RETORNO { get; set; }
        public string DESC_RETORNO { get; set; }
        public Int32 CODIGO_TRANS { get; set; }
        public Int32 DOCUMENTO { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public string DESCRIPCION_FRACCION { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
    }
}
