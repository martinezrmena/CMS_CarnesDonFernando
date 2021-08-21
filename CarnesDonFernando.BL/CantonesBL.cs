using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class CantonesBL
    {
        private readonly CantonesDAL _cantonesDal = new CantonesDAL();

        #region Consultas

        public async Task<List<CantonesEntity>> ConsultarCantones()
        {
            try
            {
                return await _cantonesDal.GetCantones();
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<List<CantonesEntity>> ConsultarCantonPorProvincia(string pProvincia)
        {
            try
            {
                return await _cantonesDal.GetCantonesPorProvincia(pProvincia);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion
    }
}
