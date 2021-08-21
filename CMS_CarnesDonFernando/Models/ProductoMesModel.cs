using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class ProductoMesModel
    {
        public string Titulo { get; set; }
        public string Fecha_Publicacion { get; set; }
        public string Fecha_Finalizacion { get; set; }
        public string ImagenUrl { get; set; }
        public string Descripcion_Resumen { get; set; }
        public string Descripcion_Detalle { get; set; }
        public string Preparacion { get; set; }
        public string UrlIconosPreparacion { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }

    public class DatosProductos
    {
        public string titulo { get; set; }
        public IFormFile imagen { get; set; }
        public string imagenPath { get; set; }
        public string fechainicia { get; set; }
        public string fechafin { get; set; }
        public string accion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string descripcion_Resumen { get; set; }
        public string descripcion_Detalle { get; set; }

        public string preparacion { get; set; }
       
    }

}



