using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
namespace Gdoc.Negocio
{
    public class NGrupo : IDisposable
    {
        private DGrupo dGrupo = new DGrupo();
        public void Dispose()
        {
            dGrupo = null;
        }
        public List<Grupo> ListarGrupo()
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

    }
}
