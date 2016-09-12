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
        public Int32 GrabarDocumentoDigitalOperacion(List<DocumentoDigitalOperacion> listDocumentodigitaloperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.DocumentoDigitalOperacions.AddRange(listDocumentodigitaloperacion);
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
