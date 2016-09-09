using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DDocumentoDigitalOperacion
    {
        public Int32 GrabarDocumentoDigitalOperacion(DocumentoDigitalOperacion documentodigitaloperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.DocumentoDigitalOperacions.Add(documentodigitaloperacion);
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
