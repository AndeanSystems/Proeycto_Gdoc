using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Negocio
{
    public class NEmpresa : IDisposable
    {
        private DEmpresa dEmpresa = new DEmpresa();
        public void Dispose()
        {
            dEmpresa = null;
        }
        public List<Empresa> ListarEmpresa()
        {
            try
            {
                return dEmpresa.ListarEmpresa();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Empresa GrabarEmpresa(Empresa empresa)
        {
            try
            {
                return dEmpresa.GrabarEmpresa(empresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Empresa EliminarEmpresa(Empresa empresa)
        {
            try
            {
                return dEmpresa.EliminarEmpresa(empresa);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
