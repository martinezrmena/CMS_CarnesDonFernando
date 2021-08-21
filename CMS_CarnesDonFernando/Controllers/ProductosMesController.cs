using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace CMS_CarnesDonFernando.Controllers
{
    public class ProductosMesController : Controller
    {
        #region Instancias de clases

        ProductoMesHelper _productosHelper = new ProductoMesHelper();
        AzureBlobStorageHelper _azureBlobHelper = new AzureBlobStorageHelper();

        private IHostingEnvironment _hostingEnvironment;
        private readonly string IconoFrio;
        private readonly string IconoHorno;
        private readonly string IconoSarten;
        private readonly string IconoCoccion;
        private readonly string IconoParrilla;

        public ProductosMesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var AppSetting = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json")
                           .Build();
            IconoFrio = AppSetting.GetSection("IconoPreparacionFrio").Value;
            IconoCoccion = AppSetting.GetSection("IconoPreparacionCoccion").Value;
            IconoHorno = AppSetting.GetSection("IconoPreparacionHorno").Value;
            IconoParrilla = AppSetting.GetSection("IconoPreparacionParrilla").Value;
            IconoSarten = AppSetting.GetSection("IconoPreparacionSarten").Value;
        }

        #endregion

        #region Consultas Principal
        public IActionResult Index()
        {
            try
            {
                //iconos de preparacion
                ViewBag.Frio = IconoFrio;
                ViewBag.Horno = IconoHorno;
                ViewBag.Parrilla = IconoParrilla;
                ViewBag.Sarten = IconoSarten;
                ViewBag.Coccion = IconoCoccion;


                var datos = _productosHelper.ConsultarProductosMes();
                return View(datos);
            }
            catch
            {
                return null;
            }
            
        }

        [HttpPost]
        public JsonResult ConsultarDatos(string titulo, string detalle, string resumen, string preparacion, string fechaInicio, string fechaFin)
        {
            bool resp = false;
        
            var query = _productosHelper.ConsultarProductosMes().Where(
                x => x.Titulo == titulo && x.Descripcion_Detalle == detalle &&
                x.Descripcion_Resumen == resumen && x.Preparacion == preparacion && 
                x.Fecha_Publicacion == fechaInicio && x.Fecha_Finalizacion == fechaFin).FirstOrDefault();

            if (query != null)
            {
                resp = true;
            }

            return Json(resp);
        }

        #endregion

        #region Agregar Productos del mes

        public JsonResult Formulario(string pLlaveParticion, string pLlaveFila)
        {
            var model = new ProductoMesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                model = _productosHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);
                return Json(model);
            }

            return Json(null);
        }

        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosProductos datos)
        {
            var model = new ProductoMesModel();

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
                            NameContainer = "promotionimages"
                        };

                        var obj = _azureBlobHelper.SubirImagenCMS(_model);
                        urlImagenBlod = obj.Result;
                    }
                    else
                    {
                        urlImagenBlod = datos.imagenPath;
                    }

                    if (datos.imagen != null)
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

                    if (urlImagenBlod != null)
                    {
                        var listPreparacion = "";

                        if (datos.preparacion.Contains("Horno"))
                        {
                            if (listPreparacion == "")
                            {
                                listPreparacion = listPreparacion + IconoHorno;
                            }
                            else
                            {
                                listPreparacion = listPreparacion + "," + IconoHorno;
                            }
                        }
                        if (datos.preparacion.Contains("Frio"))
                        {
                            if (listPreparacion == "")
                            {
                                listPreparacion = listPreparacion + IconoFrio;
                            }
                            else
                            {
                                listPreparacion = listPreparacion + "," + IconoFrio;
                            }
                        }
                        if (datos.preparacion.Contains("Sarten"))
                        {
                            if (listPreparacion == "")
                            {
                                listPreparacion = listPreparacion + IconoSarten;
                            }
                            else
                            {
                                listPreparacion = listPreparacion + "," + IconoSarten;
                            }
                        }
                        if (datos.preparacion.Contains("Coccion"))
                        {
                            if (listPreparacion == "")
                            {
                                listPreparacion = listPreparacion + IconoCoccion;
                            }
                            else
                            {
                                listPreparacion = listPreparacion + "," + IconoCoccion;
                            }
                        }
                        if (datos.preparacion.Contains("Parrilla"))
                        {
                            if (listPreparacion == "")
                            {
                                listPreparacion = listPreparacion + IconoParrilla;
                            }
                            else
                            {
                                listPreparacion = listPreparacion + "," + IconoParrilla;
                            }
                        }


                        model = new ProductoMesModel
                        {
                            ImagenUrl = urlImagenBlod,
                            Fecha_Publicacion = datos.fechainicia,
                            Fecha_Finalizacion = datos.fechafin,
                            Titulo = datos.titulo,
                            Descripcion_Resumen = datos.descripcion_Resumen,
                            Descripcion_Detalle = datos.descripcion_Detalle,
                            Preparacion = datos.preparacion,
                            UrlIconosPreparacion = listPreparacion,
                            PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                            RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
                        };
                    }

                    return await _productosHelper.Registrar(model);


            }


        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult Eliminar(string pLlaveParticion, string pLlaveFila)
        {
            ProductoMesModel productModel = new ProductoMesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                productModel = _productosHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);

                var resultado = _productosHelper.Eliminar(productModel);

                return Json(resultado);
            }

            return Json(null);
        }

    }
    #endregion


}