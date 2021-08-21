using CarnesDonFernando.EL;
using CMS_CarnesDonFernando.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Helpers
{
    public class ProductoMesHelper
    {
        #region URL

        private readonly string BaseUrl;
    
        public ProductoMesHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        #region Consulta Productos 
        public List<ProductoMesModel> ConsultarProductosMes()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/ProductosMes/Consulta";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<List<ProductoMesModel>>().Result;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }
        #endregion

        #region Consultar por Llaves

        public ProductoMesModel ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/ProductosMes/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<ProductoMesModel>().Result;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return null;
            }
        }

        #endregion

        #region Consulta si existe producto

        public ProductoMesModel ExisteProducto(string pTitulo, string pDetalle, string pResumen, string pPreparacion, string pFechaIni, string pFechaFin)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/ProductosMes/Existe?pTitulo=" + pTitulo + "&pDetalle=" + pDetalle + "&pResumen=" + pResumen + "&pPreparacion=" + pPreparacion + "&pFechaIni=" + pFechaIni + "&pFechaFin=" + pFechaFin;
                    //var ruta = "http://localhost:53997/api/ProductosMes/Existe?pTitulo=" + pTitulo + "&pDetalle=" + pDetalle + "&pResumen=" + pResumen + "&pPreparacion=" + pPreparacion + "&pFechaIni=" + pFechaIni + "&pFechaFin=" + pFechaFin;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)                 
                        return  response.Content.ReadAsAsync<ProductoMesModel>().Result;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return null;
            }
        }

        #endregion

        #region Agregar/Actualizar Producto del mes

        public async Task<bool> Registrar(ProductoMesModel pProducto)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/ProductosMes/Registra";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pProducto).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<bool>();

                    }
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Eliminar registros 

        public async Task<bool> Eliminar(ProductoMesModel prom)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/ProductosMes/Elimina";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PutAsJsonAsync(ruta, prom).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<bool>();

                    }
                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


    }
}





        

        
