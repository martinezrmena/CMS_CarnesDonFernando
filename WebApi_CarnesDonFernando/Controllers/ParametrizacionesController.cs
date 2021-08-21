using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Clase encargada de las parametizaciones que permiten envios por correos
    /// </summary>
    [Produces("application/json")]
    [Route("api/Parametrizacion")]
    [ApiController]
    public class ParametrizacionesController : ControllerBase
    {
        private readonly ParametrizacionesBL _parametrizacionBL = new ParametrizacionesBL();

        #region Consultas de Parametrizaciones
        /// <summary>
        /// Permite obtener información de las parametizaciones de envio de correos.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ParametrizacionesEntity": [
        ///                             {
        ///                                 "User": "rmartinez@cgclatam.com",
        ///                                 "Pass": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Client": "ara.conexion.cr",
        ///                                 "Port": "587",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Modelo de parametrizaciones.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<ParametrizacionesEntity> Consulta()
        {
            try
            {
                return await _parametrizacionBL.ConsultarParametrizaciones();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las parametrizaciones, favor comunicarse con el administrador.", ex);
            }
        }

        #endregion

        #region Agregar/Actualizar Parametrizaciones
        /// <summary>
        /// Permite obtener información de las parametizaciones de envio de correos 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "ParametrizacionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "acsjcdsijcnjcnsdkjcsdnckjs", //clave unica
        ///                                 "RowKey": "acsjcdsijcnjcnsdkjcsdnckjs", //clave unica
        ///                                 "User": "rmartinez@cgclatam.com",
        ///                                 "Pass": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Client": "ara.conexion.cr",
        ///                                 "Port": "587",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Estado de la petición</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> RegistrarParametrizacion([FromBody] ParametrizacionesEntity pParametrizacion)
        {
            try
            {
                return await _parametrizacionBL.GuardarParametrizacion(pParametrizacion);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar las parametrizaciones, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

    }
}