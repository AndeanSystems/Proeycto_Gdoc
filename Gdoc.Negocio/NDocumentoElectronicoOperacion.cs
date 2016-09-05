using Gdoc.Dao;
using Gdoc.Entity.Models;
using System;

namespace Gdoc.Negocio
{
    public class NDocumentoElectronicoOperacion:IDisposable
    {
        #region "Variables"
        private DDocumentoElectronicoOperacion dDocumentoElectronicoOperacion = new DDocumentoElectronicoOperacion();

        public void Dispose()
        {
            dDocumentoElectronicoOperacion = null;
        }
        #endregion
        public Int32 Grabar(DocumentoElectronicoOperacion eDocumentoElectronicoOperacion)
        {
            try
            {
                return dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
