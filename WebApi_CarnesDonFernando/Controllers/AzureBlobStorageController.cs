using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarnesDonFernando.BL;
using System.IO;
using Newtonsoft.Json.Linq;
using CarnesDonFernando.EL;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar las imagenes de los Blob (contenedores) de Azure Storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/AzureBlobStorage")]
    [ApiController]
    public class AzureBlobStorageController : ControllerBase
    {
        private readonly AzureBlobStorageBL _azureblobBL = new AzureBlobStorageBL();

        /// <summary>
        /// Permite guardar una imagen en el contenedor de Azure Storage de forma sincrona.
        /// </summary>
        /// <param name="data">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite subir imagen
        ///     {
        ///        "data": [
        ///                             {
        ///                                 "UrlLocal": "NombreArchivo.jpg",
        ///                                 "NameContainer": "NombreArchivo.jpg"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>URL donde se ha almacenado la imagen.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("SubirImagenBlob")]
        public async Task<string> SubirImagen([FromBody] AzureBlobEntity data)
        {
            try
            {
                var resultado = await _azureblobBL.SubirImagenBlob(data);

                return resultado;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Permite guardar una imagen en el contenedor de Azure Storage.
        /// </summary>
        /// <param name="jsonData">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite subir imagen
        ///     {
        ///        "jsonData": [
        ///                             {
        ///                                 "PathImage": "C:\PROYECTOS", //Path - Dirección física de la imagen
        ///                                 "ContainerName": "userimages"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>URL donde se ha almacenado la imagen.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("SubirImagen")]
        public async Task<string> SubirImagen([FromBody]JObject jsonData)
        {
            string strPath = string.Empty; 
            string strContainerName = string.Empty;
            try
            {
                strPath = jsonData["PathImage"].Value<string>();
                strContainerName = jsonData["ContainerName"].Value<string>();
               
                var resultado = await _azureblobBL.SubirImagen(strPath, strContainerName);

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
               
            }
        }

        /// <summary>
        /// Permite guardar una imagen en el contenedor de Azure Storage.
        /// </summary>
        /// <param name="ImageEntity">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite subir imagen
        ///     {
        ///        "ImageEntity": [
        ///                             {
        ///                                 "FileName": "NombreArchivo.jpg",
        ///                                 "Code": "XXXXXXXXXXXXXXX", //En formato string base64
        ///                                 "ContainerName": "userimages"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>URL donde se ha almacenado la imagen.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("SubirImagenBase")]
        public async Task<string> SubirImagenBase([FromBody] AzureImageEntity ImageEntity) 
        {
            try
            {
                var resultado = await _azureblobBL.SubirImagenBase(ImageEntity.Code, ImageEntity.FileName, ImageEntity.ContainerName);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        /// <summary>
        /// Permite guardar una imagen en el contenedor de Azure Storage.
        /// </summary>
        /// <param name="jsonData">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite subir imagen
        ///     {
        ///        "jsonData": [
        ///                             {
        ///                                 "streamFile": "XXXXXXXXXXXXXXX", //Stream de la imagen
        ///                                 "_sFileName": "NombreArchivo.jpg",
        ///                                 "_sContainerName": "userimages"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>URL donde se ha almacenado la imagen.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("SubirImagenApp")]
        public async Task<string> SubirBlobAzure([FromBody]JObject jsonData)
        {
            try
            {
                string streamFile = jsonData["streamFile"].Value<string>();
                string _sFileName = jsonData["_sFileName"].Value<string>();
                string _sContainerName = jsonData["_sContainerName"].Value<string>();

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(streamFile);
                writer.Flush();

                var resultado = await _azureblobBL.SubirBlobAzure(writer.BaseStream, _sFileName, _sContainerName);

                return resultado;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Permite eliminar una imagen del contenedor (Blob) de Azure Storage.
        /// </summary>
        /// <param name="_sFileUrl">URL de la imagen a ser eliminada</param>
        /// <param name="_sContainerName">Nombre del contenedor que contiene la imagen a ser eliminada</param>
        /// <returns>Estado del proceso una vez terminado.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [Route("BorrarImagen")]
        [HttpPost]
        public async Task<bool> BorrarImagen(string _sFileUrl, string _sContainerName)
        {
            try
            {
                var resultado = await _azureblobBL.BorrarImagen(_sFileUrl, _sContainerName);

                return resultado;
            }
            catch
            {
                throw new Exception();
            }
        }

    }
}