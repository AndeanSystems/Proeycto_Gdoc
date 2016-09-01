using Gdoc.Dao;
using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Negocio
{
    public class NUbigeo : IDisposable
    {
        private DUbigeo dUbigeo = new DUbigeo();
        public void Dispose()
        {
            dUbigeo = null;
        }
        public List<Ubigeo> ListarUbigeo()
        {
            try
            {
                return dUbigeo.ListarUbigeo();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
