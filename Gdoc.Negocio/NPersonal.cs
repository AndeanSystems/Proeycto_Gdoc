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
    public class NPersonal : IDisposable
    {
        private DPersonal dPersonal = new DPersonal();
        public void Dispose()
        {
            dPersonal = null;
        }

        public Personal GrabarPersonal(Personal personal)
        {
            try
            {
                return dPersonal.GrabarPersonal(personal);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Personal EditarPersonal(Personal personal)
        {
            try
            {
                return dPersonal.EditarPersonal(personal);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Personal> ListarPersonal()
        {
            try
            {
                return dPersonal.ListarPersonal();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
