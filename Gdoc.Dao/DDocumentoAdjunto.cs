using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DDocumentoAdjunto
    {
        public Int32 GrabarDocumentoAdjunto(DocumentoAdjunto DocumentoAdjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.DocumentoAdjuntoes.Add(DocumentoAdjunto);
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
