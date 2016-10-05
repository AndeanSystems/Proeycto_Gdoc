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
        public List<EAdjunto> ListarAdjunto()
        {
            var listAdjunto = new List<EAdjunto>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Adjuntoes.ToList();

                    var list2 = (from adjunto in db.Adjuntoes
                                 join dadjunto in db.DocumentoAdjuntoes
                                 on adjunto.IDAdjunto equals dadjunto.IDAdjunto

                                 join operacion in db.Operacions
                                    on dadjunto.IDOperacion equals operacion.IDOperacion

                                 select new { dadjunto, adjunto, operacion }).ToList();

                    list2.ForEach(x => listAdjunto.Add(new EAdjunto
                    {
                        IDAdjunto=x.adjunto.IDAdjunto,
                        IDUsuario = x.adjunto.IDUsuario,
                        NombreOriginal = x.adjunto.NombreOriginal,
                        RutaArchivo = x.adjunto.RutaArchivo,
                        TamanoArchivo = x.adjunto.TamanoArchivo,
                        FechaRegistro = x.adjunto.FechaRegistro,
                        EstadoAdjunto = x.adjunto.EstadoAdjunto,
                        TipoArchivo = x.adjunto.TipoArchivo,

                        DocumentoAdjunto = new DocumentoAdjunto
                        {
                            IDOperacion=x.dadjunto.IDOperacion,
                            IDComentarioMesaVirtual=x.dadjunto.IDComentarioMesaVirtual,
                            EstadoDoctoAdjunto=x.dadjunto.EstadoDoctoAdjunto,
                        },
                        
                        Archivo = string.Format(@"{0}_{1}", x.operacion.NumeroOperacion, x.adjunto.NombreOriginal),
                    }));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listAdjunto;

        }
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
        public short EditarAdjunto(Adjunto adjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var docad = db.Adjuntoes.Find(adjunto.IDAdjunto);
                    docad.EstadoAdjunto = adjunto.EstadoAdjunto;
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
