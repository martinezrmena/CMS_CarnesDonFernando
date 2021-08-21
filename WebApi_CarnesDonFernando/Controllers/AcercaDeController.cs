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
    /// Permite obtener la información que pertenece a Acerca de Momentos Don Fernando
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AcercaDeController : ControllerBase
    {
        private readonly AcercaDeBL _acercaBL = new AcercaDeBL();

        #region Consultas de Acerca De
        /// <summary>
        /// Permite obtener los datos que pertenecen a la información del panel "Acerca de".
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "AcercaDeEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Descripcion": "ACERCA DE:Don Fernando es una empresa familiar 100% costarricense dedicada...",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Retorna la información del acerca de Momentos Don Fernando.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<AcercaDeEntity> Consulta()
        {
            try
            {
                return await _acercaBL.ConsultarAcercaDe();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar el Acerca De, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Consutar Acerca De Llaves
        /// <summary>
        /// Permite obtener el acerca De especifico por las llaves de particion y fila.
        /// </summary>
        /// <param name="pLlaveParticion">identificación del modelo</param>
        /// <param name="pLlaveFila">idenificación del modelo</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "AcercaDeEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Descripcion": "ACERCA DE:Don Fernando es una empresa familiar 100% costarricense dedicada...",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Retorna la información del acerca de Momentos Don Fernando.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPorLlaves")]
        public async Task<AcercaDeEntity> ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _acercaBL.ConsultarAcercaDeLlaves(pLlaveParticion, pLlaveFila);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los productos del mes, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Agregar/Actualizar Acerca De
        /// <summary>
        /// Metodo que permita agregar un Acerca De, no debería de utilizarse más de una vez según 
        /// la lógica actual para registrar (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pAcercaDe">Modelo de acerca de</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "pAcercaDe": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Descripcion": "ACERCA DE:Don Fernando es una empresa familiar 100% costarricense dedicada...",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> RegistrarAcercaDe([FromBody] AcercaDeEntity pAcercaDe)
        {
            try
            {
                return await _acercaBL.GuardarAcercaDe(pAcercaDe);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar el acerca de, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar Acerca De
        /// <summary>
        /// Metodo que permita eliminar el Acerca De.
        /// </summary>
        /// <param name="pAcercaDeEntity"></param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "pAcercaDe": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Descripcion": "ACERCA DE:Don Fernando es una empresa familiar 100% costarricense dedicada...",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Estado del proceso al finalizar.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPut]
        [Route("Elimina")]
        public async Task<bool> EliminarAcercaDe([FromBody] AcercaDeEntity pAcercaDeEntity)
        {
            try
            {
                return await _acercaBL.EliminarAcercaDe(pAcercaDeEntity);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar el Acerca De, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}