using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class DistritoBL
    {
        private readonly DistritoDAL _distritosDal = new DistritoDAL();

        #region Consultas

        public async Task<List<DistritoEntity>> ConsultarDistritos()
        {
            try
            {
                return await _distritosDal.GetDistritos();
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<List<DistritoEntity>> ConsultarCantonPorProvincia(string pCanton)
        {
            try
            {
                return await _distritosDal.GetDistritosPorCanton(pCanton);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

        #region Insertar
        public async Task<bool> InsertarDistrito(DistritoEntity pDistrito)
        {
            try
            {
                return await _distritosDal.SaveDistrito(pDistrito);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion
    }
}
