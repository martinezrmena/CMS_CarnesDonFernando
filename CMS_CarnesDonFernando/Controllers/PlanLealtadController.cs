using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CMS_CarnesDonFernando.Controllers
{
    public class PlanLealtadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public PlanLealtadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Instancias de clases

        PlanLealtadHelper _planLealtadHelper = new PlanLealtadHelper();
        AzureBlobStorageHelper _azureBlobHelper = new AzureBlobStorageHelper();

        #endregion

        #region Consultas Principal

        public IActionResult Index()
        {
            try
            {
                var datos = _planLealtadHelper.ConsultarPlanLealtad();
                return View(datos);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult ConsultarDatos(string tipoCliente, string descripcion, string beneficios, string bono)
        {
            bool resp = false;

            var query = _planLealtadHelper.ConsultarPlanLealtad().Where(
                x => x.TipoCliente == tipoCliente && x.Descripcion == descripcion &&
                x.Beneficios == beneficios && x.BonoNivel == bono).FirstOrDefault();

            if (query != null)
            {
                resp = true;
            }

            return Json(resp);
        }


        #endregion

        #region Agregar Plan de lealtad

        public JsonResult Formulario(string pLlaveParticion, string pLlaveFila)
        {
            var model = new PlanLealtadModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                model = _planLealtadHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);
                return Json(model);
            }

            return Json(null);
        }


        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosPlanLealtad datos)
        {
            var model = new PlanLealtadModel();

            string filepath = string.Empty;

            try
            {
                if (datos.image != null)
                {
                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + @"\Imagenes"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + @"\Imagenes");
                    }

                    filepath = Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.image.FileName);


                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        await datos.image.CopyToAsync(filestream);
                    }

                }


            }
            catch (Exception info)
            {

                throw new Exception("Error-" + info); ;
            }


            switch ("Agregar")
            {

                case "Agregar":
                    var urlImagenBlod = string.Empty;
                    if (filepath != string.Empty)
                    {
                        AzureBlobModel _model = new AzureBlobModel
                        {
                            UrlLocal = filepath,
                            NameContainer = "planimages"
                        };

                        var obj = _azureBlobHelper.SubirImagenCMS(_model);
                        urlImagenBlod = obj.Result;
                    }
                    else
                    {
                        urlImagenBlod = datos.imagenPath;
                    }

                    try
                    {

                        if (datos.image != null)
                        {
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.image.FileName));
                        }
                        else
                        {
                            string originalString = datos.imagenPath;
                            string reversedString = new string(originalString.Reverse().ToArray());
                            int posicion = reversedString.IndexOf('/');
                            string fileName = new string(reversedString.Substring(0, posicion).Reverse().ToArray());
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", fileName));
                        }

                    }

                    catch (Exception e)
                    {
                        throw new Exception("Error-" + e);
                    }


                    if (urlImagenBlod != null)
                    {
                        model = new PlanLealtadModel
                        {
                            Foto = urlImagenBlod,
                            TipoCliente = datos.tipoCliente,
                            Descripcion = datos.descripcion,
                            Beneficios = datos.beneficios,
                            BonoNivel = datos.bono,
                            PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                            RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
                        };
                    }

                    return await _planLealtadHelper.Registrar(model);
            }


        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult Eliminar(string pLlaveParticion, string pLlaveFila)
        {
            PlanLealtadModel promoModel = new PlanLealtadModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                promoModel = _planLealtadHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);

                var resultado = _planLealtadHelper.Eliminar(promoModel);

                return Json(resultado);
            }

            return Json(null);
        }

        #endregion
    }
}