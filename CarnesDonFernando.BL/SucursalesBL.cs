using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class SucursalesBL
    {

        private readonly SucursalesDAL _sucursalDal = new SucursalesDAL();

        #region Consultas

        public async Task<List<SucursalEntity>> ConsultarSucursales()
        {
            try
            {
                return await _sucursalDal.GetSucursal();
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<List<SucursalesEntity>> ConsultarDatosSucursales()
        {
            try
            {
                return await _sucursalDal.GetSucursalesData();
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

        #region Consultar Sucursales por Llaves

        public async Task<SucursalesEntity> ConsultarDatoSucursal(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _sucursalDal.GetSucursalData(pLlaveParticion, pLlaveFila);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Actualizar/Ingresar Promociones

        public async Task<bool> GuardarSucursales(SucursalesEntity Sucursal)
        {
            try
            {
                return await _sucursalDal.SaveSucursal(Sucursal);
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<bool> GuardarSucursalEntity(SucursalEntity Sucursal)
        {
            try
            {
                return await _sucursalDal.SaveSucursalEntity(Sucursal);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Promociones

        public async Task<bool> EliminarSucursales(SucursalesEntity Sucursal)
        {
            try
            {
                return await _sucursalDal.DeleteSucursal(Sucursal);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
