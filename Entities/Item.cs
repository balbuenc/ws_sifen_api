using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Item
    {
                   public int id_item { get; set; }
                  public int id_documento_electronico { get; set; }
                  public string codigo { get; set; }
                  public string descripcion { get; set; }
                  public string observacion { get; set; }
                  public string partidaArancelaria { get; set; }
                  public string ncm { get; set; }
                  public string unidadMedida { get; set; }
                  public decimal cantidad { get; set; }
                  public decimal precioUnitario { get; set; }
                  public decimal cambio { get; set; }
                  public decimal descuento { get; set; }
                  public decimal anticipo { get; set; }
                  public string pais { get; set; }
                  public string tolerancia { get; set; }
                  public string toleranciaCantidad { get; set; }
                  public string toleranciaPorcentaje { get; set; }
                  public string cdcAnticipo { get; set; }
                  public string ivaTipo { get; set; }
                  public decimal ivaBase { get; set; }
                  public decimal iva { get; set; }
                  public string lote { get; set; }
                  public DateTime vencimiento { get; set; }
                  public string numeroSerie { get; set; }
                  public string numeroPedido { get; set; }
                  public string numeroSeguimiento { get; set; }
    }
}
