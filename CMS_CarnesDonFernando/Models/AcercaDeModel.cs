using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class AcercaDeModel
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }

    public class DatosAcercaDe
    {
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
