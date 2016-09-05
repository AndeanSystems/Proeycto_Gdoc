using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Negocio
{
    public class NConcepto : IDisposable
    {
        private DConcepto dConcepto = new DConcepto();
        public void Dispose()
        {
            dConcepto = null;
        }

        public List<Concepto> ListarConcepto()
        {
            try
            {
                return dConcepto.ListarConcepto();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Concepto GrabarConcepto(Concepto concepto)
        {
            try
            {
                return dConcepto.GrabarConcepto(concepto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Concepto EditarConcepto(Concepto concepto)
        {
            try
            {
                return dConcepto.EditarConcepto(concepto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Concepto EliminarConcepto(Concepto concepto)
        {
            try
            {
                return dConcepto.EliminarConcepto(concepto);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
