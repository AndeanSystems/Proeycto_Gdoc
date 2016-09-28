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
        public short GrabarAdjunto(Adjunto Adjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Adjuntoes.Add(Adjunto);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public short AnularAdjunto(Adjunto adjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var docad = db.DocumentoAdjuntoes.Find(adjunto.IDAdjunto);
                    docad.EstadoDoctoAdjunto = adjunto.EstadoAdjunto;
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
