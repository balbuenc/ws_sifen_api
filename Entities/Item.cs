using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Item
    {
                  public Int32 id_item { get; set; }
                  public Int32 id_documento_electronico { get; set; }
                  public string codigo { get; set; }
                  public string descripcion { get; set; }
                  public string observacion { get; set; }
                  public Int32 partidaArancelaria { get; set; }
                  public string ncm { get; set; }
                  public int unidadMedida { get; set; }
                  public decimal cantidad { get; set; }
                  public decimal precioUnitario { get; set; }
                  public decimal cambio { get; set; }
                  public decimal descuento { get; set; }
                  public decimal anticipo { get; set; }
                  public string pais { get; set; }
                  public int tolerancia { get; set; }
                  public int toleranciaCantidad { get; set; }
                  public decimal toleranciaPorcentaje { get; set; }
                  public string cdcAnticipo { get; set; }
                  public int ivaTipo { get; set; }
                  public decimal ivaBase { get; set; }
                  public decimal iva { get; set; }
                  public string lote { get; set; }
                  public DateTime vencimiento { get; set; }
                  public string numeroSerie { get; set; }
                  public string numeroPedido { get; set; }
                  public string numeroSeguimiento { get; set; }
    }
}
