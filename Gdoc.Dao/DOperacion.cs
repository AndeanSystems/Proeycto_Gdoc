using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using Gdoc.Common.Utilitario;
using System.Data;

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
                        NombreFinal=x.operacion.NombreFinal,

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
        public List<EOperacion> ListarOperacionBusquedaTotal(EOperacion operacion,IndexacionDocumento indexacion)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                var tipoOperacion = new SqlParameter { ParameterName = "@tipoOper", Value = operacion.TipoOperacion };
                var tipoDocumento = new SqlParameter { ParameterName = "@tipoDocu", Value = operacion.TipoDocumento };
                var fechaDesde = new SqlParameter { ParameterName = "@fechaDesde", Value = operacion.FechaEmision };
                var fechaHasta = new SqlParameter { ParameterName = "@fechaHasta", Value = operacion.FechaRegistro };
                var descripcion = new SqlParameter { ParameterName = "@descripcion", Value = indexacion.DescripcionIndice };
                using (var db = new DataBaseContext())
                {
                    var listResult = db.Database.SqlQuery<EOperacion>("gdoc_BusquedaOperacion @tipoOper, @tipoDocu, @fechaDesde, @fechaHasta, @descripcion",
                        tipoOperacion, tipoDocumento, fechaDesde, fechaHasta, descripcion).ToList();

                    var list = (from ope in listResult
                               join tipodocumento in db.Conceptoes
                                 on ope.TipoDocumento equals tipodocumento.CodiConcepto

                               join estado in db.Conceptoes
                               on ope.EstadoOperacion.ToString() equals estado.CodiConcepto

                               join prioridad in db.Conceptoes
                               on ope.PrioridadOperacion equals prioridad.CodiConcepto

                               join tipooperacion in db.Conceptoes
                               on ope.TipoOperacion equals tipooperacion.CodiConcepto

                                join tipoacceso in db.Conceptoes
                                on ope.AccesoOperacion equals (tipoacceso.CodiConcepto+" ")

                               where tipodocumento.TipoConcepto.Equals("012") &&
                                      estado.TipoConcepto.Equals("001") &&
                                      tipooperacion.TipoConcepto.Equals("003") &&
                                      prioridad.TipoConcepto.Equals("005")
                                      && tipoacceso.TipoConcepto.Equals("002")

                                select new { ope, tipodocumento, estado, tipooperacion, prioridad, tipoacceso }).ToList();

                    list.ForEach(x => listOperacion.Add(new EOperacion
                    {
                        IDOperacion = x.ope.IDOperacion,
                        IDEmpresa = x.ope.IDEmpresa,
                        TipoOperacion = x.ope.TipoOperacion,
                        FechaEmision = x.ope.FechaEmision,
                        NumeroOperacion = x.ope.NumeroOperacion,
                        TituloOperacion = x.ope.TituloOperacion,
                        AccesoOperacion = x.ope.AccesoOperacion,
                        EstadoOperacion = x.ope.EstadoOperacion,
                        DescripcionOperacion = x.ope.DescripcionOperacion,
                        PrioridadOperacion = x.ope.PrioridadOperacion,
                        FechaCierre = x.ope.FechaCierre,
                        FechaRegistro = x.ope.FechaRegistro,
                        FechaEnvio = x.ope.FechaEnvio,
                        FechaVigente = x.ope.FechaVigente,
                        DocumentoAdjunto = x.ope.DocumentoAdjunto,
                        TipoComunicacion = x.ope.TipoComunicacion,
                        NotificacionOperacion = x.ope.NotificacionOperacion,
                        TipoDocumento = x.ope.TipoDocumento,
                        NombreFinal = x.ope.NombreFinal,
                        TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Acceso = new Concepto { DescripcionCorta = x.tipoacceso.DescripcionCorta },
                        Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                    }));
                }
                return listOperacion;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        public List<EOperacion> ListarDocumentosRecibidos(UsuarioParticipante eUsuarioParticipante)
        {
            var listOperacion = new List<EOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var remitentes = new List<String>();
                    var listremitentes = (from remitente in db.UsuarioParticipantes

                                          join usuario in db.Usuarios
                                          on remitente.IDUsuario equals usuario.IDUsuario

                                          join operacion in db.Operacions
                                          on remitente.IDOperacion equals operacion.IDOperacion

                                          where 

                                           (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario 
                                            && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)) > 0)
                                            && (remitente.TipoParticipante==Constantes.TipoParticipante.RemitenteDE || remitente.TipoParticipante==Constantes.TipoParticipante.EmisorDD)
                                          select new { usuario,operacion}).ToList();

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
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario 
                                            && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)) > 0)

                                 select new { operacion, tipodocumento, estado, tipooperacion, prioridad }).ToList();

                    
                    

                    foreach (var item in list2)
                    {
                        foreach (var x in listremitentes.Where(y => y.operacion.IDOperacion == item.operacion.IDOperacion))
                        {
                            remitentes.Add(x.usuario.NombreUsuario);
                        }
                        listOperacion.Add(new EOperacion
                        {
                            IDOperacion = item.operacion.IDOperacion,
                            IDEmpresa = item.operacion.IDEmpresa,
                            TipoOperacion = item.operacion.TipoOperacion,
                            FechaEmision = item.operacion.FechaEmision,
                            NumeroOperacion = item.operacion.NumeroOperacion,
                            TituloOperacion = item.operacion.TituloOperacion,
                            AccesoOperacion = item.operacion.AccesoOperacion,
                            EstadoOperacion = item.operacion.EstadoOperacion,
                            DescripcionOperacion = item.operacion.DescripcionOperacion,
                            PrioridadOperacion = item.operacion.PrioridadOperacion,
                            FechaCierre = item.operacion.FechaCierre,
                            FechaRegistro = item.operacion.FechaRegistro,
                            FechaEnvio = item.operacion.FechaEnvio,
                            FechaVigente = item.operacion.FechaVigente,
                            DocumentoAdjunto = item.operacion.DocumentoAdjunto,
                            TipoComunicacion = item.operacion.TipoComunicacion,
                            NotificacionOperacion = item.operacion.NotificacionOperacion,
                            TipoDocumento = item.operacion.TipoDocumento,
                            NombreFinal = item.operacion.NombreFinal,

                            TipoOpe = new Concepto { DescripcionCorta = item.tipooperacion.DescripcionCorta },
                            TipoDoc = new Concepto { DescripcionCorta = item.tipodocumento.DescripcionCorta },
                            Estado = new Concepto { DescripcionConcepto = item.estado.DescripcionConcepto },
                            Prioridad = new Concepto { DescripcionConcepto = item.prioridad.DescripcionConcepto },


                            Remitente = string.Join(",",remitentes.ToArray()),
                        });
                        remitentes = new List<String>();
                    }
                        //list2.ForEach(x => listOperacion.Add(new EOperacion
                        //{
                        //    IDOperacion = x.operacion.IDOperacion,
                        //    IDEmpresa = x.operacion.IDEmpresa,
                        //    TipoOperacion = x.operacion.TipoOperacion,
                        //    FechaEmision = x.operacion.FechaEmision,
                        //    NumeroOperacion = x.operacion.NumeroOperacion,
                        //    TituloOperacion = x.operacion.TituloOperacion,
                        //    AccesoOperacion = x.operacion.AccesoOperacion,
                        //    EstadoOperacion = x.operacion.EstadoOperacion,
                        //    DescripcionOperacion = x.operacion.DescripcionOperacion,
                        //    PrioridadOperacion = x.operacion.PrioridadOperacion,
                        //    FechaCierre = x.operacion.FechaCierre,
                        //    FechaRegistro = x.operacion.FechaRegistro,
                        //    FechaEnvio = x.operacion.FechaEnvio,
                        //    FechaVigente = x.operacion.FechaVigente,
                        //    DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                        //    TipoComunicacion = x.operacion.TipoComunicacion,
                        //    NotificacionOperacion = x.operacion.NotificacionOperacion,
                        //    TipoDocumento = x.operacion.TipoDocumento,
                        //    NombreFinal = x.operacion.NombreFinal,

                        //    TipoOpe = new Concepto { DescripcionCorta = x.tipooperacion.DescripcionCorta },
                        //    TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                        //    Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        //    Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },


                        //    Remitente = remitentes,

                        //}));
                    

                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
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

                                 join documentodigital in db.DocumentoDigitalOperacions
                                 on operacion.IDOperacion equals documentodigital.IDOperacion

                                 //join usuariopart in db.UsuarioParticipantes
                                 //on operacion.IDOperacion equals usuariopart.IDOperacion

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 join estado in db.Conceptoes
                                 on operacion.EstadoOperacion.ToString() equals estado.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && x.TipoParticipante == Constantes.TipoParticipante.EmisorDD) > 0)

                                 select new { operacion, tipodocumento, documentodigital,/* usuariopart,*/ estado }).ToList();

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
                        NombreFinal=x.operacion.NombreFinal,
                        DocumentoDigitalOperacion = new DocumentoDigitalOperacion
                        {
                            DerivarDocto = x.documentodigital.DerivarDocto,
                        },


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

                    var listremitente = (from remitente in db.UsuarioParticipantes

                                         join operacion in db.Operacions
                                         on remitente.IDOperacion equals operacion.IDOperacion

                                         join usuario in db.Usuarios
                                         on remitente.IDUsuario equals usuario.IDUsuario

                                         where remitente.TipoParticipante.Equals("04")
                                         select new { remitente, operacion, usuario }).ToList();

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

                                 join prioridad in db.Conceptoes
                                 on operacion.PrioridadOperacion equals prioridad.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && (x.TipoParticipante == Constantes.TipoParticipante.RemitenteDE || x.TipoParticipante == Constantes.TipoParticipante.EmisorDE)) > 0)
                                        
                                 select new { operacion, tipodocumento, documentoelectronico, /*usuariopart,*/ estado /*,usuario*/,prioridad }).ToList();


                    foreach (var x in list)
                    {
                        listOperacion.Add(new EOperacion
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
                            NombreFinal = x.operacion.NombreFinal,
                            DocumentoElectronicoOperacion = new DocumentoElectronicoOperacion
                            {
                                IDDoctoElectronicoOperacion = x.documentoelectronico.IDDoctoElectronicoOperacion,
                                IDOperacion = x.documentoelectronico.IDOperacion,
                                Memo = x.documentoelectronico.Memo,
                            },

                            TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                            Prioridad = new Concepto { DescripcionConcepto = x.prioridad.DescripcionConcepto },
                            Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },

                            Emisor= listremitente.FirstOrDefault().usuario.NombreUsuario,
                        });
                    }
                    //list.ForEach(x => listOperacion.Add(new EOperacion
                    //{
                    //    IDOperacion = x.operacion.IDOperacion,
                    //    IDEmpresa = x.operacion.IDEmpresa,
                    //    TipoOperacion = x.operacion.TipoOperacion,
                    //    FechaEmision = x.operacion.FechaEmision,
                    //    NumeroOperacion = x.operacion.NumeroOperacion,
                    //    TituloOperacion = x.operacion.TituloOperacion,
                    //    AccesoOperacion = x.operacion.AccesoOperacion,
                    //    EstadoOperacion = x.operacion.EstadoOperacion,
                    //    DescripcionOperacion = x.operacion.DescripcionOperacion,
                    //    PrioridadOperacion = x.operacion.PrioridadOperacion,
                    //    FechaCierre = x.operacion.FechaCierre,
                    //    FechaRegistro = x.operacion.FechaRegistro,
                    //    FechaEnvio = x.operacion.FechaEnvio,
                    //    FechaVigente = x.operacion.FechaVigente,
                    //    DocumentoAdjunto = x.operacion.DocumentoAdjunto,
                    //    TipoComunicacion = x.operacion.TipoComunicacion,
                    //    NotificacionOperacion = x.operacion.NotificacionOperacion,
                    //    TipoDocumento = x.operacion.TipoDocumento,
                    //    NombreFinal = x.operacion.NombreFinal,
                    //    DocumentoElectronicoOperacion = new DocumentoElectronicoOperacion
                    //    {
                    //        IDDoctoElectronicoOperacion=x.documentoelectronico.IDDoctoElectronicoOperacion,
                    //        IDOperacion=x.documentoelectronico.IDOperacion,
                    //        Memo = x.documentoelectronico.Memo,
                    //    },
                        
                    //    TipoDoc = new Concepto { DescripcionCorta = x.tipodocumento.DescripcionCorta },
                    //    Prioridad= new Concepto{DescripcionConcepto=x.prioridad.DescripcionConcepto},
                    //    Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                    //}));


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

                                 where tipomesa.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario &&  x.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV ) > 0)

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
                        NombreFinal = x.operacion.NombreFinal,
                        TipoDoc = new Concepto { DescripcionCorta = x.tipomesa.DescripcionCorta },
                        Estado = new Concepto { DescripcionConcepto = x.estado.DescripcionConcepto },
                        Prioridad = new Concepto { DescripcionCorta = x.prioridad.DescripcionCorta },
                    }));


                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listOperacion;
        }
        public List<EOperacion> ListarMesaTrabajoVirtual(UsuarioParticipante eUsuarioParticipante)
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

                                 join organizador in db.UsuarioParticipantes
                                 on operacion.IDOperacion equals organizador.IDOperacion

                                 join usuario in db.Usuarios
                                 on organizador.IDUsuario equals usuario.IDUsuario

                                 where tipomesa.TipoConcepto.Equals("012") &&
                                        estado.TipoConcepto.Equals("001") &&
                                        prioridad.TipoConcepto.Equals("005") &&
                                        //(operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && (x.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV || x.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV)) > 0)
                                        (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) > 0)
                                        && organizador.TipoParticipante.Equals(Constantes.TipoParticipante.OrganizadorMV)
                                 select new { operacion, tipomesa,  /*usuariopart,*/ estado /*,usuario*/, prioridad,organizador,usuario }).ToList();

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
                        Prioridad = new Concepto { DescripcionCorta = x.prioridad.DescripcionCorta },

                        OrganizadorMV = x.usuario.NombreUsuario,
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
                    entidad.FechaCierre = operacion.FechaCierre; 
                    entidad.NombreFinal = operacion.NombreFinal;
                    entidad.EstadoOperacion = operacion.EstadoOperacion;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public short EliminarOperacion(Operacion operacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var ope = db.Operacions.Find(operacion.IDOperacion);
                    ope.EstadoOperacion = operacion.EstadoOperacion;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string NumeroOperacion(Int64 IDUsuario,string tipooperacion, string tipodocumento, int idempresa)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var iCodiUsu = new SqlParameter { ParameterName = "@iCodiUsu", Value = IDUsuario };
                    var cTipoOper = new SqlParameter { ParameterName = "@cTipoOper", Value = tipooperacion };
                    var cTipoDocto = new SqlParameter { ParameterName = "@cTipoDocto", Value = tipodocumento };
                    var iIDEmpresa = new SqlParameter { ParameterName = "@iIDEmpresa", Value = idempresa };
                    var out_cNumeroOperacion = new SqlParameter
                    {
                        ParameterName = "@out_cNumeroOperacion",
                        Value = "",
                        Direction = ParameterDirection.Output };
                    //var out_cNumeroOperacion = new SqlParameter("@out_cNumeroOperacion", SqlDbType.VarChar) { Direction = System.Data.ParameterDirection.Output };

                    var numero = db.Database.SqlQuery<String>("gdoc_NumeraOperacion @iIDEmpresa, @iCodiUsu, @cTipoOper, @cTipoDocto, @out_cNumeroOperacion out", iIDEmpresa, iCodiUsu, cTipoOper, cTipoDocto, out_cNumeroOperacion);

                    //var NumeroOperacion = (string)out_cNumeroOperacion.Value;

                    var OperacionNumero = numero.First();
                    return OperacionNumero;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
