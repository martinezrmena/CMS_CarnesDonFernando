using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
   public class SucursalesEntity : TableEntity
    {
        public SucursalesEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public SucursalesEntity() { }
        public string FotoUrl { get; set; }
        public string Nombre { get; set; }
        public string EnlaceGoogleMaps { get; set; }
        public string IconoGoogleMaps { get; set; }
        public string EnlaceWaze { get; set; }
        public string IconoWaze { get; set; }
        public string TelefonoTienda { get; set; }
        public string TelefonoRestaurante { get; set; }
        public string HorarioTienda { get; set; }
        public string HorarioRestaurante { get; set; }
        public string Telefonos { get; set; }

    }

   public class SucursalEntity : TableEntity
    {
        public SucursalEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public SucursalEntity(SucursalModel sucursal)
        {
            this.PartitionKey = Guid.NewGuid().ToString();
            this.RowKey = Guid.NewGuid().ToString();
            this.Centro = sucursal.Centro;
            this.Descripcion_Centro = sucursal.Descripcion_Centro;
        }

        public SucursalEntity() { }
        public string Centro { get; set; }
        public string Descripcion_Centro { get; set; }
    }
}
