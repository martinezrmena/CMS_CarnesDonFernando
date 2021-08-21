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
    /// Clase encargada de la administración de los cantones.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Cantones")]
    [ApiController]
    public class CantonesController : ControllerBase
    {
        /// <summary>
        /// BL de la clase cantones
        /// </summary>
        private readonly CantonesBL _cantonBL = new CantonesBL();

        #region Consultas cantones BD CIISA
        /// <summary>
        /// Obtiene la lista de cantones desde los servidores de CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "CantonModel": [
        ///                             {
        ///                                 "Pais": "CR",
        ///                                 "Provincia": "5",
        ///                                 "Canton": "62",
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
        [Route("Consulta")]
        public async Task<List<CantonModel>> ConsultarCantones()
        {

            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0,2,30),
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
                    var a = await client.consultaCantonesAppAsync();

                    var result = a.Any1;

                    var datos = result.Descendants("CANTONES").Select(d =>
                        new CantonModel
                        {
                            Pais = d.Element("PAIS").Value,
                            Provincia = d.Element("PROVINCIA").Value,
                            Canton = d.Element("CANTON").Value,
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

        /// <summary>
        /// Obtiene la lista de cantones dependiendo de la provincia establecida 
        /// guardados en los servicios de CIISA.
        /// </summary>
        /// <param name="pProvincia">Código de la provincia de la cual son dependientes los cantones que se requieren mostrar.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "CantonModel": [
        ///                             {
        ///                                 "Pais": "CR",
        ///                                 "Provincia": "5",
        ///                                 "Canton": "62",
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
        [Route("ConsultaCanton")]
        public async Task <IEnumerable<CantonModel>> ConsultarCanton(string pProvincia)
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
                    var a = await client.consultaCantonesAppAsync();

                    var result = a.Any1;

                    var datos = result.Descendants("tabla").Select(d =>
                        new CantonModel
                        {
                            Pais = d.Element("PAIS").Value,
                            Provincia = d.Element("PROVINCIA").Value,
                            Canton = d.Element("CANTON").Value,
                            Descripcion = d.Element("DESCRIPCION").Value
                        }
                    ).ToList().Where(x => x.Provincia == pProvincia);

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
        /// Obtiene la lista de cantones desde Azure Tables.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "CantonesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Pais": "CR",
        ///                                 "Provincia": "5",
        ///                                 "Canton": "62",
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
        [Route("ConsultarCantones")]
        public async Task<List<CantonesEntity>> ConsultaCantones()
        {
            try
            {
                return await  _cantonBL.ConsultarCantones();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los Cantones, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de cantones dependiendo de la provincia establecida guardados en Azure Tables.
        /// </summary>
        /// <param name="pProvincia">Código de la provincia de la cual son dependientes los cantones que se requieren mostrar.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "CantonesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXX",
        ///                                 "Pais": "CR",
        ///                                 "Provincia": "5",
        ///                                 "Canton": "62",
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
        [Route("ConsultarCantonPorProvincia")]
        public async Task<List<CantonesEntity>> ConsultarCantonPorProvincia(string pProvincia)
        {
            try
            {
                return await _cantonBL.ConsultarCantonPorProvincia(pProvincia);
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los Cantones, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion
    }
}