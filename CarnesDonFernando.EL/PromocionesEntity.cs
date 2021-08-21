using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PromocionesEntity : TableEntity
    {
        public PromocionesEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;

        }

        public PromocionesEntity() { }
        public string Titulo { get; set; }
        public string Fecha_Publicacion { get; set; }
        public string Fecha_Finalizacion { get; set; }
        public string ImagenUrl { get; set; }
        public string Enlace { get; set; }
    }
}
