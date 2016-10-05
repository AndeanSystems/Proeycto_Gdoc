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
        public List<EDocumentoAdjunto> ListarDocumentoAdjunto()
        {
            var listDocumentoAdjunto = new List<EDocumentoAdjunto>();
            try
            {
                using (var db = new DataBaseContext())
                {

                    var list2 = (from docadjunto in db.DocumentoAdjuntoes
                                    join adjunto in db.Adjuntoes
                                    on docadjunto.IDAdjunto equals adjunto.IDAdjunto

                                    join operacion in db.Operacions
                                    on docadjunto.IDOperacion equals operacion.IDOperacion

                                    select new { docadjunto, adjunto, operacion }).ToList();


                    list2.ForEach(x => listDocumentoAdjunto.Add(new EDocumentoAdjunto
                    {
                        IDDocumentoAdjunto = x.docadjunto.IDDocumentoAdjunto,
                        IDOperacion = x.docadjunto.IDOperacion,
                        IDAdjunto = x.docadjunto.IDAdjunto,
                        IDComentarioMesaVirtual = x.docadjunto.IDComentarioMesaVirtual,
                        EstadoDoctoAdjunto = x.docadjunto.EstadoDoctoAdjunto,

                        Adjunto = new Adjunto { 
                            NombreOriginal=x.adjunto.NombreOriginal,
                            TipoArchivo=x.adjunto.TipoArchivo,
                            RutaArchivo=x.adjunto.RutaArchivo,
                            EstadoAdjunto=x.adjunto.EstadoAdjunto,
                        },
                        //NombreOriginal = x.adjunto.NombreOriginal,

                        Archivo = string.Format(@"{0}_{1}", x.operacion.NumeroOperacion, x.adjunto.NombreOriginal),
                    }));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listDocumentoAdjunto;
            
        }
        public short GrabarDocumentoAdjunto(DocumentoAdjunto DocumentoAdjunto)
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
        public short EditarDocumentoAdjunto(DocumentoAdjunto documentoAdjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var docad=db.DocumentoAdjuntoes.Find(documentoAdjunto.IDDocumentoAdjunto);
                    docad.EstadoDoctoAdjunto = documentoAdjunto.EstadoDoctoAdjunto;
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
