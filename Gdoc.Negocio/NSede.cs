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
    public class NSede : IDisposable
    {
        private DSede dSede = new DSede();
        public void Dispose()
        {
            dSede = null;
        }
        public List<Sede> ListarSede()
        {
            try
            {
                return dSede.ListarSede();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
