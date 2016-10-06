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
        public Sede GrabarSede(Sede sede)
        {
            try
            {
                return dSede.GrabarSede(sede);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Sede EliminarSede(Sede sede)
        {
            try
            {
                return dSede.EliminarSede(sede);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Sede EditarSede(Sede sede)
        {
            try
            {
                return dSede.EditarSede(sede);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
