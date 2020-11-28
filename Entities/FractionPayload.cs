using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Entities
{
    public class FractionPayload
    {
        public int lotes_libres  { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string nombre{ get; set; }
        public string buscar { get; set; }
        public string actualizado_al { get; set; }
    }
}
