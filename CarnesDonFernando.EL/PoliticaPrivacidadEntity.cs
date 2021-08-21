using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PoliticaPrivacidadEntity : TableEntity
    {


        public PoliticaPrivacidadEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public PoliticaPrivacidadEntity() { }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

    }
}
