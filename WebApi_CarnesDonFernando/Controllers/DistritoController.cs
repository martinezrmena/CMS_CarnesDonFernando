using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using MomentosDonFernando;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Clase encargada de la administración de los distritos.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Distritos")]
    [ApiController]
    public class DistritoController : Controller
    {
        /// <summary>
        /// BL de la clase distritos
        /// </summary>
        private readonly DistritoBL _distritoBL = new DistritoBL();

        #region Consultas distritos BD CIISA
        /// <summary>
        /// Obtiene la lista de distritos desde los servidores de CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Distrito
        ///     {
        ///        "CantonModel": [
        ///                             {
        ///                                 "Canton": "5",
        ///                                 "Distrito": "62",
        ///                                 "Descripcion": "La Cruz"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de distritos.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<DistritoModel>> ConsultarDistritos()
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
                    var a = await client.consultaDistritosAppAsync();

                    var result = a.Any1;

                    var datos = result.Descendants("DISTRITOS").Select(d =>
                        new DistritoModel
                        {
                            Canton = d.Element("CANTON").Value,
                            Distrito = d.Element("DISTRITO").Value,
                            Descripcion = d.Element("DESCRIPCION").Value
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

        #region Consultas Cantones en Azure Tables
        /// <summary>
        /// Obtiene la lista de distritos desde Azure Tables.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "DistritosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Canton": "5",
        ///                                 "Distrito": "62",
        ///                                 "Descripcion": "La Cruz"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de distritos.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarDistritos")]
        public async Task<List<DistritoEntity>> ConsultaDistritos()
        {
            try
            {
                return await _distritoBL.ConsultarDistritos();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los Cantones, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de distritos dependiendo del canton establecida guardados en Azure Tables.
        /// </summary>
        /// <param name="pCanton">Código del cantón de la cual son dependientes los distritos que se requieren mostrar.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "DistritosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Canton": "5",
        ///                                 "Distrito": "62",
        ///                                 "Descripcion": "La Cruz"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de cantones.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarDistritosPorCanton")]
        public async Task<List<DistritoEntity>> ConsultarDistritosPorCanton(string pCanton)
        {
            try
            {
                return await _distritoBL.ConsultarCantonPorProvincia(pCanton);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los Cantones, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Registro Azure
        /// <summary>
        /// Permite insertar un registro
        /// </summary>
        /// <param name="pDistrito">Modelo de tipo distrito.</param>
        /// <returns>Lista de cantones.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("InsertarDistrito")]
        public async Task<bool> InsertarDistrito(DistritoEntity pDistrito)
        {
            try
            {
                return await _distritoBL.InsertarDistrito(pDistrito);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al insertar el distrito, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}
