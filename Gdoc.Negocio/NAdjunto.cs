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
        public Int32 GrabarAdjunto(List<Adjunto> listAdjunto)
        {
            try
            {
                dAdjunto.GrabarAdjunto(listAdjunto);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
