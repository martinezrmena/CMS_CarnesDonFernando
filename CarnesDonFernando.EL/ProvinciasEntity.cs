using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class ProvinciasEntity : TableEntity
    {
        public ProvinciasEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public ProvinciasEntity() { }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Descripcion { get; set; }
    }
}
