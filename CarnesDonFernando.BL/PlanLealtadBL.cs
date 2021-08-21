using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.BL
{
    public class PlanLealtadBL
    {
        private readonly PlanLealtadDAL  _planDAL = new PlanLealtadDAL();

        #region Consultar Todos los planes de lealtad
        public async Task<List<PlanLealtadEntity>> ConsultarTodosPlanesLealtad()
        {
            try
            {
                return await _planDAL.GetPlans();
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Consultar planes de lealtad por Llaves

        public async Task<PlanLealtadEntity> ConsultarPlanLealtad(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                return await _planDAL.GetPlan(pLlaveParticion, pLlaveFila);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Actualizar/Ingresar Planes de Lealtad

        public async Task<bool> GuardarPlanLealtad(PlanLealtadEntity Plan)
        {
            try
            {
                return await _planDAL.SavePlan(Plan);
            }
            catch
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Eliminar Planes de Lealtad

        public async Task<bool> EliminarPlanLealtad(PlanLealtadEntity Plan)
        {
            try
            {
                return await _planDAL.DeletePlan(Plan);
            }
            catch
            {
                throw new Exception("");
            }
        }

        #endregion

    }
}
