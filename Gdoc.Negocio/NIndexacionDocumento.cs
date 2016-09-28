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

        public List<IndexacionDocumento> ListarIndexacionDocumento()
        {
            try
            {
                return dIndexacion.ListarIndexacionDocumento();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public short GrabarIndexacion(List<IndexacionDocumento> listIndexacion)
        {
            try
            {
                return dIndexacion.GrabarIndexacion(listIndexacion);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
