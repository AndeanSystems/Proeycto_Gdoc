using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using Gdoc.Common.Utilitario;

namespace Gdoc.Dao
{
    public class DOperacion
    {
        public List<EOperacion> ListarOperacionBusqueda()
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 //join documentodigital in db.DocumentoDigitalOperacions
                                 //on operacion.IDOperacion equals documentodigital.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        tipooperacion.TipoConcepto.Equals("003") &&
                                        prioridad.TipoConcepto.Equals("005")

                                 select new { operacion, tipodocumento, /*documentodigital, usuariopart,*/ estado, tipooperacion, prioridad }).ToList();

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

                        //DocumentoDigitalOperacion=new DocumentoDigitalOperacion{
                        //    Comentario=x.documentodigital.Comentario,
                        //},

                        TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarDocumentosRecibidos(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        tipooperacion.TipoConcepto.Equals("003") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)) > 0)

                                 select new { operacion, tipodocumento, estado, tipooperacion, prioridad }).ToList();

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

                        TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
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
        public List<EOperacion> ListarOperacionDigital(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db= new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 //join documentodigital in db.DocumentoDigitalOperacions
                                 //on operacion.IDOperacion equals documentodigital.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && x.TipoParticipante == Constantes.TipoParticipante.RemitenteDE) > 0)

                                 select new { operacion, tipodocumento, /*documentodigital, usuariopart,*/ estado }).ToList();

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

                        //DocumentoDigitalOperacion=new DocumentoDigitalOperacion{
                        //    Comentario=x.documentodigital.Comentario,
                        //},


                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
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
        public List<EOperacion> ListarOperacionElectronico(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    //var query1 = db.UsuarioParticipantes.Include("Operacion")
                    //                .Where(x=>x.IDUsuario == eUsuarioParticipante.IDUsuario).ToList();
                    //var operacionsert = db.Operacions.ToList();

                    //var list = db.Operacions.ToList();

                    var list = (from operacion in db.Operacions

                                 join documentoelectronico in db.DocumentoElectronicoOperacions
                                 on operacion.IDOperacion equals documentoelectronico.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 //join usuario in db.Usuarios
                                 //on usuariopart.IDUsuario equals usuario.IDUsuario

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && x.TipoParticipante == Constantes.TipoParticipante.RemitenteDE ) > 0)
                                        
                                 select new { operacion, tipodocumento, documentoelectronico, /*usuariopart,*/ estado /*,usuario*/ }).ToList();

                    list.ForEach(x => listOperacion.Add(new EOperacion
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

                        DocumentoElectronicoOperacion = new DocumentoElectronicoOperacion
                        {
                            Memo = x.documentoelectronico.Memo,
                        },
                        
                        //UsuarioParticipante = new UsuarioParticipante
                        //{
                        //    IDUsuarioParticipante=x.usuariopart.IDUsuarioParticipante,
                        //    IDUsuario=x.usuariopart.IDUsuario,
                        //    TipoParticipante=x.usuariopart.TipoParticipante,
                        //},

                        //Usuario=new Usuario{
                        //    NombreUsuario=x.usuario.NombreUsuario,
                        //},
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
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
        public List<EOperacion> ListarMesaVirtual(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Operacions.ToList();

                    var list2 = (from operacion in db.Operacions

                                 join tipomesa in db.Conceptoes
                                 on operacion.TipoDocumento equals tipomesa.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 where tipomesa.TipoConcepto.Equals("011") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) > 0)

                                 select new { operacion, tipomesa,  /*usuariopart,*/ estado /*,usuario*/, prioridad }).ToList();

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

                        TipoDoc = new Concepto { DescripcionCorta = x.tipomesa.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public short EditarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Operacions.Find(operacion.IDOperacion);
                    entidad.TituloOperacion = operacion.TituloOperacion;
                    entidad.AccesoOperacion = operacion.AccesoOperacion;
                    entidad.DescripcionOperacion = operacion.DescripcionOperacion;
                    entidad.PrioridadOperacion = operacion.PrioridadOperacion;
                    entidad.TipoComunicacion = operacion.TipoComunicacion;
                    entidad.TipoDocumento = operacion.TipoDocumento;
                    entidad.FechaEnvio = operacion.FechaEnvio;
                    entidad.FechaVigente = operacion.FechaVigente;
                    db.SaveChanges();
                }
                return 1;
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
