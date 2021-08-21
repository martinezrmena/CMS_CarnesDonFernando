using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    /// <summary>
    /// Clase encargada de almacenar un cliente una vez obtenido de los servicios de CIISA
    /// </summary>
    public class ClientesModel
    {
        public decimal Id_Cliente { get; set; }
        public string Identif_Cliente { get; set; }
        public string Tipo_Persona { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Email { get; set; }
        public DateTime F_Nacimiento { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Direccion { get; set; }
        public decimal Sucursal { get; set; }
        public decimal Saldo_Puntos { get; set; }
    }
}
