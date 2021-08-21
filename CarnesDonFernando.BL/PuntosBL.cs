using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class PuntosBL
    {
        private readonly PuntosDAL _puntosDAL = new PuntosDAL();

        #region Consultar Puntos por Cedula
        public async Task<List<PuntosEntity>> ConsultarPuntosUsuario(string Cedula)
        {
            try
            {
                if (!string.IsNullOrEmpty(Cedula))
                {
                    return await _puntosDAL.GetPuntosUsuario(Cedula);
                }
                else 
                {
                    return new List<PuntosEntity>();
                }
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<List<PuntosEntity>> ConsultarPuntosGlobal()
        {
            try
            {
                return await _puntosDAL.GetPuntosGlobal();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Actualizar/Ingresar Puntos

        public async Task<bool> GuardarPuntos(PuntosEntity Puntos)
        {
            try
            {
                return await _puntosDAL.SavePuntos(Puntos);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Puntos

        public async Task<bool> EliminarPunto(PuntosEntity Puntos)
        {
            try
            {
                return await _puntosDAL.DeletePuntos(Puntos);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion
    }
}
