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
    /// Permite administrar las provincias.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Provincias")]
    [ApiController]
    public class ProvinciasController : ControllerBase
    {
        private readonly ProvinciasBL _provinciaBL = new ProvinciasBL();

        #region Consultas provincias BD Carnes Don Fernando
        /// <summary>
        /// Permite obtener las provincias desde los servicios de CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ProvinciaModel": [
        ///                             {
        ///                                 "País": "CR",
        ///                                 "Provincia": "1",
        ///                                 "Descripción": "San José",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Lista de provincias.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<ProvinciaModel>> ConsultarProvincias()
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
                    var a = await client.consultaProvinciasAppAsync();

                    var result = a.Any1;


                    List<ProvinciaModel> datos = result.Descendants("PROVINCIAS").Select(d =>
                                                new ProvinciaModel
                                                {
                                                    Pais = d.Element("PAIS").Value,
                                                    Provincia = d.Element("PROVINCIA").Value,
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

        #region Consultas Provincias en Azure Tables
        /// <summary>
        /// Permite obtener las provincias desde Azure Tables.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consulta
        ///     {
        ///        "ProvinciasEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "Pais": "CR",
        ///                                 "Provincia": "1",
        ///                                 "Descripcion": "San José",
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Lista de provincias.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarProvincias")]
        public async Task<List<ProvinciasEntity>> ConsultaProvincias()
        {
            try
            {
                return await _provinciaBL.ConsultarProvincias();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar las Provincias, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

    }
}