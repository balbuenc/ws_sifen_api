using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Dte
    {

        public string cdc { get; set; }

        public int id_documento_electronico { get; set; }

        public int tipoDocumento { get; set; }
        public string establecimiento { get; set; }

        public string codigoSeguridadAleatorio { get; set; }
        public string punto { get; set; }
        public string numero { get; set; }
        public string descripcion { get; set; }
        public string observacion { get; set; }
        public int tipoContribuyente { get; set; }
        public DateTime fecha { get; set; }
        public int tipoEmision { get; set; }
        public int tipoTransaccion { get; set; }
        public int tipoImpuesto { get; set; }
        public string moneda { get; set; }
        public int condicionAnticipo { get; set; }
        public int condicionTipoCambio { get; set; }
        public decimal cambio { get; set; }
        public int id_cliente { get; set; }
        public int id_usuario { get; set; }
        public int id_certificado { get; set; }
        public string data { get; set; }
        public int id_estado { get; set; }

        public string tipoEmisionDescripcion { get; set; }
        public string estado { get; set; }
        public string TipoImpuestoDescripcion { get; set; }
        public string TipoDocumentoElectronicoDescripcion { get; set; }
        public string denominacion { get; set; }
        public string PuntoExpedicionDescripcion { get; set; }
        public string razonSocial { get; set; }
        public string tipoTransaccionDescripcion { get; set; }


        public int condicionOperacion { get; set; }
        public string condicionDescripcion { get; set; }
        public int condicionCredito { get; set; }
        public string creditoDescripcion { get; set; }

        public string plazoCredito { get; set; }

        public int presencia { get; set; }
        public int tipoPago { get; set; }


        //CLIENTE
        public int cli_contribuyente { get; set; }
        public string cli_ruc { get; set; }
        public string cli_razonSocial { get; set; }
        public string cli_nombreFantasia { get; set; }
        public int cli_tipoOperacion { get; set; }
        public string cli_direccion { get; set; }
        public string cli_numeroCasa { get; set; }
        public int cli_departamento { get; set; }
        public int cli_distrito { get; set; }
        public int cli_ciudad { get; set; }
        public string cli_pais { get; set; }
        public int cli_tipoContribuyente { get; set; }
        public int cli_documentoTipo { get; set; }
        public string cli_documentoNumero { get; set; }
        public string cli_telefono { get; set; }
        public string cli_celular { get; set; }
        public string cli_email { get; set; }
        public string cli_codigo { get; set; }


    }
}
