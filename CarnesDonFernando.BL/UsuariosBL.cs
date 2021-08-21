using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class UsuariosBL
    {
        private readonly UsuariosDAL _usuariosDal = new UsuariosDAL();

        #region Consultas

        public async Task<List<UsuariosEntity>> ConsultarUsuarios()
        {
            try
            {
                return await _usuariosDal.GetUsers();
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<UsuariosEntity> ConsultarUsuario(string pEmail, string pContrasenna)
        {
            return await _usuariosDal.GetUser(pEmail, pContrasenna);
        }

        public async Task<UsuariosEntity> ConsultarUsuarioXCedula(string pCedula)
        {
            return await _usuariosDal.GetUser(pCedula);
        }

        public async Task<UsuariosEntity> ConsultarUsuarioXCorreo(string pCorreo)
        {
            return await _usuariosDal.GetUserMail(pCorreo);
        }

        public async Task<List<UsuariosEntity>> ConsultarUsuarioSync(bool psincronizado)
        {
            return await _usuariosDal.GetUsersSync(psincronizado);
        }

        #endregion

        #region Ingresar/Actualizar

        public async Task<bool> GuardarUsuarios(UsuariosEntity Usuario)
        {
            try
            {
                return await _usuariosDal.SaveUser(Usuario);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

        #region Eliminar

        public async Task<bool> EliminarUsuarios(UsuariosEntity Usuario)
        {
            try
            {
                return await _usuariosDal.DeleteUser(Usuario);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
