using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Inmo_Lote
    {
        public int ID_LOTE { get; set; }
        public int ID_FRACCION { get; set; }
        public string ID_MANZANA { get; set; }
        public int ID_CLIENTE { get; set; }

        public int CANT_DIAS_RESERVA { get; set; }
        public int CODIGO_RESERVA { get; set; }

        public decimal COSTO_LOTE { get; set; }
        public string CTA_CTE_CTRAL { get; set; }
        public decimal DIMENSION { get; set; }
        public decimal DIMENSION_ESTE { get; set; }

        public decimal DIMENSION_NORTE { get; set; }

        public decimal DIMENSION_OESTE { get; set; }
        public decimal DIMENSION_SUR { get; set; }
        public string ESTADO { get; set; }

        public DateTime FECHA_CHEQUERA { get; set; }
        public DateTime FECHA_ESTADO { get; set; }
        public DateTime FECHA_INGRESO { get; set; }
        public int ID_COMISION { get; set; }
        public string LINDERO_ESTE { get; set; }
        public string LINDERO_NORTE { get; set; }
        public string LINDERO_OESTE { get; set; }
        public string LINDERO_SUR { get; set; }
        public decimal MONTO_CUOTA { get; set; }
        public string NRO_FINCA { get; set; }

        public int NUMERO_CONTRATO { get; set; }
        public string OBSERVACION { get; set; }
        public int PLAZO { get; set; }
        public decimal PRECIO_CONTADO { get; set; }
        public decimal PRECIO_FINANCIADO { get; set; }

        public int ID_VENDEDOR { get; set; }
        public decimal PRECIO_VENTA_FINAL { get; set; }
        public decimal TOTAL_PAGADO { get; set; }
        public decimal SALDO_DEUDOR { get; set; }
        public int ULTIMA_CUOTA_PAGADA { get; set; }
        public DateTime FECHA_ULTIMO_PAGO { get; set; }
        public int CUOTA_REFUERZO { get; set; }
        public decimal PAGO_A_CUENTA { get; set; }
        public decimal PORCENTAJE_MORA { get; set; }
        public string ID_MONEDA { get; set; }
        public string NOMBRE_PARA_DOCUMENTO { get; set; }
        public decimal ENTREGA_INICIAL { get; set; }
        public string USER_AUTORIZADOR { get; set; }
        public DateTime FECHA_AUTORIZACION { get; set; }
        public  DateTime HORA_AUTORIZACION  { get; set; }
        public int ID_JEFE_VENTA { get; set; }
        public int NRO_DOCUMENTO_ENTREGA_INICIAL { get; set; }

        public int RGP_NRO_SECCION { get; set; }
        public int RGP_NUMERO { get; set; }
        public int RGP_FOLIO { get; set; }
        public int RGP_ANIO_INCRIPCION { get; set; }
        public decimal TOTAL_DEVENGADO { get; set; }
        public int ID_COMISION_JEFE_VENTA { get; set; }
        public DateTime FECHA_INCRIPCION_MUNIC { get; set; }
        public int MES_ATRASO { get; set; }

        public int ID_COMISION_PROPIETARIO { get; set; }
        public DateTime FECHA_VENTA { get; set; }
        public int ID_ORDEN { get; set; }
        public int MES_PARA_VENCIMIENTO { get; set; }
        public decimal PRECIO_REAL_VENTA { get; set; }
        public decimal IVA_CUOTA { get; set; }
        public int ID_COMISION_CAPTADOR { get; set; }
        public int NRO_RESOLUCION_MUNIC { get; set; }
        public DateTime FECHA_ORDEN_ESCRITURACION { get; set; }
        public string ESTADO_CONTRATO { get; set; }
        public DateTime FECHA_ESTADO_CONTRATO { get; set; }
        public string MENSAJE_PARA_CLIENTE { get; set; }
        public DateTime RGP_FECHA_INSCRIPCION { get; set; }
        public int ID_GERENTE_VENTA { get; set; }
        public string TIPO_VENTA { get; set; }
        public decimal IMPORTE_ENTREGA_INICIAL { get; set; }
        public int CUOTA_ENTREGA_INICIAL { get; set; }
        public string FORMA_PAGO { get; set; }
        public int ID_MEJORA { get; set; }
        public string FRACCION { get; set; }
        public string MANZANA { get; set; }
        public string LOTE { get; set; }
        public string CONTRATO_FIRMADO { get; set; }

        
    }
}
