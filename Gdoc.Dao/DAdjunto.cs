using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DAdjunto
    {
        public Int32 GrabarAdjunto(List<Adjunto> listAdjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Adjuntoes.AddRange(listAdjunto);
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
