using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class AcercaDeEntity : TableEntity
    {
        public AcercaDeEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;

        }

        public AcercaDeEntity() { }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
