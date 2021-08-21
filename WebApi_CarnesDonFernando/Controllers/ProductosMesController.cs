using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar los productos del mes
    /// </summary>
    [Produces("application/json")]
    [Route("api/ProductosMes")]
    [ApiController]
    public class ProductosMesController : Controller
    {
        private readonly ProductosMesBL _productosMesBL = new ProductosMesBL();

        #region Consutar Productos del mes
        /// <summary>
        /// Permite obtener la lista de productos del mes.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ProductosMesEntity": [
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
        /// <returns>lista de productos del mes.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<ProductosMesEntity>> Consulta()
        {
            try
            {
                return await _productosMesBL.ConsultarTodosProductos();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los productos del mes, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Consutar un Producto del mes
        /// <summary>
        /// Permite obtener el producto del mes según sus llaves unicas.
        /// </summary>
        /// <param name="pLlaveParticion">Llave unica</param>
        /// <param name="pLlaveFila">LLave unica</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ProductosMesEntity": [
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
        /// <returns>Producto del mes.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPorLlaves")]
        public async Task<ProductosMesEntity> ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _productosMesBL.ConsultarProducto(pLlaveParticion, pLlaveFila);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los productos del mes, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener un producto.
        /// </summary>
        /// <param name="pTitulo">Titulo del producto</param>
        /// <param name="pDetalle">Detalle del producto</param>
        /// <param name="pResumen">Resumen del producto</param>
        /// <param name="pPreparacion">Formas de preparación del producto</param>
        /// <param name="pFechaIni">Fecha de inicio de circulación del producto</param>
        /// <param name="pFechaFin">Fecha de finalización de circulación del producto</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ProductosMesEntity": [
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
        /// <returns>Producto del mes.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Existe")]
        public async Task<ProductosMesEntity> ExisteProducto(string pTitulo, string pDetalle, string pResumen, string pPreparacion, string pFechaIni, string pFechaFin)
        {
            try
            {  
                return await _productosMesBL.ConsultarSiExiste(pTitulo, pDetalle, pResumen, pPreparacion, pFechaIni, pFechaFin);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los productos, favor comunicarse con el administrador.", ex);
            }
        }

        #endregion

        #region Agregar/Actualizar Productos del mes
        /// <summary>
        /// Permite ingresar - actualizar un producto 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pProductos">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "ProductosMesEntity": [
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
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Registra")]
        public async Task<bool> Registra([FromBody] ProductosMesEntity pProductos)
        {
            try
            {
                return await _productosMesBL.GuardarProductos((pProductos));
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar el producto, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar producto del mes
        /// <summary>
        /// Permite eliminar un producto.
        /// </summary>
        /// <param name="pProducto">Modelo</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "ProductosMesEntity": [
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
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPut]
        [Route("Elimina")]
        public async Task<bool> Elimina([FromBody] ProductosMesEntity pProducto)
        {
            try
            {
                return await _productosMesBL.EliminarProducto(pProducto);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}

