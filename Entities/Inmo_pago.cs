using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Inmo_Pago
    {
        public Int32 ID_MOVIMIENTO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_PAGO { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public int ID_FRACCION { get; set; }
        public int ID_LOTE { get; set; }
        public string ID_MANZANA { get; set; }
    
        public decimal IMPORTE_CHEQUE { get; set; }
        public decimal IMPORTE_EFECTIVO { get; set; }
        public decimal IMPORTE_MORA { get; set; }
        public string NRO_COMPROBANTE { get; set; }
        public int NUMERO_CUOTA { get; set; }
        public string TIPO_CUOTA { get; set; }
        public string TIPO_PAGO { get; set; }
        public int ID_COBRADOR { get; set; }
        public int MES_ATRAZO { get; set; }
        public string TRANSFERIDO { get; set; }
        public decimal IMPORTE_MORA_EXONERADA { get; set; }
        public int ID_CAJA { get; set; }
        public int ID_SUCURSAL { get; set; }
        public string LIQUIDADO_VENDEDOR { get; set; }
        public DateTime FECHA_LIQ_VENDEDOR { get; set; }
        public string LIQUIDADO_COBRADOR { get; set; }
        public DateTime FECHA_LIQ_COBRADOR { get; set; }
        public string LIQUIDADO_PROPIETARIO { get; set; }
        public DateTime FECHA_LIQ_PROPIETARIO { get; set; }
        public string LIQUIDADO_JEFE_VENTA { get; set; }
        public DateTime FECHA_LIQ_JEFE_VENTA { get; set; }
        public decimal TOTAL_DEVENGADO { get; set; }
        public string SECUENCIA { get; set; }
        public decimal IMPORTE_CUOTA_EXONERADA { get; set; }
        
        public decimal COMISION_PROP_CUOTA { get; set; }
        public decimal COMISION_PROP_AJUSTE { get; set; }
        public decimal IVA_CUOTAS { get; set; }
        public decimal IVA_MORA { get; set; }
        public string USUARIO_CARGADOR { get; set; }
        public string TIPO_COMPROBANTE { get; set; }
        public string LIQUIDADO_CAPTADOR { get; set; }
        public DateTime FECHA_LIQ_CAPTADOR { get; set; }
        public decimal COMISION_CAPT_CUOTA { get; set; }
        public decimal COMISION_CAPT_AJUSTE { get; set; }
        public string TIPO_COBRO_VARIOS { get; set; }
        public decimal IMPORTE_COBRO_VARIOS { get; set; }
        public DateTime FECHA_LIQ_GERENTE_VTA { get; set; }
        public string LIQUIDADO_GERENTE_VTA { get; set; }
        public int ID_COBRADOR_COMPARTIDO { get; set; }
        public decimal PORC_COMISION_COB_COMP { get; set; }
        public int NRO_CUOTA_INTERNA { get; set; }
        public int NUMERO_CONTRATO { get; set; }
        public DateTime FECHA_REAL_PAGO { get; set; }


    }
}
