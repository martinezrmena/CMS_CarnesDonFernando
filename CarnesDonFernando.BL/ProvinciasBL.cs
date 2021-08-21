using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class ProvinciasBL
    {

        private readonly ProvinciasDAL _provinciasDal = new ProvinciasDAL();

        #region Consultas

        public async Task<List<ProvinciasEntity>> ConsultarProvincias()
        {
            try
            {
                return await _provinciasDal.GetProvincias();
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
