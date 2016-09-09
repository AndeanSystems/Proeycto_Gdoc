using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;


namespace Gdoc.Negocio
{
    public class NGeneral : IDisposable
    {
        private DGeneral dGeneral = new DGeneral();
        public void Dispose()
        {
            dGeneral = null;
        }
        public List<General> ListarGeneralParametros()
        {
            try
            {
                return dGeneral.ListarGeneralParametros();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public General EditarGeneralParametros(General general)
        {
            try
            {
                return dGeneral.EditarGeneralParametros(general);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public General GrabarGeneralParametros(General general)
        {
            try
            {
                return dGeneral.GrabarGeneralParametros(general);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public General CargaParametros(int IDEmpresa)
        {
            try
            {
                return dGeneral.CargaParametros(IDEmpresa);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
