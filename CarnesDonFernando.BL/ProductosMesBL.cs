using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class ProductosMesBL
    {
        private readonly ProductosMesDAL _productosDal = new ProductosMesDAL();


        #region Consultar Todos los Productos del Mes
        public async Task<List<ProductosMesEntity>> ConsultarTodosProductos()
        {
            try
            {
                return await _productosDal.GetProducts();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar producto del mes por Llaves

        public async Task<ProductosMesEntity> ConsultarProducto(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _productosDal.GetProduct(pLlaveParticion, pLlaveFila);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar si existe producto
        public async Task<ProductosMesEntity> ConsultarSiExiste(string titulo, string detalle, string resumen, string preparacion, string fechaInicio, string fechaFin)
        {
            try
            {
                return await _productosDal.ExistProduct(titulo,detalle,resumen,preparacion,fechaInicio,fechaFin);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

        #region Actualizar/Ingresar Productos del Mes

        public async Task<bool> GuardarProductos(ProductosMesEntity Producto)
        {
            try
            {
                return await _productosDal.SaveProduct(Producto);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Promociones

        public async Task<bool> EliminarProducto(ProductosMesEntity Producto)
        {
            try
            {
                return await _productosDal.DeleteProduct(Producto);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
