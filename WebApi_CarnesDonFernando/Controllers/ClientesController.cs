using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Mvc;
using WebApi_CarnesDonFernando.Helpers;
using System.Text.RegularExpressions;
using MomentosDonFernando;
using InsercionApp;
using ActualizacionApp;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Permite administrar a los usuarios/clientes de la aplicación
    /// </summary>
    [Produces("application/json")]
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        #region Consultas clientes BD Carnes Don Fernando
        /// <summary>
        /// Permite obtener a los clientes que se encuentran en el servidor de CIISA.
        /// </summary>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consultar
        ///     {
        ///        "ClientesModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1",
        ///                                 "Identif_Cliente": "123456789",
        ///                                 "Tipo_Persona": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido1": "Martínez",
        ///                                 "Apellido2": "Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "F_Nacimiento": "01/05/1980",
        ///                                 "Sexo": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "2",
        ///                                 "Canton": "20",
        ///                                 "Direccion": "Este es el lugar de locación",
        ///                                 "Sucursal": "49",
        ///                                 "Saldo_Puntos": "0"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Lista de clientes extraido de formato XML.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("Consulta")]
        public async Task<List<ClientesModelXML>> Consultarclientes()
        {
          
            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                OpenTimeout = new TimeSpan(0, 3, 0),
                SendTimeout = new TimeSpan(0, 3, 0),
                CloseTimeout = new TimeSpan(0, 3, 0)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointConsutaApp);
            using (var myChannelFactory = new ChannelFactory<ConsultaAppSoap>(myBinding, myEndpoint))
            {
                ConsultaAppSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.consultaClientesAppAsync();

                    var result = a.Any1;


                    //var datos = result.Descendants("tabla").Select(d =>
                    var datos = result.Descendants("CLIENTES").Select(d =>
                        new ClientesModelXML
                        {
                            Id_Cliente = d.Element("ID_CLIENTE").ElementValueNull(),
                            Identif_Cliente = d.Element("IDENTIF_CLIENTE").ElementValueNull(),
                            Tipo_Persona = d.Element("TIPO_PERSONA").ElementValueNull(),
                            Nombre = d.Element("NOMBRE").Value,
                            Apellido1 = d.Element("APELLIDO1").ElementValueNull(),
                            Apellido2 = d.Element("APELLIDO2").ElementValueNull(),
                            Email = d.Element("EMAIL1").ElementValueNull(),
                            F_Nacimiento = d.Element("F_NACIMIENTO").ElementValueNull(),
                            Sexo = d.Element("SEXO").ElementValueNull(),
                            Telefono = d.Element("TELEFONO").ElementValueNull(),
                            Provincia = d.Element("PROVINCIA").ElementValueNull(),
                            Canton = d.Element("CANTON").ElementValueNull(),
                            Sucursal = d.Element("SUCURSAL").ElementValueNull(),
                            Saldo_Puntos = d.Element("SALDO_PUNTOS").ElementValueNull(),
                            Direccion = d.Element("DIRECCION").ElementValueNull()
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
        /// Permite obtener un cliente desde los servicios de CIISA.
        /// </summary>
        /// <param name="sCedula">Identificación del cliente que se desea obtener</param>
        /// <remarks>
        /// Ejemplo de resultado:
        ///
        ///     GET /Consultar
        ///     {
        ///        "ClientesModelXML": [
        ///                             {
        ///                                 "Id_Cliente": "1",
        ///                                 "Identif_Cliente": "123456789",
        ///                                 "Tipo_Persona": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido1": "Martínez",
        ///                                 "Apellido2": "Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "F_Nacimiento": "01/05/1980",
        ///                                 "Sexo": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "2",
        ///                                 "Canton": "20",
        ///                                 "Direccion": "Este es el lugar de locación",
        ///                                 "Sucursal": "49",
        ///                                 "Saldo_Puntos": "0"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Modelo de cliente.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Route("ConsultaCliente")]
        public async Task<ClientesModelXML> Consultarcliente(string sCedula)
        {
            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                OpenTimeout = new TimeSpan(0,3,0),
                SendTimeout = new TimeSpan(0, 3, 0),
                CloseTimeout = new TimeSpan(0, 3, 0)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointConsutaApp);
            using (var myChannelFactory = new ChannelFactory<ConsultaAppSoap>(myBinding, myEndpoint))
            {
                ConsultaAppSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.consultaClientesAppAsync();

                    var result = a.Any1;

                    var datos = result.Descendants("CLIENTES").Select(d =>
                        new ClientesModelXML
                        {
                            Id_Cliente = (d.Element("ID_CLIENTE").ElementValueNull()),
                            Identif_Cliente = d.Element("IDENTIF_CLIENTE").ElementValueNull(),
                            Tipo_Persona = d.Element("TIPO_PERSONA").ElementValueNull(),
                            Nombre = d.Element("NOMBRE").ElementValueNull(),
                            Apellido1 = d.Element("APELLIDO1").ElementValueNull(),
                            Apellido2 = d.Element("APELLIDO2").ElementValueNull(),
                            Email = d.Element("EMAIL").ElementValueNull(),
                            F_Nacimiento = (d.Element("F_NACIMIENTO").ElementValueNull()),
                            Sexo = d.Element("SEXO").ElementValueNull(),
                            Telefono = d.Element("TELEFONO").ElementValueNull(),
                            Provincia = d.Element("PROVINCIA").ElementValueNull(),
                            Canton = d.Element("CANTON").ElementValueNull(),
                            Sucursal = (d.Element("SUCURSAL").ElementValueNull()),
                            Saldo_Puntos = (d.Element("SALDO_PUNTOS").ElementValueNull())
                        }
                    ).Where(x => x.Identif_Cliente == sCedula);

                    ((ICommunicationObject)client).Close();
                    myChannelFactory.Close();

                    return datos.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    (client as ICommunicationObject)?.Abort();
                    return null;
                }
            }
        }
        #endregion

        #region Insertar clientes BD Carnes Don Fernando
        /// <summary>
        /// Permite insertar un cliente en los servicios de CIISA con una contraseña ya encriptada.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        /// NOTA IMPORTANTE: Actualmente el método de CIISA no permite envio de contraseña.
        ///
        ///     POST /Insertar
        ///     {
        ///        "UsuariosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Cedula": "123456789",
        ///                                 "Password": "qdexHG++5DG5EJ/ZujVwcQ==",
        ///                                 "Tipo_Cedula": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido": "Martínez Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "Fecha_Nacimiento": "01/05/1980",
        ///                                 "Genero": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "Alajuela",
        ///                                 "CodigoProvincia": "2",
        ///                                 "Canton": "Canton",
        ///                                 "CodigoCanton": "20",
        ///                                 "Direccion_Exacta": "Este es el lugar de locación",
        ///                                 "Sucursal1": "Restaurante Heredia",
        ///                                 "CodigoSucursal1": "49",
        ///                                 "Sucursal2" "Restaurante Pinares",
        ///                                 "Codigo_Invitacion": "XXXXXXXX",
        ///                                 "PictureURL": "www.imagen.com",
        ///                                 "PictureName": "Nueva.jpg",
        ///                                 "Puntos": "0",
        ///                                 "Sincronizado": "false"
        ///                             }
        ///                         ]
        ///     }
        /// </remarks>
        /// <returns>Retorna la información del acerca de Momentos Don Fernando.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Insertar")]
        public async Task<bool> InsertarCliente(UsuariosEntity model)
        {
            try
            {
                insertarClienteRequestBody icrBody = new insertarClienteRequestBody();
                int contador = 0;
                icrBody.pidentificacionCliente = model.Cedula;
                icrBody.pNombre = model.Nombre;

                switch (model.Tipo_Cedula)
                {
                    case ClientesHelper.Nacional:
                        icrBody.ptipoPersona = ClientesHelper.NacionalSigla;
                        break;
                    case ClientesHelper.Extranjero:
                        icrBody.ptipoPersona = ClientesHelper.Extranjero;
                        break;
                    default:
                        icrBody.ptipoPersona = ClientesHelper.FisicaSigla;
                        break;
                }

                foreach (string splitstring in Regex.Split(model.Apellido, " "))
                {
                    if (contador == 0)
                    {
                        icrBody.pApellido1 = splitstring;
                    }
                    else 
                    {
                        icrBody.pApellido2 = splitstring;
                    }
                    contador++;
                }

                icrBody.pEmail1 = model.Email;
                icrBody.pDireccion = model.Direccion_Exacta;

                switch (model.Genero)
                {
                    case ClientesHelper.Masculino:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroMasculino;
                        break;
                    case ClientesHelper.Femenino:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroFemenino;
                        break;
                    case ClientesHelper.PrefiereNoDecirlo:
                        icrBody.pSexo = ClientesHelper.PrefiereNoDecirloSigla;
                        break;
                    default:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroMasculino;
                        break;
                }

                icrBody.pProvincia = model.CodigoProvincia;
                icrBody.pSucursal = model.CodigoSucursal1;
                icrBody.pTelefono = model.Telefono;
                if (string.IsNullOrEmpty(model.Puntos))
                {
                    icrBody.pSaldoPuntos = "0";
                }
                else 
                {
                    icrBody.pSaldoPuntos = model.Puntos;
                }
                icrBody.pFechaNacimiento = model.Fecha_Nacimiento;
                icrBody.pCanton = model.CodigoCanton;

                insertarClienteRequest icrequest = new insertarClienteRequest(icrBody);

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
                        var a = await client.insertarClienteAsync(icrequest);

                        var result = a.Body.insertarClienteResult;

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
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite insertar un cliente en los servicios de CIISA con una contraseña sin encriptar.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        /// NOTA IMPORTANTE: Actualmente el método de CIISA no permite envio de contraseña.
        ///
        ///     POST /Insertar
        ///     {
        ///        "UsuariosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Cedula": "123456789",
        ///                                 "Password": "contraejemplo#$1", //Es necesaria una contraseña sin encriptar
        ///                                 "Tipo_Cedula": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido": "Martínez Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "Fecha_Nacimiento": "01/05/1980",
        ///                                 "Genero": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "Alajuela",
        ///                                 "CodigoProvincia": "2",
        ///                                 "Canton": "Canton",
        ///                                 "CodigoCanton": "20",
        ///                                 "Direccion_Exacta": "Este es el lugar de locación",
        ///                                 "Sucursal1": "Restaurante Heredia",
        ///                                 "CodigoSucursal1": "49",
        ///                                 "Sucursal2" "Restaurante Pinares",
        ///                                 "Codigo_Invitacion": "XXXXXXXX",
        ///                                 "PictureURL": "www.imagen.com",
        ///                                 "PictureName": "Nueva.jpg",
        ///                                 "Puntos": "0",
        ///                                 "Sincronizado": "false"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Retorna la información del acerca de Momentos Don Fernando.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("InsertarClienteEncript")]
        public async Task<bool> InsertarClienteEncript(UsuariosEntity model)
        {
            try
            {
                insertarClienteRequestBody icrBody = new insertarClienteRequestBody();
                ValidationString validationString = new ValidationString();
                int contador = 0;
                icrBody.pidentificacionCliente = model.Cedula;
                icrBody.pNombre = model.Nombre;
                model.Password = validationString.Encriptar(model.Password);

                switch (model.Tipo_Cedula)
                {
                    case ClientesHelper.Juridica:
                        icrBody.ptipoPersona = ClientesHelper.JuridicaSigla;
                        break;
                    case ClientesHelper.Fisica:
                        icrBody.ptipoPersona = ClientesHelper.FisicaSigla;
                        break;
                    default:
                        icrBody.ptipoPersona = ClientesHelper.FisicaSigla;
                        break;
                }

                foreach (string splitstring in Regex.Split(model.Apellido, " "))
                {
                    if (contador == 0)
                    {
                        icrBody.pApellido1 = splitstring;
                    }
                    else
                    {
                        icrBody.pApellido2 = splitstring;
                    }
                    contador++;
                }

                icrBody.pEmail1 = model.Email;
                icrBody.pDireccion = model.Direccion_Exacta;

                switch (model.Genero)
                {
                    case ClientesHelper.Masculino:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroMasculino;
                        break;
                    case ClientesHelper.Femenino:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroFemenino;
                        break;
                    default:
                        icrBody.pSexo = ClientesHelper.SiglaGeneroMasculino;
                        break;
                }

                icrBody.pProvincia = model.CodigoProvincia;
                icrBody.pSucursal = model.CodigoSucursal1;
                icrBody.pTelefono = model.Telefono;
                if (string.IsNullOrEmpty(model.Puntos))
                {
                    icrBody.pSaldoPuntos = "0";
                }
                else
                {
                    icrBody.pSaldoPuntos = model.Puntos;
                }
                icrBody.pFechaNacimiento = model.Fecha_Nacimiento;
                icrBody.pCanton = model.CodigoCanton;

                insertarClienteRequest icrequest = new insertarClienteRequest(icrBody);

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
                        var a = await client.insertarClienteAsync(icrequest);

                        var result = a.Body.insertarClienteResult;

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
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Actualizar cliente BD Carnes Don Fernando 
        /// <summary>
        /// Permite actualizar un cliente en los servidores de CIISA con contraseña ya encriptada.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        /// NOTA IMPORTANTE: Actualmente el método de CIISA no permite envio de contraseña.
        ///
        ///     POST /Actualizar
        ///     {
        ///        "UsuariosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Cedula": "123456789",
        ///                                 "Password": "qdexHG++5DG5EJ/ZujVwcQ==", //Es necesaria una contraseña sin encriptar
        ///                                 "Tipo_Cedula": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido": "Martínez Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "Fecha_Nacimiento": "01/05/1980",
        ///                                 "Genero": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "Alajuela",
        ///                                 "CodigoProvincia": "2",
        ///                                 "Canton": "Canton",
        ///                                 "CodigoCanton": "20",
        ///                                 "Direccion_Exacta": "Este es el lugar de locación",
        ///                                 "Sucursal1": "Restaurante Heredia",
        ///                                 "CodigoSucursal1": "49",
        ///                                 "Sucursal2" "Restaurante Pinares",
        ///                                 "Codigo_Invitacion": "XXXXXXXX",
        ///                                 "PictureURL": "www.imagen.com",
        ///                                 "PictureName": "Nueva.jpg",
        ///                                 "Puntos": "0",
        ///                                 "Sincronizado": "false"
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
        [Route("Actualizar")]
        public async Task<bool> ActualizarCliente(UsuariosEntity model)
        {
            actualizarClientePorCedulaRequestBody icrBody = new actualizarClientePorCedulaRequestBody();
            int contador = 0;
            icrBody.pcedula = model.Cedula;
            icrBody.pnombre = model.Nombre;
            foreach (string splitstring in Regex.Split(model.Apellido, " "))
            {
                if (contador == 0)
                {
                    icrBody.papellido1 = splitstring;
                }
                else
                {
                    icrBody.papellido2 = splitstring;
                }
                contador++;
            }
            icrBody.pcorreo = model.Email;
            icrBody.pdireccion = model.Direccion_Exacta;

            switch (model.Genero)
            {
                case ClientesHelper.Masculino:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroMasculino;
                    break;
                case ClientesHelper.Femenino:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroFemenino;
                    break;
                default:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroMasculino;
                    break;
            }

            icrBody.pprovincia = model.CodigoProvincia;
            icrBody.psucursal = model.CodigoSucursal1;
            icrBody.ptelefono1 = model.Telefono;

            //if (string.IsNullOrEmpty(model.Puntos))
            //{
            //    icrBody.pSaldoPuntos = "0";
            //}
            //else
            //{
            //    icrBody.pSaldoPuntos = model.Puntos;
            //}

            icrBody.pfechaNacimiento = model.Fecha_Nacimiento;
            icrBody.pcanton = model.CodigoCanton;


            actualizarClientePorCedulaRequest icrequest = new actualizarClientePorCedulaRequest(icrBody);

            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0, 2, 30),
                SendTimeout = new TimeSpan(0, 2, 30),
                OpenTimeout = new TimeSpan(0, 2, 30)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointActualizacionApp);
            using (var myChannelFactory = new ChannelFactory<ActualizacionSoap>(myBinding, myEndpoint))
            {
                ActualizacionSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.actualizarClientePorCedulaAsync(icrequest);

                    var result = a.Body.actualizarClientePorCedulaResult;

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
        /// Permite actualizar un cliente en los servidores de CIISA con contraseña pendiente de encriptar.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        /// NOTA IMPORTANTE: Actualmente el método de CIISA no permite envio de contraseña.
        ///
        ///     POST /Actualizar
        ///     {
        ///        "UsuariosEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Cedula": "123456789",
        ///                                 "Password": "contra123#$", //Es necesaria una contraseña sin encriptar
        ///                                 "Tipo_Cedula": "Física",
        ///                                 "Nombre": "Rafael Alfonso",
        ///                                 "Apellido": "Martínez Mena",
        ///                                 "Email": "ejemplo@hotmail.com",
        ///                                 "Fecha_Nacimiento": "01/05/1980",
        ///                                 "Genero": "Hombre",
        ///                                 "Telefono": "78522420",
        ///                                 "Provincia": "Alajuela",
        ///                                 "CodigoProvincia": "2",
        ///                                 "Canton": "Canton",
        ///                                 "CodigoCanton": "20",
        ///                                 "Direccion_Exacta": "Este es el lugar de locación",
        ///                                 "Sucursal1": "Restaurante Heredia",
        ///                                 "CodigoSucursal1": "49",
        ///                                 "Sucursal2" "Restaurante Pinares",
        ///                                 "Codigo_Invitacion": "XXXXXXXX",
        ///                                 "PictureURL": "www.imagen.com",
        ///                                 "PictureName": "Nueva.jpg",
        ///                                 "Puntos": "0",
        ///                                 "Sincronizado": "false"
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
        [Route("ActualizarClienteEncript")]
        public async Task<bool> ActualizarClienteEncript(UsuariosEntity model)
        {
            actualizarClientePorCedulaRequestBody icrBody = new actualizarClientePorCedulaRequestBody();
            ValidationString validationString = new ValidationString();
            int contador = 0;
            icrBody.pcedula = model.Cedula;
            icrBody.pnombre = model.Nombre;
            foreach (string splitstring in Regex.Split(model.Apellido, " "))
            {
                if (contador == 0)
                {
                    icrBody.papellido1 = splitstring;
                }
                else
                {
                    icrBody.papellido2 = splitstring;
                }
                contador++;
            }
            icrBody.pcorreo = model.Email;
            icrBody.pdireccion = model.Direccion_Exacta;

            switch (model.Genero)
            {
                case ClientesHelper.Masculino:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroMasculino;
                    break;
                case ClientesHelper.Femenino:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroFemenino;
                    break;
                default:
                    icrBody.pgenero = ClientesHelper.SiglaGeneroMasculino;
                    break;
            }

            icrBody.pprovincia = model.CodigoProvincia;
            icrBody.psucursal = model.CodigoSucursal1;
            icrBody.ptelefono1 = model.Telefono;

            //if (string.IsNullOrEmpty(model.Puntos))
            //{
            //    icrBody.pSaldoPuntos = "0";
            //}
            //else
            //{
            //    icrBody.pSaldoPuntos = model.Puntos;
            //}

            icrBody.pfechaNacimiento = model.Fecha_Nacimiento;
            icrBody.pcanton = model.CodigoCanton;


            actualizarClientePorCedulaRequest icrequest = new actualizarClientePorCedulaRequest(icrBody);

            var myBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 20000000,
                CloseTimeout = new TimeSpan(0, 2, 30),
                SendTimeout = new TimeSpan(0, 2, 30),
                OpenTimeout = new TimeSpan(0, 2, 30)
            };
            var myEndpoint = new EndpointAddress(HelperMomentosDonFernando.EndPointActualizacionApp);
            using (var myChannelFactory = new ChannelFactory<ActualizacionSoap>(myBinding, myEndpoint))
            {
                ActualizacionSoap client = null;

                try
                {
                    client = myChannelFactory.CreateChannel();
                    var a = await client.actualizarClientePorCedulaAsync(icrequest);

                    var result = a.Body.actualizarClientePorCedulaResult;

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
        #endregion

    }
}