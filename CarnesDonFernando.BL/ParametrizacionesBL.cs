using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class ParametrizacionesBL
    {
        private readonly ParametrizacionesDAL _parametrizacionesDal = new ParametrizacionesDAL();

        #region Consultar Parametrizaciones
        public async Task<ParametrizacionesEntity> ConsultarParametrizaciones()
        {
            try
            {
                return await _parametrizacionesDal.GetSettings();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Actualizar/Ingresar Parametrizaciones

        public async Task<bool> GuardarParametrizacion(ParametrizacionesEntity Settings)
        {
            try
            {
                return await _parametrizacionesDal.SaveSettings(Settings);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

    }
}
