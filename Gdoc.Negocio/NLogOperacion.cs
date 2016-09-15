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
    public class NLogOperacion : IDisposable
    {
        private DLogOperacion dLogOperacion = new DLogOperacion();
        public void Dispose()
        {
            dLogOperacion = null;
        }
        public LogOperacion GrabarLogOperacion(LogOperacion logoperacion)
        {
            try
            {
                return dLogOperacion.GrabarLogOperacion(logoperacion);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
