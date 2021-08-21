using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarnesDonFernando.EL;
using CarnesDonFernando.BL;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar las promociones
    /// </summary>
    [Produces("application/json")]
    [Route("api/Promociones")]
    [ApiController]
    public class PromocionesController : ControllerBase
    {
        private readonly PromocionesBL _promocionBL = new PromocionesBL();

        #region Consultas de Promociones
        /// <summary>
        /// Permite obtener la lista de promociones.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PromocionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Fecha_Publicacion": "30/12/2020",
        ///                                 "Fecha_Finalizacion": "30/12/2020",
        ///                                 "ImagenUrl": "https://appmomentosdf.blob.core.windows.net/promotionimages/PREVENTANAVIDAD202010.jpg",
        ///                                 "Enlace": "https://ejemplopagina.com"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de promociones.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<PromocionesEntity>> Consulta()
        {
            try
            {
                return await _promocionBL.ConsultarTodasPromociones();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Promociones, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener la promoción según sus llaves unicas.
        /// </summary>
        /// <param name="pLlaveParticion">Llave unica</param>
        /// <param name="pLlaveFila">LLave unica</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PromocionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Fecha_Publicacion": "30/12/2020",
        ///                                 "Fecha_Finalizacion": "30/12/2020",
        ///                                 "ImagenUrl": "https://appmomentosdf.blob.core.windows.net/promotionimages/PREVENTANAVIDAD202010.jpg",
        ///                                 "Descripcion_Resumen": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Descripcion_Detalle": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Preparacion": "Coccion, Frio, Horno",
        ///                                 "UrlIconosPreparacion": "https://appmomentosdf.blob.core.windows.net/preparacion/Horno.png,https://appmomentosdf.blob.core.windows.net/preparacion/Frio.png,https://appmomentosdf.blob.core.windows.net/preparacion/CoccionLiquida.png"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Promoción.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPorLlaves")]
        public async Task<PromocionesEntity> ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _promocionBL.ConsultarPromocion(pLlaveParticion, pLlaveFila);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Promociones, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite validar si existe una promoción con los datos exactamente iguales.
        /// </summary>
        /// <param name="pTitulo">Título de la promoción.</param>
        /// <param name="pEnlace">Enlace de visita de la promoción.</param>
        /// <param name="pFechaIni">Fecha de inicio de la promoción.</param>
        /// <param name="pFechaFin">Fecha de finalización de la promoción.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "PromocionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Fecha_Publicacion": "30/12/2020",
        ///                                 "Fecha_Finalizacion": "30/12/2020",
        ///                                 "ImagenUrl": "https://appmomentosdf.blob.core.windows.net/promotionimages/PREVENTANAVIDAD202010.jpg",
        ///                                 "Descripcion_Resumen": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Descripcion_Detalle": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Preparacion": "Coccion, Frio, Horno",
        ///                                 "UrlIconosPreparacion": "https://appmomentosdf.blob.core.windows.net/preparacion/Horno.png,https://appmomentosdf.blob.core.windows.net/preparacion/Frio.png,https://appmomentosdf.blob.core.windows.net/preparacion/CoccionLiquida.png"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Promoción.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Existe")]
        public async Task<PromocionesEntity> ExistePromocion(string pTitulo, string pEnlace, string pFechaIni, string pFechaFin)
        {
            try
            {
                return await _promocionBL.ConsultarSiExiste(pTitulo,pEnlace, pFechaIni, pFechaFin);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Promociones, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Agregar/Actualizar Promociones
        /// <summary>
        /// Permite agregar - actualizar un registro de promociones 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pPromocion">Modelo de promoción.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "PromocionesEntity": [
        ///                              {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Fecha_Publicacion": "30/12/2020",
        ///                                 "Fecha_Finalizacion": "30/12/2020",
        ///                                 "ImagenUrl": "https://appmomentosdf.blob.core.windows.net/promotionimages/PREVENTANAVIDAD202010.jpg",
        ///                                 "Descripcion_Resumen": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Descripcion_Detalle": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Preparacion": "Coccion, Frio, Horno",
        ///                                 "UrlIconosPreparacion": "https://appmomentosdf.blob.core.windows.net/preparacion/Horno.png,https://appmomentosdf.blob.core.windows.net/preparacion/Frio.png,https://appmomentosdf.blob.core.windows.net/preparacion/CoccionLiquida.png"
        ///                              }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> RegistrarPromocion([FromBody] PromocionesEntity pPromocion)
        {
            try
            {
                return await _promocionBL.GuardarPromociones(pPromocion);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar registros
        /// <summary>
        /// Permite eliminar una promoción.
        /// </summary>
        /// <param name="pPromocionEntity">Modelo de promoción.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "PromocionesEntity": [
        ///                              {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Titulo": "Acerca de Momentos Don Fernando",
        ///                                 "Fecha_Publicacion": "30/12/2020",
        ///                                 "Fecha_Finalizacion": "30/12/2020",
        ///                                 "ImagenUrl": "https://appmomentosdf.blob.core.windows.net/promotionimages/PREVENTANAVIDAD202010.jpg",
        ///                                 "Descripcion_Resumen": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Descripcion_Detalle": "Pierna de Cerdo de 2,5kg o 4 Kg...",
        ///                                 "Preparacion": "Coccion, Frio, Horno",
        ///                                 "UrlIconosPreparacion": "https://appmomentosdf.blob.core.windows.net/preparacion/Horno.png,https://appmomentosdf.blob.core.windows.net/preparacion/Frio.png,https://appmomentosdf.blob.core.windows.net/preparacion/CoccionLiquida.png"
        ///                              }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPut]
        [Route("Elimina")]
        public async Task<bool> EliminarPromocion([FromBody] PromocionesEntity pPromocionEntity)
        {
            try
            {
                return await _promocionBL.EliminarPromociones(pPromocionEntity);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}