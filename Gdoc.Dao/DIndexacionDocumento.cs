using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DIndexacionDocumento
    {
        public IndexacionDocumento GrabarIndexacion(IndexacionDocumento indexacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.IndexacionDocumentoes.Add(indexacion);
                    db.SaveChanges();
                }
                return indexacion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
