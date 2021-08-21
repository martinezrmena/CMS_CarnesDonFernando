using CarnesDonFernando.BL;
using CarnesDonFernando.EL;
using InsercionApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading.Tasks;
using WebApi_CarnesDonFernando.Helpers;

namespace WebApi_CarnesDonFernando.Controllers
{
    [Produces("application/json")]
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosBL _usuariosBL = new UsuariosBL();
        private readonly string BaseUrl;

        public UsuariosController()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #region Consultas de Usuarios en la tabla de azure
        /// <summary>
        /// Permite consultar todos los usuarios en Azure.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     GET /Consultar
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
        /// <returns>Lista de usuarios.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<UsuariosEntity>> Consulta()
        {
            try
            {
                return await _usuariosBL.ConsultarUsuarios();
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los Usuarios, favor comunicarse con el administrador.", ex);
            }
        }


        /// <summary>
        /// Permite autenticar un usuario por su cedula y contraseña encriptada en Azure.
        /// </summary>
        /// <param name="jsonData">Datos en formato JSON para autenticar usuario con contraseña encriptada.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Consultar
        ///     {
        ///        "jsonData": [
        ///                             {
        ///                                 "Email": "ejemplo@hotmail.com"
        ///                                 "Password": "kcdjcnj#$#===cd13" //contraseña encriptada
        ///                             }
        ///                         ]
        ///     }
        /// Ejemplo de resultado:
        ///
        ///     POST /Consultar
        ///     {
        ///        "UsuariosEntity": [
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
        /// <returns>Usuario.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarUsuario")]
        public async Task<UsuariosEntity> ConsultarUsuario([FromBody]JObject jsonData)
        {
            try
            {
                string pEmail = jsonData["Email"].Value<string>();
                string pContrasenna = jsonData["Password"].Value<string>();

                var resultado = await _usuariosBL.ConsultarUsuario(pEmail, pContrasenna);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite consultar un usuario por la cédula en Azure.
        /// </summary>
        /// <param name="pCedula">Identificación del usuario a obtener</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Consultar
        ///     {
        ///        "UsuariosEntity": [
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
        /// <returns>Usuario.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarUsuarioCedula")]
        public async Task<UsuariosEntity> ConsultarUsuarioPorCedula(string pCedula)
        {
            try
            {
                var resultado = await _usuariosBL.ConsultarUsuarioXCedula(pCedula);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite consultar usuario por correo y contraseña encriptada en Azure.
        /// </summary>
        /// <param name="jsonData">JSON que contiene el correo y contraseña encriptada.</param>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Consultar
        ///     {
        ///        "jsonData": [
        ///                             {
        ///                                 "_email": "123456789"
        ///                                 "_password": "asadcd===221#$" //contraseña encriptada
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Estado de la petición.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarPorCorreo")]
        public async Task<bool> ConsultarUsuarioPorCorreo([FromBody]JObject jsonData)
        {
            try
            {
                bool response = false;

                string pCorreo = jsonData["_email"].Value<string>();
                string pContrasenna = jsonData["_password"].Value<string>();

                var resultado = await _usuariosBL.ConsultarUsuarioXCorreo(pCorreo);

                if (resultado == null)
                {
                    response = false;
                }
                else
                {
                    UsuariosEntity Entity = resultado;

                    Entity.Password = pContrasenna;

                    //Actualiza el usuario con la contrasenna
                    response = await _usuariosBL.GuardarUsuarios(Entity);


                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite obtener usuarios especificos pendientes de actualizar o actualizados 
        /// en Azure desde el servicios del arreo.
        /// </summary>
        /// <param name="pSincronizado">Estado de los usuarios a consultar</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Consultar
        ///     {
        ///        "UsuariosEntity": [
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
        /// <returns>Lista de usuario que coincidan con la validación</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("ConsultarUsuarioSync")]
        public async Task<List<UsuariosEntity>> ConsultarUsuarioSync(bool pSincronizado)
        {
            try
            {
                var resultado = await _usuariosBL.ConsultarUsuarioSync(pSincronizado);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }

        #endregion

        #region Agregar/Actualizar Registros 
        /// <summary>
        /// Permite registrar un usuario en los servicios del Arreo y luego lo sincroniza en Azure.
        /// </summary>
        /// <param name="pUsuario">Modelo del tipo usuario.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Insertar
        ///     {
        ///        "UsuariosEntity": [
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
        public async Task<bool> RegistrarUsuario([FromBody] UsuariosEntity pUsuario)
        {
            try
            {
                var ruta = "";
                pUsuario.Sincronizado = false;

                using (HttpClient client = new HttpClient())
                {
                    ruta = BaseUrl + "api/Clientes/Insertar";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pUsuario).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadAsAsync<bool>();

                        if (res)
                        {
                            pUsuario.Sincronizado = true;
                        }

                    }

                    bool result = await _usuariosBL.GuardarUsuarios(pUsuario);

                    return result;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar la promocion, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite registrar/actualizar un usuario en Azure 
        /// (para actualizar utilizar las llaves unicas).
        /// </summary>
        /// <param name="pUsuario">Modelo de usuario a insertar o actualizar.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Insertar-Actualizar
        ///     {
        ///        "UsuariosEntity": [
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
        [Route("RegistraUsuario")]
        public async Task<bool> RegistrarUsuarioAzure([FromBody] UsuariosEntity pUsuario)
        {
            try
            {
            
               return await _usuariosBL.GuardarUsuarios(pUsuario);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar el usuario, favor comunicarse con el administrador.", ex);
            }
        }

        /// <summary>
        /// Permite actualizar un usuario en los servicios del arreo y luego lo sincroniza en Azure.
        /// </summary>
        /// <param name="pUsuario">Modelo a insertar.</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     POST /Actualizar
        ///     {
        ///        "UsuariosEntity": [
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
        [Route("Actualiza")]
        public async Task<bool> ActualizarUsuario([FromBody] UsuariosEntity pUsuario)
        {
            try
            {
                var porPuntos = Request.Headers.FirstOrDefault(x => x.Key == "PorPuntos").Value.FirstOrDefault();

                var ruta = "";
                pUsuario.Sincronizado = false;

                using (HttpClient client = new HttpClient())
                {
                    ruta = BaseUrl + "api/Clientes/Actualizar";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync(ruta, pUsuario);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadAsAsync<bool>();

                        if (res) 
                        {
                            pUsuario.Sincronizado = true;
                        }

                    }

                }

                bool result = await _usuariosBL.GuardarUsuarios(pUsuario);
                //consumir
                if (!string.IsNullOrEmpty(porPuntos))
                {
                    //consumir
                    var respuesta = await ConsumirInsertaReferenciaramigoIdApp(pUsuario.Cedula);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al agregar la promocion, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        #region Eliminar registros
        /// <summary>
        /// Permite eliminar un usuario desde Azure.
        /// </summary>
        /// <param name="pUsuario">Modelo a eliminar</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     PUT /Eliminar
        ///     {
        ///        "UsuariosEntity": [
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
        public async Task<bool> EliminarUsuario([FromBody] UsuariosEntity pUsuario)
        {
            try
            {
                return await _usuariosBL.EliminarUsuarios(pUsuario);

            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al eliminar el usuario, favor comunicarse con el administrador.", ex);
            }
        }
        #endregion

        /// <summary>
        /// Permite insertar una referencia a un amigo.
        /// </summary>
        /// <param name="identificacionCliente">Código del cliente</param>
        /// <returns>Estado de la petición.</returns>
        private async Task<bool> ConsumirInsertaReferenciaramigoIdApp(string identificacionCliente)
        {
            var icrBody = new InsertaReferenciaramigoIdAppRequestBody
            {
                pidentificacionCliente = identificacionCliente
            };

            var icrequest = new InsertaReferenciaramigoIdAppRequest(icrBody);

            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0, 2, 30),
                SendTimeout = new TimeSpan(0, 2, 30),
                OpenTimeout = new TimeSpan(0, 2, 30)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointInsercionApp);
            using (var myChannelFactory = new ChannelFactory<InsercionAppSoap>(myBinding, myEndpoint))
            {
                InsercionAppSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.InsertaReferenciaramigoIdAppAsync(icrequest);

                    var result = a.Body.InsertaReferenciaramigoIdAppResult;

                    ((ICommunicationObject)client).Close();
                    myChannelFactory.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    (client as ICommunicationObject)?.Abort();
                    return false;
                }
            }
        }


        /// <summary>
        /// Permite enviar un correo electronico
        /// </summary>
        /// <param name="emailModel">Datos en formato JSON para autenticar usuario con contraseña encriptada.</param>
        /// <returns>Usuario.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("SendEmail")]
        public async Task<int> SendEmail([FromBody] EmailModel emailModel)
        {
            try
            {
                Plantillas_Correo Plantillas = new Plantillas_Correo();
                ValidationString validationString = new ValidationString();

                switch (emailModel.Subject)
                {
                    case Plantillas_Correo.TitleRecoverPassword:
                        emailModel.Body = Plantillas.Mail_ChangePassword(emailModel.Body);
                        break;
                    default:
                        emailModel.Body = Plantillas.SendMessage(emailModel.Body);
                        break;
                }

                var message = new MimeMessage();
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                message.From.Add(new MailboxAddress(emailModel.User));
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                message.To.Add(new MailboxAddress(emailModel.Recipient));
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                message.Subject = emailModel.Subject;

                message.Body = new TextPart("html")
                {
                    Text = emailModel.Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(emailModel.Client, emailModel.Port, false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(emailModel.User, validationString.Desencriptar(emailModel.Pass));

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se presentó un error al consultar los usuarios, favor comunicarse con el administrador.", ex);
            }
        }
    }
}
