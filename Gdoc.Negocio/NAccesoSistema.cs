using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
namespace Gdoc.Negocio
{
    public class NAccesoSistema : IDisposable
    {
        private DAccesoSistema dAccesoSistema = new DAccesoSistema();
        public void Dispose()
        {
            dAccesoSistema = null;
        }
        public List<EAccesoSistema> ListarAccesoSistema()
        {
            try
            {
                return dAccesoSistema.ListarAccesoSistema();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<AccesoSistema> ListarAccesos()
        {
            try
            {
                return dAccesoSistema.ListarAccesos();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AccesoSistema CambiarEstadoAcceso(AccesoSistema accesosistema)
        {
            try
            {
                return dAccesoSistema.CambiarEstadoAcceso(accesosistema);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public AccesoSistema grabarAcceso(AccesoSistema accesosistema)
        {
            try
            {
                return dAccesoSistema.grabarAcceso(accesosistema);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
