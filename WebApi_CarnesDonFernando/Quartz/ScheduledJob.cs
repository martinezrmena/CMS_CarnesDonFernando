using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi_CarnesDonFernando.Controllers;

namespace WebApi_CarnesDonFernando.Quartz
{
    public class ScheduledJob : IJob
    {
        #region Properties
        private readonly IConfiguration configuration;
        private readonly ILogger<ScheduledJob> logger;

        private readonly ClientesController clientesController = new ClientesController();
        private readonly UsuariosController usuariosController = new UsuariosController();
        private readonly WebController webController = new WebController();
        private readonly PuntosAppController puntosAppController = new PuntosAppController();
        private readonly DistritoController distritoController = new DistritoController();
        private readonly SucursalesController sucursalController = new SucursalesController();
        #endregion

        public ScheduledJob(IConfiguration configuration, ILogger<ScheduledJob> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            HttpContext _context = context.JobDetail.JobDataMap["context"] as HttpContext;

            try
            {
                //Trae clientes de Oracle - CIISA
                var listaClientes = await clientesController.Consultarclientes();

                //Trae clientes en azure
                var listClientesAzure = await usuariosController.Consulta();

                //Trae todos los puntos de todos los usuarios en CIISA
                var listPuntos = await webController.ConsultarPuntosGlobal();

                //Trae todos los puntos de todos los usuarios en Azure
                var listPuntosAzure = await puntosAppController.ConsultarPuntosGlobalAzure();

                //Trae los distritos de CIISA
                var listaDistritos = await distritoController.ConsultarDistritos();
                var lisDistritosAzure = await distritoController.ConsultaDistritos();
                DistritoEntity distritoEntity;

                //Trae las sucursales de CIISA
                var listaSucursales = await sucursalController.ConsultarSucursales();
                var lisSucursalesAzure = await sucursalController.ConsultaSucursales();
                SucursalEntity sucursalEntity;

                #region Distritos
                foreach (var distrito in listaDistritos ?? new System.Collections.Generic.List<DistritoModel>())
                {
                    try
                    {
                        if (!lisDistritosAzure.Any(x=> x.Descripcion == distrito.Descripcion))
                        {
                            //Si el distrito no existe en azure lo insertamos
                            distritoEntity = new DistritoEntity(distrito);
                            await distritoController.InsertarDistrito(distritoEntity);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Sucursales
                foreach (var sucursal in listaSucursales ?? new System.Collections.Generic.List<SucursalModel>())
                {
                    try
                    {
                        if (!lisSucursalesAzure.Any(x => x.Descripcion_Centro == sucursal.Descripcion_Centro))
                        {
                            //Si el distrito no existe en azure lo insertamos
                            sucursalEntity = new SucursalEntity(sucursal);
                            await sucursalController.InsertarSucursal(sucursalEntity);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion


                #region Datos BD Oracle
                //Verificamos los clientes en azure contra los clientes en CIISA
                foreach (var cliente in listaClientes ?? new System.Collections.Generic.List<ClientesModelXML>())
                {
                    try
                    {
                        if (cliente.Identif_Cliente.Length == 9)
                        {
                            //Consultar si cliente existe en la tabla de azure
                            var usuarioAzure = listClientesAzure.Where(x => x.Cedula == cliente.Identif_Cliente).ToList().FirstOrDefault();

                            if (usuarioAzure == null) //si no existe el cliente lo inserta 
                            {
                                string formattedDate = string.Empty;

                                try
                                {
                                    if (!string.IsNullOrEmpty(cliente.F_Nacimiento))
                                    {
                                        DateTime date = Convert.ToDateTime(cliente.F_Nacimiento);
                                        formattedDate = date.ToString("dd/MM/yyyy");
                                    }
                                }
                                catch (Exception)
                                {
                                    formattedDate = string.Empty;
                                }

                                var entityUser = new UsuariosEntity
                                {
                                    Nombre = cliente.Nombre,
                                    Apellido = cliente.Apellido1 + " " + cliente.Apellido2,
                                    Cedula = cliente.Identif_Cliente,
                                    Email = cliente.Email,
                                    CodigoCanton = cliente.Canton,
                                    CodigoProvincia = cliente.Provincia,
                                    Direccion_Exacta = cliente.Direccion,
                                    Puntos = cliente.Saldo_Puntos,
                                    Fecha_Nacimiento = formattedDate,
                                    Genero = (cliente.Sexo == "M" ? "Hombre" : "Mujer"),
                                    Telefono = cliente.Telefono,
                                    CodigoSucursal1 = cliente.Sucursal,
                                    Password = "",
                                    Codigo_Invitacion = "",
                                    Tipo_Cedula = (cliente.Tipo_Persona == "F" ? "Física" : "Jurídica"),
                                    PictureName = "",
                                    PartitionKey = Guid.NewGuid().ToString(),
                                    RowKey = Guid.NewGuid().ToString(),
                                    PictureURL = "",
                                    Canton = "",
                                    Provincia = "",
                                    Sucursal1 = "",
                                    Sincronizado = true
                                };

                                //await usuariosController.RegistrarUsuario(entityUser);
                                await usuariosController.RegistrarUsuarioAzure(entityUser);
                            }
                            else
                            {
                                //compara los puntos de la tabla de azure con los puntos de la BD de Carnes Don Fernando
                                if (usuarioAzure.Puntos != cliente.Saldo_Puntos.ToString())
                                {
                                    UsuariosEntity Entity = usuarioAzure;

                                    Entity.Puntos = cliente.Saldo_Puntos;

                                    Entity.Sincronizado = true;

                                    //await usuariosController.RegistrarUsuario(Entity);
                                    await usuariosController.RegistrarUsuarioAzure(Entity);
                                }
                                //else {
                                //    logDiferencia.Information("{0} - {1} - {2} - {3}", cliente.Identif_Cliente, cliente.Tipo_Persona, cliente.Email, "Los puntos son iguales en azure y en Oracle");
                                //}
                            }
                        }
                        //else {
                        //    logDiferencia.Information("{0} - {1} - {2} - {3}", cliente.Identif_Cliente, cliente.Tipo_Persona, cliente.Email,"La cedula no es igual a 9 digitos.");
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region Datos Cliente en Azure
                //Sincronizar la lista de Oracle con los datos de Azure que no hayan podido ser guardados
                var listAzureSync = await usuariosController.ConsultarUsuarioSync(false);

                foreach (var user in listAzureSync ?? new System.Collections.Generic.List<UsuariosEntity>())
                {
                    try
                    {
                        var temp = listaClientes.Where(x => x.Identif_Cliente == user.Cedula).FirstOrDefault();
                        bool result = false;

                        if (temp == null)
                        {
                            result = await clientesController.InsertarCliente(user);
                        }
                        else
                        {
                            result = await clientesController.ActualizarCliente(user);
                        }

                        if (result)
                        {
                            user.Sincronizado = true;
                        }

                        //await usuariosController.RegistrarUsuario(user);
                        await usuariosController.RegistrarUsuarioAzure(user);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                #endregion

                #region Datos Puntos
                if (listPuntos != null && listaClientes != null)
                {
                    try
                    {
                        PuntosEntity puntosEntity = new PuntosEntity();
                        ClientesModelXML clientesModelXML = new ClientesModelXML();
                        foreach (var punto in listPuntos ?? new System.Collections.Generic.List<PuntosModelXML>())
                        {

                            //Si no hay ninguno que coincida con estos datos entonces hay que insertarlo
                            if (!listPuntosAzure.Any(x => x.Id_Cliente == punto.Id_Cliente &&
                                                        x.Fecha_Mov == punto.Fecha_Mov &&
                                                        x.Descripcion == punto.Descripcion &&
                                                        x.Saldo == punto.Saldo &&
                                                        x.Centro == punto.Centro))
                            {
                                clientesModelXML = listaClientes.Where(x => x.Id_Cliente == punto.Id_Cliente).FirstOrDefault();

                                if (clientesModelXML != null)
                                {
                                    puntosEntity.Id_Cliente = clientesModelXML.Id_Cliente;
                                    puntosEntity.Cedula = clientesModelXML.Identif_Cliente;
                                    puntosEntity.Saldo = punto.Saldo;
                                    puntosEntity.Descripcion = punto.Descripcion;
                                    puntosEntity.Centro = punto.Centro;
                                    puntosEntity.Tipo_Mov = punto.Tipo_Mov;
                                    puntosEntity.Fecha_Mov = punto.Fecha_Mov;
                                    puntosEntity.PartitionKey = Guid.NewGuid().ToString();
                                    puntosEntity.RowKey = Guid.NewGuid().ToString();

                                    await puntosAppController.RegistrarPuntos(puntosEntity);
                                }
                            }
                        }

                        listPuntosAzure = await puntosAppController.ConsultarPuntosGlobalAzure();

                        foreach (var puntoAzure in listPuntosAzure)
                        {
                            //Verificaremos si el punto ya no existe en CIISA
                            if (!listPuntos.Any(x => x.Id_Cliente == puntoAzure.Id_Cliente &&
                                                        x.Fecha_Mov == puntoAzure.Fecha_Mov &&
                                                        x.Descripcion == puntoAzure.Descripcion &&
                                                        x.Saldo == puntoAzure.Saldo &&
                                                        x.Centro == puntoAzure.Centro))
                            {
                                //En cuyo caso no exista en CIISA debemos eliminarlo tambien de Azure
                                await puntosAppController.EliminarPunto(puntoAzure);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await Task.CompletedTask;
        }


    }
}
