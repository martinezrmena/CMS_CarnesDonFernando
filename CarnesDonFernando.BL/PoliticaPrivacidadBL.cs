using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class PoliticaPrivacidadBL
    {
        private readonly PoliticaPrivacidadDAL _politicaDAL = new PoliticaPrivacidadDAL();


        #region Consultar Politica de Privacidad
        public async Task<PoliticaPrivacidadEntity> ConsultarPoliticaPrivacidad()
        {
            try
            {
                return await _politicaDAL.GetPrivacyPolicy();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

    }
}
