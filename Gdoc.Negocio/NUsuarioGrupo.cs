using Gdoc.Dao;
using Gdoc.Entity.Extension;
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
            //throw new NotImplementedException();
            dUsuarioGrupo = null;
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
        public List<EUsuarioGrupo2> listarUsuarioG()
        {
            try
            {
                return dUsuarioGrupo.listarUsuarioG();
            }
            catch (Exception)
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
        public short EditarUsuarioGrupo(UsuarioGrupo eUsuarioGrupo)
        {
            try
            {
                dUsuarioGrupo.Editar(eUsuarioGrupo);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
