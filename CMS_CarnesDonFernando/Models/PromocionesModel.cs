using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class PromocionesModel
    {
        public string Titulo { get; set; }
        public string Fecha_Publicacion { get; set; }
        public string Fecha_Finalizacion { get; set; }
        public string ImagenUrl { get; set; }
        public string Enlace { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
    
    public class DatosPromocion
    {
        public string titulo { get; set; }
        public string enlace { get; set; }
        public IFormFile imagen { get; set; }
        public string imagenPath { get; set; }
        public string fechainicia { get; set; }
        public string fechafin { get; set; }
        public string accion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
