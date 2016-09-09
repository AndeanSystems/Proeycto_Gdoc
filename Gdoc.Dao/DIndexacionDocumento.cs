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
        public short GrabarIndexacion(List<IndexacionDocumento> listIndexacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.IndexacionDocumentoes.AddRange(listIndexacion);
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
