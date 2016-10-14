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
        public List<EUsuarioParticipante> ListarUsuarioParticipante()
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
        public short Grabar(List<UsuarioParticipante> listUsuarioParticipante)
        {
            try
            {
                dUsuarioParticipante.Grabar(listUsuarioParticipante);
                return 1;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
