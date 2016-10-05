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
    public class NAdjunto : IDisposable
    {
        private DAdjunto dAdjunto = new DAdjunto();
        public void Dispose()
        {
            dAdjunto = null;
        }
        public List<EAdjunto> ListarAdjunto()
        {
            try
            {
                return dAdjunto.ListarAdjunto();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Int32 GrabarAdjunto(Adjunto Adjunto)
        {
            try
            {
                dAdjunto.GrabarAdjunto(Adjunto);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
