using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class WEB_PAGOS
    {
        public string NUMERO_CONTRATO { get; set; }
        public int ID_FRACCION { get; set; }
        public string ID_MANZANA { get; set; }
        public int ID_LOTE { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public decimal MONTO_COBRADO_CUOTA { get; set; }
        public decimal MONTO_COBRADO_MORA { get; set; }
        public int CODIGO_RETORNO { get; set; }
        public string DESC_RETORNO { get; set; }
        public Int32 CODIGO_TRANS { get; set; }
        public string PAGO_GENERADO { get; set; }
        public DateTime FECHA_GENERADO { get; set; }
        public string COMPROBANTE { get; set; }
        public decimal CUOTA_COBRADA { get; set; }
        public decimal IVA_CUOTA { get; set; }
        public decimal MORA_COBRADA { get; set; }
        public decimal IVA_MORA { get; set; }

        
    }
}
