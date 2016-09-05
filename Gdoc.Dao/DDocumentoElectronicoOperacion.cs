using Gdoc.Entity.Models;
using System;

namespace Gdoc.Dao
{
    public class DDocumentoElectronicoOperacion
    {
        public Int32 Grabar(DocumentoElectronicoOperacion eDocumentoElectronicoOperacion) {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.DocumentoElectronicoOperacions.Add(eDocumentoElectronicoOperacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
