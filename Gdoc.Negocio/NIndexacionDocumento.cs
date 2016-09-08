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
    public class NIndexacionDocumento : IDisposable
    {
        private DIndexacionDocumento dIndexacion = new DIndexacionDocumento();
        public void Dispose()
        {
            dIndexacion = null;
        }
        public IndexacionDocumento GrabarIndexacion(IndexacionDocumento indexacion)
        {
            try
            {
                return dIndexacion.GrabarIndexacion(indexacion);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
