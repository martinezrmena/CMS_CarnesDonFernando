using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMS_CarnesDonFernando.Controllers
{
    public class ParametrizacionController : Controller
    {
        ParametrizacionesHelper _helper = new ParametrizacionesHelper();

        public IActionResult Index()
        {
            try
            {
                var model = new ParametrizacionesModel();

                var datos = _helper.ConsultarParametrizacion();

              
                if (datos != null)
                {
                    string passDencrypt = _helper.Desencriptar(datos.Pass);

                    ParametrizacionesModel Settings = datos;

                    Settings.Pass = passDencrypt;

                    ViewBag.Accion = "Actualizar";

                    return View(Settings);

                }

                return View(model);
            }
            catch
            {
                return null;
            }
            
        }

        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosParametrizacion datos)
        {
            var model = new ParametrizacionesModel();

            string passEncrypt = _helper.Encriptar(datos.pass);

            model = new ParametrizacionesModel
            {
                User = datos.user,
                Port = datos.port,
                Client = datos.client,
                Pass = passEncrypt,
                Time = 0,
                Time_Des = "",
                PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
            };

            return await _helper.Registrar(model);
        }
    }
}