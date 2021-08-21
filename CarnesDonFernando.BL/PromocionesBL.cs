using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class PromocionesBL
    {
        private readonly PromocionesDAL _promocionesDal = new PromocionesDAL();

        #region Consultar Todas Promociones
        public async Task<List<PromocionesEntity>> ConsultarTodasPromociones()
        {
            try
            {
                return await _promocionesDal.GetPromotions();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar por Llaves

        public async Task<PromocionesEntity> ConsultarPromocion(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _promocionesDal.GetPromotion(pLlaveParticion, pLlaveFila);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar si existe promocion
        public async Task<PromocionesEntity> ConsultarSiExiste(string pTitulo, string pEnlace, string pFechaIni, string pFechaFin)
        {
            try
            {
                return await _promocionesDal.ExistPromotion(pTitulo,pEnlace,pFechaIni,pFechaFin);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

        #region Actualizar/Ingresar Promociones

        public async Task<bool> GuardarPromociones(PromocionesEntity Promocion)
        {
            try
            {
                return await _promocionesDal.SavePromotion(Promocion);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Promociones

        public async Task<bool> EliminarPromociones(PromocionesEntity Promocion)
        {
            try
            {
                return await _promocionesDal.DeletePromotion(Promocion);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion
    }
}
