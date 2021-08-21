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
    /// Permite administrar la información de la politica de privacidad.
    /// </summary>
    [Produces("application/json")]
    [Route("api/PoliticaPrivacidad")]
    [ApiController]
    public class PoliticaPrivacidadController : ControllerBase
    {
        private readonly PoliticaPrivacidadBL _politicaPrivacidadBL = new PoliticaPrivacidadBL();

        #region Consultas de Politica Privacidad
        /// <summary>
        /// Permite obtener la politica de privacidad.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PoliticaPrivacidadEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Descripcion": "ACERCA DE:Don Fernando es una empresa familiar 100% costarricense dedicada...",
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de politica de privacidad.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<PoliticaPrivacidadEntity> Consulta()
        {
            try
            {
                return await _politicaPrivacidadBL.ConsultarPoliticaPrivacidad();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar la politica de privacidad, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}