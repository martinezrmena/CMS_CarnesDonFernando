using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class UsuariosEntity : TableEntity
    {

        public UsuariosEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;

        }

        public UsuariosEntity() { }

        public string Cedula { get; set; }
        public string Password { get; set; }
        public string Tipo_Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Fecha_Nacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Provincia { get; set; }
        public string CodigoProvincia { get; set; }
        public string Canton { get; set; }
        public string CodigoCanton { get; set; }
        public string Distrito { get; set; }
        public string CodigoDistrito { get; set; }
        public string Direccion_Exacta { get; set; }
        public string Sucursal1 { get; set; }
        public string CodigoSucursal1 { get; set; }
        public string Codigo_Invitacion { get; set; }
        public string PictureURL { get; set; }
        public string PictureName { get; set; }
        public string Puntos { get; set; }
        public bool Sincronizado { get; set; }

        public bool CheckedPolitica { get; set; }

        public bool CheckedMensaje { get; set; }

    }
}
