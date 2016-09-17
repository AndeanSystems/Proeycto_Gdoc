using Gdoc.Dao;
using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Negocio
{
    public class NModuloPaginaUrl:IDisposable
    {
        #region "Variable"
        protected DModuloPaginaUrl dModuloPaginaUrl = new DModuloPaginaUrl();
        #endregion

        public List<EModuloPaginaUrl> ObtenerPaginaModuloUrl(Usuario eUsuario) {
            try
            {
                return dModuloPaginaUrl.ObtenerPaginaModuloUrl(eUsuario);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public void Dispose()
        {
            dModuloPaginaUrl = null;
        }
    }
}
