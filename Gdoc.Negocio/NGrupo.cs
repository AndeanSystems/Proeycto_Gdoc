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
    public class NGrupo : IDisposable
    {
        private DGrupo dGrupo = new DGrupo();
        public void Dispose()
        {
            dGrupo = null;
        }
        public List<EGrupo> ListarGrupo()
        {
            try
            {
                return dGrupo.ListarGrupo();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarGrupoUsuarios(Grupo grupo)
        {
            try
            {
                dGrupo.GrabarGrupoUsuarios(grupo);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Grupo EditarGrupo(Grupo grupo)
        {
            try
            {
                return dGrupo.EditarGrupo(grupo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Grupo EliminarGrupo(Grupo grupo)
        {
            try
            {
                return dGrupo.EliminarGrupo(grupo);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
