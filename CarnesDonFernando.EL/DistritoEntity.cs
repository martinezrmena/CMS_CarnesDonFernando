using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class DistritoEntity : TableEntity
    {

        public DistritoEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public DistritoEntity(DistritoModel distritoModel)
        {
            this.Distrito = distritoModel.Distrito;
            this.Descripcion = distritoModel.Descripcion;
            this.Canton = distritoModel.Canton;
            this.PartitionKey = Guid.NewGuid().ToString();
            this.RowKey = Guid.NewGuid().ToString();
        }

        public DistritoEntity() { }
        public string Distrito { get; set; }
        public string Descripcion { get; set; }
        public string Canton { get; set; }
    }
}
