using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApi_CarnesDonFernando.Helpers;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar los puntos de los clientes en la aplicación.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Puntos")]
    [ApiController]
    public class PuntosAppController : ControllerBase
    {
        PuntosBL puntosBL = new PuntosBL();

        #region Consultas Puntos de clientes BD Carnes Don Fernando
        /// <summary>
        /// Permite obtener la lista de puntos en relación a un cliente en Azure.
        /// </summary>
        /// <param name="jsonData">Objeto de tipo JSON que permite enviar la cédula del cliente.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "jsonData": [
        ///                             {
        ///                                 "Cedula": "123456789"
        ///                             }
        ///                         ]
        ///     }
        ///     
        /// Ejemplo de resultado:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "PuntosModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de tipo puntos con los datos del cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Consulta")]
        public async Task<List<PuntosModelXML>> ConsultarPuntos([FromBody]JObject jsonData)
        {

            string sCedula = jsonData["Cedula"].Value<string>();

            List<PuntosModelXML> puntosModelXMLs = new List<PuntosModelXML>();
            var puntosAzure = await puntosBL.ConsultarPuntosUsuario(sCedula);

            foreach (var punto in puntosAzure ?? new List<PuntosEntity>())
            {
                puntosModelXMLs.Add(new PuntosModelXML(punto));
            }

            return puntosModelXMLs;
        }

        /// <summary>
        /// Permite obtener la lista de puntos de todos los clientes en Azure.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "PuntosModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de tipo puntos con los datos del cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultaGlobal")]
        public async Task<List<PuntosEntity>> ConsultarPuntosGlobalAzure()
        {
            return await puntosBL.ConsultarPuntosGlobal();
        }

        /// <summary>
        /// Permite obtener la lista de puntos de un cliente en Azure.
        /// </summary>
        /// <param name="Cedula">Identificación del cliente</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "PuntosModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de tipo puntos con los datos del cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultaCedula")]
        public async Task<List<PuntosEntity>> ConsultarPuntosCedulaAzure(string Cedula)
        {
            return await puntosBL.ConsultarPuntosUsuario(Cedula);
        }
        #endregion

        #region Agregar/Actualizar Puntos
        /// <summary>
        /// Permite registrar/actualizar un punto. 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="Puntos">Modelo de puntos</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "PuntosEntity": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Cedula": "1212121212"
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> RegistrarPuntos([FromBody] PuntosEntity Puntos)
        {
            try
            {
                return await puntosBL.GuardarPuntos(Puntos);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar la sucursal, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar registros
        /// <summary>
        /// Permite eliminar un punto.
        /// </summary>
        /// <param name="pPuntoEntity">Modelo de punto.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar - Actualizar
        ///     {
        ///        "PuntosEntity": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Cedula": "1212121212"
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPut]
        [Route("Elimina")]
        public async Task<bool> EliminarPunto([FromBody] PuntosEntity pPuntoEntity)
        {
            try
            {
                return await this.puntosBL.EliminarPunto(pPuntoEntity);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}