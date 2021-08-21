using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
   public class CantonesEntity : TableEntity
    {

        public CantonesEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public CantonesEntity() { }
        public string Canton { get; set; }
        public string Descripcion { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }

    }
}
