using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    /// <summary>
    /// Clase encargada de contener datos de una imagen enviada para ser almacenada de azure storage
    /// </summary>
    public class AzureImageEntity
    {
        /// <summary>
        /// Nombre del archivo a guardar
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// string en base64 para convertirse en Stream
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Nombre del contenedor donde se guardará la imagen
        /// </summary>
        public string ContainerName { get; set; } = "userimages";
    }
}
