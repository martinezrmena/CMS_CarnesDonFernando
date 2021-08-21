using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMS_CarnesDonFernando.Controllers
{
    public class AcercaDeController : Controller
    {
        AcercaDeHelper _acercadeHelper = new AcercaDeHelper();
        AzureBlobStorageHelper _azureBlobHelper = new AzureBlobStorageHelper();
        public IActionResult Index()
        {
            try
            {
                var model = new AcercaDeModel();

                var datos = _acercadeHelper.ConsultarAcercaDe();

                if (datos != null)
                {
                    ViewBag.Accion = "Actualizar";
                    return View(datos);
                }

                return View(model);

            }
            catch
            {
                return null;
            }


        }

        public JsonResult Formulario(string pLlaveParticion, string pLlaveFila)
        {
            var model = new AcercaDeModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                model = _acercadeHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);
                return Json(model);
            }

            return Json(null);
        }
     
        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosAcercaDe datos)
        {
            var model = new AcercaDeModel();         
         
            model = new AcercaDeModel
            {
                     Titulo = datos.titulo,
                     Descripcion = datos.descripcion,
                     PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                     RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
            };
            
            return await _acercadeHelper.Registrar(model);

           
        }
    }
}