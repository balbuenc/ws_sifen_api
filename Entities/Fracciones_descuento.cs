using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class Fracciones_descuento
    {
        public int id { get; set; }
        public int fraccion { get; set; }
        public decimal factor_descuento { get; set; }
    }
}
