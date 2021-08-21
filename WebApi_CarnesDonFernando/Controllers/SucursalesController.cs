using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MomentosDonFernando;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar las sucursales.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Sucursales")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly SucursalesBL _sucursalBL = new SucursalesBL();

        #region Consultas Sucursales BD Carnes Don Fernando
        /// <summary>
        /// Permite consultar las sucursales en los servidores de CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "SucursalEntity": [
        ///                             {
        ///                                 "Centro": "1",
        ///                                 "Descripcion": "Heredia",
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de sucursales.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<SucursalModel>> ConsultarSucursales()
        {

            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0, 2, 30),
                SendTimeout = new TimeSpan(0, 2, 30),
                OpenTimeout = new TimeSpan(0, 2, 30)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointConsutaApp);
            using (var myChannelFactory = new ChannelFactory<ConsultaAppSoap>(myBinding, myEndpoint))
            {
                ConsultaAppSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.consultaSucursalesAppAsync();

                    var result = a.Any1;


                    List<SucursalModel> datos = result.Descendants("SUCURSALES").Select(d =>
                                                new SucursalModel
                                                {
                                                    Centro = d.Element("CENTRO").Value,
                                                    Descripcion_Centro = d.Element("DESCRIPCION_CENTRO").Value
                                                }
                    ).ToList();

                    ((ICommunicationObject)client).Close();
                    myChannelFactory.Close();


                    return datos;
                }
                catch (Exception ex)
                {
                    (client as ICommunicationObject)?.Abort();
                    return null;
                }
            }
        }
        #endregion

        #region Consultas sucursales azure tables
        /// <summary>
        /// Permite consultar las sucursales en las tablas de Azure.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "SucursalEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "Centro": "1",
        ///                                 "Descripcion_Centro": "Heredia",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Lista de sucursales.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarSucursales")]
        public async Task<List<SucursalEntity>> ConsultaSucursales()
        {
            try
            {
                return await _sucursalBL.ConsultarSucursales();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Sucursales, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite consultar los datos de las sucursales.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "SucursalesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "FotoUrl": "https://foto1.com",
        ///                                 "Nombre": "Sucursal Santa Ana",
        ///                                 "EnlaceGoogleMaps": "https://www.google.com.sv/maps",
        ///                                 "IconoGoogleMaps": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/maps.png",
        ///                                 "EnlaceWaze": "https://www.waze.com/es-419",
        ///                                 "IconoWaze": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/waze.jpg",
        ///                                 "TelefonoTienda": "22214318",
        ///                                 "TelefonoRestaurante": "22214318",
        ///                                 "HorarioTienda": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "HorarioRestaurante": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "Telefonos": "22214318,22214318"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Lista de datos de las sucursales.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarDatosSucursal")]
        public async Task<List<SucursalesEntity>> ConsultaDatosSucursal()
        {
            try
            {
                return await _sucursalBL.ConsultarDatosSucursales();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Sucursales, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite consultar los datos de la sucursal por llaves unicas.
        /// </summary>
        /// <param name="pLlaveParticion">Llave unica</param>
        /// <param name="pLlaveFila">Llave unica</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "SucursalesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "FotoUrl": "https://foto1.com",
        ///                                 "Nombre": "Sucursal Santa Ana",
        ///                                 "EnlaceGoogleMaps": "https://www.google.com.sv/maps",
        ///                                 "IconoGoogleMaps": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/maps.png",
        ///                                 "EnlaceWaze": "https://www.waze.com/es-419",
        ///                                 "IconoWaze": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/waze.jpg",
        ///                                 "TelefonoTienda": "22214318",
        ///                                 "TelefonoRestaurante": "22214318",
        ///                                 "HorarioTienda": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "HorarioRestaurante": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "Telefonos": "22214318,22214318"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo con datos de la sucursal.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPorLlaves")]
        public async Task<SucursalesEntity> ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _sucursalBL.ConsultarDatoSucursal(pLlaveParticion, pLlaveFila);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Promociones, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Agregar/Actualizar Sucursales
        /// <summary>
        /// Permite registrar/actualizar una sucursal 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pSucursal">Modelo de sucursal</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar - Actualizar
        ///     {
        ///        "SucursalesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "FotoUrl": "https://foto1.com",
        ///                                 "Nombre": "Sucursal Santa Ana",
        ///                                 "EnlaceGoogleMaps": "https://www.google.com.sv/maps",
        ///                                 "IconoGoogleMaps": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/maps.png",
        ///                                 "EnlaceWaze": "https://www.waze.com/es-419",
        ///                                 "IconoWaze": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/waze.jpg",
        ///                                 "TelefonoTienda": "22214318",
        ///                                 "TelefonoRestaurante": "22214318",
        ///                                 "HorarioTienda": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "HorarioRestaurante": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "Telefonos": "22214318,22214318"
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
        public async Task<bool> RegistrarSucursal([FromBody] SucursalesEntity pSucursal)
        {
            try
            {
                return await _sucursalBL.GuardarSucursales(pSucursal);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar la sucursal, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Registro Azure
        /// <summary>
        /// Permite insertar un registro
        /// </summary>
        /// <param name="pSucursal">Modelo de tipo sucursal.</param>
        /// <returns>Lista de sucursales.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("InsertarSucursal")]
        public async Task<bool> InsertarSucursal(SucursalEntity pSucursal)
        {
            try
            {
                return await _sucursalBL.GuardarSucursalEntity(pSucursal);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al insertar el distrito, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar registros
        /// <summary>
        /// Permite eliminar datos de una sucursal.
        /// </summary>
        /// <param name="pSucursalEntity">Modelo de datos de la sucursal.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "SucursalesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "FotoUrl": "https://foto1.com",
        ///                                 "Nombre": "Sucursal Santa Ana",
        ///                                 "EnlaceGoogleMaps": "https://www.google.com.sv/maps",
        ///                                 "IconoGoogleMaps": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/maps.png",
        ///                                 "EnlaceWaze": "https://www.waze.com/es-419",
        ///                                 "IconoWaze": "https://appmomentosdf.blob.core.windows.net/branchofficeimages/waze.jpg",
        ///                                 "TelefonoTienda": "22214318",
        ///                                 "TelefonoRestaurante": "22214318",
        ///                                 "HorarioTienda": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "HorarioRestaurante": "Lunes a Sábado de: 9:30 am a: 6:00pm,Domingo de: 10:00am a: 4:00 PM",
        ///                                 "Telefonos": "22214318,22214318"
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
        public async Task<bool> EliminarSucursal([FromBody] SucursalesEntity pSucursalEntity)
        {
            try
            {
                return await _sucursalBL.EliminarSucursales(pSucursalEntity);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar la sucursal, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

    }
}