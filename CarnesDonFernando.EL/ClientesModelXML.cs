using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    /// <summary>
    /// Clase encargada de obtener los datos de un cliente con los nombres de campos que entregan los servicios de CIISA
    /// </summary>
    public class ClientesModelXML
    {
        public string Id_Cliente { get; set; }
        public string Identif_Cliente { get; set; }
        public string Tipo_Persona { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Email { get; set; }
        public string F_Nacimiento { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Direccion { get; set; }
        public string Sucursal { get; set; }
        public string Saldo_Puntos { get; set; }
    }
}
