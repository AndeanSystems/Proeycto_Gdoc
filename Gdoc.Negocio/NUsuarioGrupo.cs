using Gdoc.Dao;
using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;

namespace Gdoc.Negocio
{
    public class NUsuarioGrupo: IDisposable
    {
        #region "Variables"
        private DUsuarioGrupo dUsuarioGrupo = new DUsuarioGrupo();
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<UsuarioGrupo> listarUsuarioGrupo(UsuarioGrupo eUsuarioGrupo) {
            try
            {
                return dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        public short GrabarUsuarioGrupo(List<UsuarioGrupo> eUsuarioGrupo)
        {
            try
            {
                dUsuarioGrupo.GrabarUsuarioGrupo(eUsuarioGrupo);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
