using Gdoc.Dao;
using Gdoc.Entity.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Negocio
{
    public class NEUsuarioGrupo : IDisposable
    {
        private DEUsuarioGrupo dEUsuarioGrupo = new DEUsuarioGrupo();
        public void Dispose()
        {
            dEUsuarioGrupo = null;
        }
        public List<EUsuarioGrupo> ObtenerUsuarioGrupo(EUsuarioGrupo eUsuarioGrupo)
        {
            try
            {
                return dEUsuarioGrupo.ObtenerUsuarioGrupo(eUsuarioGrupo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
