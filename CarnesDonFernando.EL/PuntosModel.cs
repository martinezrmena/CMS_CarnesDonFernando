using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class PuntosModel
    {
        public decimal Id_Cliente { get; set; } // identificador interno
        public decimal Saldo { get; set; } //saldo de puntos
        public string Tipo_Mov { get; set; } //codigo del movimiento
        public string Descripcion { get; set; } //descripcion de tipo de movimiento
        public DateTime Fecha_Mov { get; set; } //fecha del movimiento
        public string Centro { get; set; } //codigo del centro (sucursal)

    }
}
