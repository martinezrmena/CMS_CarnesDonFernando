using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class PlanLealtadModel
    {
        public string Foto { get; set; }
        public string TipoCliente { get; set; }
        public string Descripcion { get; set; }
        public string Beneficios { get; set; }
        public string BonoNivel { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }

    public class DatosPlanLealtad
    {
        public string tipoCliente { get; set; }
        public IFormFile image { get; set; }
        public string imagenPath { get; set; }
        public string descripcion { get; set; }
        public string beneficios { get; set; }
        public string bono { get; set; }
        public string accion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
