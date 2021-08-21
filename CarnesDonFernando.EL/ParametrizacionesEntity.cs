using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class ParametrizacionesEntity : TableEntity
    {

        public ParametrizacionesEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;

        }

        public ParametrizacionesEntity() { }

        public string User { get; set; }
        public string Pass { get; set; }
        public string Client { get; set; }
        public int Port { get; set; }
    }
}
