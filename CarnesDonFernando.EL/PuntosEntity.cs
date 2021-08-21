using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PuntosEntity : TableEntity
    {
        public PuntosEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public PuntosEntity() { }

        public string Id_Cliente { get; set; } // identificador interno
        public string Cedula { get; set; }   //Cedula del cliente
        public string Saldo { get; set; } //saldo de puntos
        public string Tipo_Mov { get; set; } //codigo del movimiento
        public string Descripcion { get; set; } //descripcion de tipo de movimiento
        public string Fecha_Mov { get; set; } //fecha del movimiento
        public string Centro { get; set; } //codigo del centro (sucursal)

    }
}
