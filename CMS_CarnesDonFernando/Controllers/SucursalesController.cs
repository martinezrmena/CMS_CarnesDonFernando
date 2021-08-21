using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Helpers;
using CMS_CarnesDonFernando.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CMS_CarnesDonFernando.Controllers
{
    public class SucursalesController : Controller
    {

        private IHostingEnvironment _hostingEnvironment;
        public SucursalesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #region Instancias de clases

        SucursalesHelpers _sucursalesHelper = new SucursalesHelpers();
        AzureBlobStorageHelper _azureBlobHelper = new AzureBlobStorageHelper();

        #endregion
        public IActionResult Index()
        {
            try
            {
                var datos = _sucursalesHelper.ConsultarSucursales();
                return View(datos);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult ConsultarDatos(string nombre, string enlaceGM, string enlaceWZ, string telTienda, string telRestaurante, string horarioTienda, string horarioRestaurante)
        {
            bool resp = false;

            var query = _sucursalesHelper.ConsultarSucursales().Where(
                x => x.Nombre == nombre && x.EnlaceGoogleMaps == enlaceGM &&
                x.EnlaceWaze == enlaceWZ && x.TelefonoTienda == telTienda &&
                x.TelefonoRestaurante == telRestaurante && x.HorarioTienda == horarioTienda &&
                x.HorarioRestaurante == horarioRestaurante).FirstOrDefault();

            if (query != null)
            {
                resp = true;
            }

            return Json(resp);
        }

        #region Agregar Sucursal

        public JsonResult Formulario(string pLlaveParticion, string pLlaveFila)
        {
            var model = new SucursalesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                model = _sucursalesHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);
                return Json(model);
            }

            return Json(null);
        }


        [HttpPost]
        public async Task<bool> EjecutarAcciones(DatosSucursal datos)
        {
            var model = new SucursalesModel();

            string filepath = string.Empty;
            string filepathWaze = string.Empty;
            string filepathMaps = string.Empty;

            try
            {

                if (datos.foto != null)
                {
                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + @"\Imagenes"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + @"\Imagenes");
                    }

                    filepath = Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.foto.FileName);

                    //Foto de la sucursal
                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        await datos.foto.CopyToAsync(filestream);
                    }

                }

                if (datos.iconoGoogleMaps != null){

                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + @"\Imagenes"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + @"\Imagenes");
                    }
                    filepathMaps = Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.iconoGoogleMaps.FileName);
                    
                    //icono google maps
                    using (var filestream = new FileStream(filepathMaps, FileMode.Create))
                    {
                        await datos.iconoGoogleMaps.CopyToAsync(filestream);
                    }
                }

                if (datos.iconoWaze != null)
                {
                    if (!Directory.Exists(_hostingEnvironment.ContentRootPath + @"\Imagenes"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + @"\Imagenes");
                    }
                    filepathWaze = Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.iconoWaze.FileName);

                    //icono Waze
                    using (var filestream = new FileStream(filepathWaze, FileMode.Create))
                    {
                        await datos.iconoWaze.CopyToAsync(filestream);
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
                    var urlImagenBlodWaze = string.Empty;
                    var urlImagenBlodMaps = string.Empty;

                    if (filepath != string.Empty)
                    {
                        AzureBlobModel _model = new AzureBlobModel
                        {
                            UrlLocal = filepath,
                            NameContainer = "branchofficeimages"

                        };

                        var obj = _azureBlobHelper.SubirImagenCMS(_model);
                        urlImagenBlod = obj.Result;
                    } else
                    {
                        urlImagenBlod = datos.imagenPath;
                    }

                    if (filepathMaps != string.Empty)
                    {
                        AzureBlobModel _modelM = new AzureBlobModel
                        {
                            UrlLocal = filepathMaps,
                            NameContainer = "branchofficeimages"
                        };

                        var objMaps = _azureBlobHelper.SubirImagenCMS(_modelM);
                        urlImagenBlodMaps = objMaps.Result;
                    } else
                    {
                        urlImagenBlodMaps = datos.imagenPathMaps;
                    }

                    if (filepathWaze != string.Empty)
                    {
                        AzureBlobModel _modelW = new AzureBlobModel
                        {
                            UrlLocal = filepathWaze,
                            NameContainer = "branchofficeimages"

                        };

                        var objWaze = _azureBlobHelper.SubirImagenCMS(_modelW);
                        urlImagenBlodWaze = objWaze.Result;
                    }
                    else
                    {
                        urlImagenBlodWaze = datos.imagenPathWaze;
                    }

                    try
                    {
                        //Foto de la sucursal
                        if (datos.foto != null){

                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.foto.FileName));
                        }
                        else
                        {
                            string originalString = datos.imagenPath;
                            string reversedString = new string(originalString.Reverse().ToArray());
                            int posicion = reversedString.IndexOf('/');
                            string fileName = new string(reversedString.Substring(0, posicion).Reverse().ToArray());
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", fileName));

                        }

                        //Waze
                        if (datos.iconoWaze != null)
                        {
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.iconoWaze.FileName));

                        }
                        else
                        {
                            string originalStringW = datos.imagenPathWaze;
                            string reversedStringW = new string(originalStringW.Reverse().ToArray());
                            int posicionW = reversedStringW.IndexOf('/');
                            string fileNameW = new string(reversedStringW.Substring(0, posicionW).Reverse().ToArray());
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", fileNameW));

                        }

                        //Google Maps
                        if (datos.iconoGoogleMaps != null)
                        {
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", datos.iconoGoogleMaps.FileName));

                        }
                        else
                        {
                            string originalStringM = datos.imagenPathMaps;
                            string reversedStringM = new string(originalStringM.Reverse().ToArray());
                            int posicionM = reversedStringM.IndexOf('/');
                            string fileNameM = new string(reversedStringM.Substring(0, posicionM).Reverse().ToArray());
                            System.IO.File.Delete(Path.Combine(_hostingEnvironment.ContentRootPath + @"\Imagenes", fileNameM));

                        }
                    }


                    catch (Exception e)
                    {
                        throw new Exception("Error-" + e);
                    }


                    if (urlImagenBlod != null && urlImagenBlodMaps != null && urlImagenBlodWaze != null)
                    {
                        var telefonos = datos.telefonoTienda + "," + datos.telefonoRestaurante;

                        model = new SucursalesModel
                        {
                            FotoUrl = urlImagenBlod,
                            IconoGoogleMaps = urlImagenBlodMaps,
                            IconoWaze = urlImagenBlodWaze,
                            Nombre = datos.nombre,
                            EnlaceGoogleMaps = datos.direccionGoogleMaps,
                            EnlaceWaze = datos.direccionWaze,
                            TelefonoTienda = datos.telefonoTienda,
                            TelefonoRestaurante = datos.telefonoRestaurante,
                            HorarioTienda = datos.horarioTienda,
                            HorarioRestaurante = datos.horarioRestaurante,
                            Telefonos = telefonos,
                            PartitionKey = (datos.PartitionKey != string.Empty && datos.PartitionKey != null ? datos.PartitionKey : Guid.NewGuid().ToString()),
                            RowKey = (datos.RowKey != string.Empty && datos.RowKey != null ? datos.RowKey : Guid.NewGuid().ToString())
                        };
                    }

                    return await _sucursalesHelper.Registrar(model);
                   

            }


        }
        #endregion

        #region Eliminar
        [HttpPost]
        public JsonResult Eliminar(string pLlaveParticion, string pLlaveFila)
        {
            SucursalesModel promoModel = new SucursalesModel();

            if (pLlaveParticion != null && pLlaveFila != null)
            {
                promoModel = _sucursalesHelper.ConsultarPorLlaves(pLlaveParticion, pLlaveFila);

                var resultado = _sucursalesHelper.Eliminar(promoModel);

                return Json(resultado);
            }

            return Json(null);
        }





        #endregion
    }
}