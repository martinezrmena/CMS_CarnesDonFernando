using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
   public class AcercaDeBL
    {
        private readonly AcercaDeDAL _acercaDeDal = new AcercaDeDAL();

        #region Consultar Acerca De
        public async Task<AcercaDeEntity> ConsultarAcercaDe()
        {
            try
            {
                return await _acercaDeDal.GetAbout();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar Acerca De por Llaves

        public async Task<AcercaDeEntity> ConsultarAcercaDeLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _acercaDeDal.GetAboutKeys(pLlaveParticion, pLlaveFila);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Actualizar/Ingresar Acerca De

        public async Task<bool> GuardarAcercaDe(AcercaDeEntity About)
        {
            try
            {
                return await _acercaDeDal.SaveAbout(About);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Acerca De

        public async Task<bool> EliminarAcercaDe(AcercaDeEntity About)
        {
            try
            {
                return await _acercaDeDal.DeleteAbout(About);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
