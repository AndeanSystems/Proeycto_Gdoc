using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DOperacion
    {
        public short Grabar(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Operacions.Add(operacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<EOperacion> ListarOperacion()
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db= new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join documentodigital in db.DocumentoDigitalOperacions
                                 on operacion.IDOperacion equals documentodigital.IDOperacion

                                 join usuariopart in db.UsuarioParticipantes
                                 on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001")

                                 select new { operacion, tipodocumento, documentodigital, usuariopart, estado }).ToList();

                    list2.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.operacion.IDOperacion,
                        IDEmpresa = x.operacion.IDEmpresa,
                        TipoOperacion = x.operacion.TipoOperacion,
                        FechaEmision = x.operacion.FechaEmision,
                        NumeroOperacion = x.operacion.NumeroOperacion,
                        TituloOperacion = x.operacion.TituloOperacion,
                        AccesoOperacion = x.operacion.AccesoOperacion,
                        EstadoOperacion = x.operacion.EstadoOperacion,
                        DescripcionOperacion = x.operacion.DescripcionOperacion,
                        PrioridadOperacion = x.operacion.PrioridadOperacion,
                        FechaCierre = x.operacion.FechaCierre,
                        FechaRegistro = x.operacion.FechaRegistro,
                        FechaEnvio = x.operacion.FechaEnvio,
                        FechaVigente = x.operacion.FechaVigente,
                        DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        TipoComunicacion = x.operacion.TipoComunicacion,
                        NotificacionOperacion = x.operacion.NotificacionOperacion,
                        TipoDocumento = x.operacion.TipoDocumento,

                        DocumentoDigitalOperacion=new DocumentoDigitalOperacion{
                            Comentario=x.documentodigital.Comentario,
                        },


                        TipoDoc = new Concepto { DescripcionConcepto=x.tipodocumento.DescripcionConcepto},
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                    }));

                    
                }
                
            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public Operacion EditarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Operacions.Find(operacion.IDOperacion);
                    db.SaveChanges();
                }
                return operacion;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Operacion EliminarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var ope = db.Operacions.Find(operacion.IDOperacion);
                    ope.EstadoOperacion = operacion.EstadoOperacion;
                    db.SaveChanges();
                }
                return operacion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
