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
        public List<DocumentoDigitalOperacion> ListarDocumentoDigitalOperacion()
        {
            var listDocumentoDigitalOperacion = new List<DocumentoDigitalOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.DocumentoDigitalOperacions.ToList();

                    var list2 = (from adjunto in db.Adjuntoes
                                 join dadjunto in db.DocumentoAdjuntoes
                                 on adjunto.IDAdjunto equals dadjunto.IDAdjunto


                                 select new { dadjunto, adjunto }).ToList();

                    list.ForEach(x => listDocumentoDigitalOperacion.Add(new DocumentoDigitalOperacion
                    {
                        IDDoctoDigitalOperacion=x.IDDoctoDigitalOperacion,
                        IDOperacion=x.IDOperacion,
                        DerivarDocto=x.DerivarDocto,
                        NombreOriginal=x.NombreOriginal,
                        RutaFisica=x.RutaFisica,
                        TamanoDocto=x.TamanoDocto,
                        NombreFisico = x.NombreFisico,
                        TipoArchivo=x.TipoArchivo,
                    }));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listDocumentoDigitalOperacion;

        }
        public short GrabarDocumentoDigitalOperacion(List<DocumentoDigitalOperacion> listDocumentodigitaloperacion)
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
        public short EditarDocumentoDigitalOperacion(DocumentoDigitalOperacion documentodigitaloperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var docdigope=db.DocumentoDigitalOperacions.Find(documentodigitaloperacion.IDDoctoDigitalOperacion);
                    docdigope.NombreOriginal = documentodigitaloperacion.NombreOriginal;
                    docdigope.RutaFisica = documentodigitaloperacion.RutaFisica;
                    docdigope.TamanoDocto = documentodigitaloperacion.TamanoDocto;
                    docdigope.NombreFisico = documentodigitaloperacion.NombreFisico;
                    docdigope.TipoArchivo = documentodigitaloperacion.TipoArchivo;

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
