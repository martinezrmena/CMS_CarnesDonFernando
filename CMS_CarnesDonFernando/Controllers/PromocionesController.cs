using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using Microsoft.AspNetCore.Mvc;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CMS_CarnesDonFernando.Controllers
{
    public class PromocionesController : Controller
    {
        
        private IHostingEnvironment _hostingEnvironment;
        public PromocionesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Instancias de clases

        PromocionesHelper _promocionHelper = new PromocionesHelper();
        AzureBlobStorageHelper _azureBlobHelper = new AzureBlobStorageHelper();
        NotificacionHelper _notificacionHelper = new NotificacionHelper();
        
        #endregion

        #region Consultas Principal

        public IActionResult Index()
        {
            try
            {
                var datos = _promocionHelper.ConsultarPromociones();
                return View(datos);
            }
            catch
            {
                return null;
            }
           
        }

        [HttpPost]
        public JsonResult ConsultarDatos(string enlace, string titulo, string fechaInicio, string fechaFin)
        {
            bool resp = false;

            var query = _promocionHelper.ConsultarPromociones().Where(
                x => x.Enlace == enlace && x.Titulo == titulo && 
                x.Fecha_Publicacion == fechaInicio && x.Fecha_Finalizacion == fechaFin).FirstOrDefault(); 

            if (query != null)
            {
                resp = true;
            }
          
            return Json(resp);
        }

        #endregion

        #region Agregar Promociones

        public JsonResult Formulario(string pLlaveParticion, string pLlaveFila)
        {
            var model = new PromocionesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                model =  _promocionHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);
                return Json(model);
            }

            return Json(null);
        }


        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosPromocion datos)
        {
            bool estadoRegistrar = false;
            
            var model = new PromocionesModel();

            string filepath = string.Empty;
           
            try
            {
                if (datos.imagen != null)
                {
                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + @"\Imagenes"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + @"\Imagenes");
                    }

                    filepath = Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.imagen.FileName);


                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        await datos.imagen.CopyToAsync(filestream);
                    }                 
                }                
            }
            catch (Exception info )
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
                            NameContainer = "promotionimages"
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
                       
                        if(datos.imagen != null)
                        {
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.imagen.FileName));
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
                        model = new PromocionesModel
                        {
                            ImagenUrl = urlImagenBlod,
                            Enlace = datos.enlace,
                            Fecha_Publicacion = datos.fechainicia,
                            Fecha_Finalizacion = datos.fechafin,
                            Titulo = datos.titulo,
                            PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                            RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
                        };
                    }

                    estadoRegistrar = await _promocionHelper.Registrar(model);

                    if (estadoRegistrar)
                    {
                        //envia notificacion Android
                        await _notificacionHelper.Enviar_Android(model);

                        //envia notificacion iOS
                        await _notificacionHelper.Enviar_iOS(model);
                    }

                    return estadoRegistrar;         
            }          
        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult Eliminar(string pLlaveParticion, string pLlaveFila)
        {
            PromocionesModel promoModel = new PromocionesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                promoModel = _promocionHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);

                var resultado = _promocionHelper.Eliminar(promoModel);

                return Json(resultado);
            }

            return Json(null);
        }

        #endregion
    }
}