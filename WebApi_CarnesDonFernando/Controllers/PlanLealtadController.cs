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
    /// Permite administrar el plan de lealtad
    /// </summary>
    [Produces("application/json")]
    [Route("api/PlanLealtad")]
    [ApiController]
    public class PlanLealtadController : ControllerBase
    {
        private readonly PlanLealtadBL _planLealtadBL = new PlanLealtadBL();

        #region Consultas de Plan de lealtad
        /// <summary>
        /// Permite obtener los datos que pertenecen a la información del plan de lealtad.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PlanLealtadEntity": [
        ///                                 {
        ///                                     "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "Foto": "https://appmomentosdf.blob.core.windows.net/planimages/platinum.png",",
        ///                                     "TipoCliente": "Plantino",
        ///                                     "Descripcion": "Es un plan que se obtiene al lograr...",
        ///                                     "Beneficios": "Recibirá un porcentaje de cash back del 4% del total de su compra...",
        ///                                     "BonoNivel": "4"
        ///                                 }
        ///                             ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Lista de planes de lealtad.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<PlanLealtadEntity>> Consulta()
        {
            try
            {
                return await _planLealtadBL.ConsultarTodosPlanesLealtad();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los planes de lealtad, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener un plan de lealtad por sus llaves unicas.
        /// </summary>
        /// <param name="pLlaveParticion">Llave unica</param>
        /// <param name="pLlaveFila">LLave unica</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PlanLealtadEntity": [
        ///                                 {
        ///                                     "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "Foto": "https://appmomentosdf.blob.core.windows.net/planimages/platinum.png",",
        ///                                     "TipoCliente": "Plantino",
        ///                                     "Descripcion": "Es un plan que se obtiene al lograr...",
        ///                                     "Beneficios": "Recibirá un porcentaje de cash back del 4% del total de su compra...",
        ///                                     "BonoNivel": "4"
        ///                                 }
        ///                             ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Plan de lealtad.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPorLlaves")]
        public async Task<PlanLealtadEntity> ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _planLealtadBL.ConsultarPlanLealtad(pLlaveParticion, pLlaveFila);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar el Plan de Lealtad, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Agregar/Actualizar Plan de Lealtad
        /// <summary>
        /// Permite actualizar o insertar un plan de lealtad 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "PlanLealtadEntity": [
        ///                                 {
        ///                                     "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "Foto": "https://appmomentosdf.blob.core.windows.net/planimages/platinum.png",",
        ///                                     "TipoCliente": "Plantino",
        ///                                     "Descripcion": "Es un plan que se obtiene al lograr...",
        ///                                     "Beneficios": "Recibirá un porcentaje de cash back del 4% del total de su compra...",
        ///                                     "BonoNivel": "4"
        ///                                 }
        ///                             ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> RegistrarPlanLealtad([FromBody] PlanLealtadEntity pPlanLealtad)
        {
            try
            {
                return await _planLealtadBL.GuardarPlanLealtad(pPlanLealtad);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar el plan de lealtad, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar registros
        /// <summary>
        /// Permite eliminar el plan de lealtad.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "PlanLealtadEntity": [
        ///                                 {
        ///                                     "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                     "Foto": "https://appmomentosdf.blob.core.windows.net/planimages/platinum.png",",
        ///                                     "TipoCliente": "Plantino",
        ///                                     "Descripcion": "Es un plan que se obtiene al lograr...",
        ///                                     "Beneficios": "Recibirá un porcentaje de cash back del 4% del total de su compra...",
        ///                                     "BonoNivel": "4"
        ///                                 }
        ///                             ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPut]
        [Route("Elimina")]
        public async Task<bool> Elimina([FromBody] PlanLealtadEntity pPlanEntity)
        {
            try
            {
                return await _planLealtadBL.EliminarPlanLealtad(pPlanEntity);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}