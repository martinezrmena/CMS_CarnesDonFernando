using CMS_CarnesDonFernando.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Helpers
{
    public class ParametrizacionesHelper
    {

        #region URL

        private readonly string BaseUrl;

        public ParametrizacionesHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        /// <summary>
        /// Metodo para encriptar contraseña
        /// </summary>
        /// <param name="texto">La contraseña a encriptar</param>
        /// <returns>La contraseña encriptada</returns>
        public string Encriptar(string texto)
        {
            try
            {

                string key = "qualityinfosolutions"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception ex)
            {
                
            }

            return texto;
        }

        /// <summary>
        /// Metodo para desencriptar contraseña
        /// </summary>
        /// <param name="texto">La contraseña a desencriptar</param>
        /// <returns>La contraseña desencriptada</returns>
        public string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "qualityinfosolutions";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception ex)
            {
                
            }

            return textoEncriptado;
        }

        #region Consulta Parametrizaciones
        public ParametrizacionesModel ConsultarParametrizacion()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Parametrizacion/Consulta";
                    //var ruta = "http://localhost:53997/api/Parametrizacion/Consulta";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<ParametrizacionesModel>().Result;
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

        #region Agregar/Actualizar Parametrizaciones

        public async Task<bool> Registrar(ParametrizacionesModel pSettings)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Parametrizacion/Registra";
                   //var ruta = "http://localhost:53997/api/Parametrizacion/Registra";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pSettings).Result;
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

    }
}
