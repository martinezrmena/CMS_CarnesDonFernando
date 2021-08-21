using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MomentosDonFernando;
using WebApi_CarnesDonFernando.Helpers;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar los metodos utilizados desde la Web.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class WebController : ControllerBase
    {
        private readonly UsuariosBL _usuariosBL = new UsuariosBL();
        private readonly PuntosBL puntosBL = new PuntosBL();
        private readonly string BaseUrl;

        public WebController()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        /// <summary>
        /// Permite autenticar un usuario con la contraseña sin encriptar en Azure.
        /// </summary>
        /// <param name="UserData">Datos para autenticar usuario con la contraseña sin encriptar.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Consultar
        ///     {
        ///        {
        ///             "Email": "ejemplo@hotmail.com",
        ///             "Password": "contra"
        ///         }
        ///     }
        ///
        /// Ejemplo de resultado:
        ///
        ///     POST /Consultar
        ///     {
        ///        "UsuariosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //llave unica
        ///                                 "Cedula": "123456789",
        ///                                 "Password": "ASADFV12!"$##==", //contraseña encriptada
        ///                                 "Tipo_Cedula": "Fisica",
        ///                                 "Nombre": "Rafael",
        ///                                 "Apellido": "Martínez",
        ///                                 "Email": "ejemplo@gmail.com",
        ///                                 "Fecha_Nacimiento": "10/06/2019",
        ///                                 "Genero": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "Alajuela",
        ///                                 "CodigoProvincia": "2",
        ///                                 "Canton": "Alajuela",
        ///                                 "CodigoCanton": "20",
        ///                                 "Direccion_Exacta": "Cartago Urb El Silo Casa 5f",
        ///                                 "Sucursal1": "Restaurante Heredia",
        ///                                 "CodigoSucursal1": "49",
        ///                                 "Sucursal2": "Restaurante Heredia",
        ///                                 "Codigo_Invitacion": "ababkjsackjsa",
        ///                                 "PictureURL": "https://appmomentosdf.blob.core.windows.net/userimages/imagen.jpg",
        ///                                 "PictureName": "imagen.jpg",
        ///                                 "Puntos": "8",
        ///                                 "Sincronizado": "false"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Usuario.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="204">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarUsuario")]
        public async Task<UsuariosEntity> ConsultarUsuario([FromBody] DataUserModel UserData)
        {
            try
            {
                ValidationString validationString = new ValidationString();
                string pEmail = UserData?.Email;
                string pContrasenna = UserData?.Password;

                pContrasenna = validationString.Encriptar(pContrasenna);

                var resultado = await _usuariosBL.ConsultarUsuario(pEmail, pContrasenna);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener la lista de puntos en relación a un cliente.
        /// </summary>
        /// <param name="UserModel">Modelo que permite enviar la cédula del cliente.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        {
        ///             "Cedula": "12345"
        ///         }
        ///     }
        ///     
        /// Ejemplo de resultado:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "PuntosModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de tipo puntos con los datos del cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarPuntos")]
        public async Task<List<PuntosModelXML>> ConsultarPuntos([FromBody] DataUserModel UserModel)
        {

            List<PuntosModelXML> puntosModelXMLs = new List<PuntosModelXML>();
            var puntosAzure = await puntosBL.ConsultarPuntosUsuario(UserModel?.Cedula);

            foreach (var punto in puntosAzure ?? new List<PuntosEntity>())
            {
                puntosModelXMLs.Add(new PuntosModelXML(punto));
            }

            return puntosModelXMLs;
        }

        /// <summary>
        /// Permite registrar/actualizar un usuario en Azure sin contraseñas encriptadas
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pUsuario">Modelo de usuario a insertar o actualizar.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Insertar-Actualizar
        ///        {
        ///             "PartitionKey": "jaNAJnaJKNAKJnajkNAJ",
        ///             "RowKey": "jnsdjncjksdncjkdscnjksdcnsjknc",
        ///             "Timestamp": "2021-01-13T18:31:32.118Z",
        ///             "ETag": "",
        ///             "Cedula": "222222223",
        ///             "Password": "abc123#.",
        ///             "Tipo_Cedula": "Fisica",
        ///             "Nombre": "Ignacio",
        ///             "Apellido": "Mena",
        ///             "Email": "ejemplo@gmail.com",
        ///             "Fecha_Nacimiento": "11/11/1995",
        ///             "Genero": "Hombre",
        ///             "Telefono": "22222222",
        ///             "Provincia": "Heredia",
        ///             "CodigoProvincia": "1",
        ///             "Canton": "Heredia",
        ///            "CodigoCanton": "2",
        ///            "Direccion_Exacta": "av rio",
        ///             "Sucursal1": "Heredia",
        ///             "CodigoSucursal1": "1",
        ///             "Sucursal2": "Heredia",
        ///             "Codigo_Invitacion": "",
        ///             "PictureURL": "",
        ///             "PictureName": "",
        ///             "Puntos": "0",
        ///             "Sincronizado": true
        ///        }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="304">El usuario ya existe pero no proporciono las llaves unicas del mismo.</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("RegistraUsuario")]
        public async Task<bool> RegistrarUsuarioAzure([FromBody] UsuariosEntity pUsuario)
        {
            try
            {
                var usuarioCedula = await _usuariosBL.ConsultarUsuarioXCedula(pUsuario.Cedula);
                var usuarioCorreo = await _usuariosBL.ConsultarUsuarioXCorreo(pUsuario.Email);

                if (usuarioCedula != null && (usuarioCedula.PartitionKey != pUsuario.PartitionKey || usuarioCedula.RowKey != pUsuario.RowKey))
                {
                    //Si el usuario existe pero no posee las mismas llaves entonces es un error.

                    HttpContext.Response.StatusCode = HttpStatusCode.NotModified.GetHashCode();
                    return false;
                }

                if (usuarioCorreo != null && (usuarioCedula.PartitionKey != pUsuario.PartitionKey || usuarioCedula.RowKey != pUsuario.RowKey))
                {
                    //Si el usuario existe pero no posee las mismas llaves entonces es un error.
                    HttpContext.Response.StatusCode = HttpStatusCode.NotModified.GetHashCode();
                    return false;
                }

                ValidationString validationString = new ValidationString();
                pUsuario.Password = validationString.Encriptar(pUsuario.Password);

                var result = await _usuariosBL.GuardarUsuarios(pUsuario);

                if (result)
                {
                    return true;
                }
                else
                {
                    HttpContext.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar el usuario, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener la lista de puntos de todos los clientes en CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Permite consultar los puntos
        ///     {
        ///        "PuntosModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1", //identificador interno
        ///                                 "Saldo": "7",
        ///                                 "Tipo_Mov": "Compra",
        ///                                 "Descripcion": "Se han comprado 7...",
        ///                                 "Fecha_Mov": "7/8/2019",
        ///                                 "Centro": "1"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de tipo puntos con los datos del cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultarPuntosGlobal")]
        public async Task<List<PuntosModelXML>> ConsultarPuntosGlobal()
        {
            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0, 3, 0),
                OpenTimeout = new TimeSpan(0, 3, 0),
                SendTimeout = new TimeSpan(0, 3, 0)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointConsutaApp);
            using (var myChannelFactory = new ChannelFactory<ConsultaAppSoap>(myBinding, myEndpoint))
            {
                ConsultaAppSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.consultaPuntosAppAsync();

                    var result = a.Any1;

                    var datos = result.Descendants("PUNTOS").Select(d =>
                        new PuntosModelXML
                        {
                            Id_Cliente = (d.Element("ID_CLIENTE").ElementValueNull()),
                            Saldo = (d.Element("SALDO").ElementValueNull()),
                            Tipo_Mov = d.Element("TIPO_MOV").ElementValueNull(),
                            Descripcion = d.Element("DESCRIPCION").ElementValueNull(),
                            Fecha_Mov = (d.Element("FECHA_MOV").ElementValueNull()),
                            Centro = d.Element("CENTRO").ElementValueNull()
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
    }
}
