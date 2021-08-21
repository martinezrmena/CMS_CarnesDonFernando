using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class ParametrizacionesModel
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string User { get; set; }
        public string Pass { get; set; }
        public string Client { get; set; }
        public int Port { get; set; }
        public int Time { get; set; }
        public string Time_Des { get; set; }
    }

    public class DatosParametrizacion
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string user { get; set; }
        public string pass { get; set; }
        public string client { get; set; }
        public int port { get; set; }
        public string accion { get; set; }
    }
}
