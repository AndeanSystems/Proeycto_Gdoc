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
    public class NUsuarioParticipante : IDisposable
    {
        private DUsuarioParticipante dUsuarioParticipante = new DUsuarioParticipante();
        public void Dispose()
        {
            dUsuarioParticipante = null;
        }
        public List<UsuarioParticipante> ListarUsuarioParticipante()
        {
            try
            {
                return dUsuarioParticipante.ListarUsuarioParticipante();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
