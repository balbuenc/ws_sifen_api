using System;

namespace GoldenGateAPI.Entities
{
    public class Operation
    {
        public Int32 id_operacion { get; set; }
        public DateTime fecha  { get; set; }
        public string comando { get; set; }
        public string response { get; set; }
        public string status { get; set; }
        public string numero{ get; set; }

    }
}
