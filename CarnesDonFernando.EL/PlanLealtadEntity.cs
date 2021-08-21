using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PlanLealtadEntity : TableEntity
    {

        public PlanLealtadEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public PlanLealtadEntity() { }
        public string Foto { get; set; }
        public string TipoCliente { get; set; }
        public string Descripcion { get; set; }
        public string Beneficios { get; set; }
        public string BonoNivel { get; set; }

    }
}
