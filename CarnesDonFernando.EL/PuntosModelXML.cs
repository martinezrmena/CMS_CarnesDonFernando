using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PuntosModelXML
    {
        public PuntosModelXML() { }

        public PuntosModelXML(PuntosEntity puntosEntity)
        {
            Id_Cliente = puntosEntity.Id_Cliente;
            Saldo = puntosEntity.Saldo;
            Tipo_Mov = puntosEntity.Tipo_Mov;
            Descripcion = puntosEntity.Descripcion;
            Fecha_Mov = puntosEntity.Fecha_Mov;
            Centro = puntosEntity.Centro;
        }

        public string Id_Cliente { get; set; } // identificador interno
        public string Saldo { get; set; } //saldo de puntos
        public string Tipo_Mov { get; set; } //codigo del movimiento
        public string Descripcion { get; set; } //descripcion de tipo de movimiento
        public string Fecha_Mov { get; set; } //fecha del movimiento
        public string Centro { get; set; } //codigo del centro (sucursal)

    }
}
