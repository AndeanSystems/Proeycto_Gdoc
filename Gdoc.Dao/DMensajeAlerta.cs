﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using Gdoc.Common.Utilitario;

namespace Gdoc.Dao
{
    public class DMensajeAlerta
    {
        public List<EMensajeAlerta> ListarMensajeAlerta(Int64 IDUsuario)
        {
            var listMensajeAlerta = new List<EMensajeAlerta>();
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

                                           (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == IDUsuario
                                            && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioProveido || x.TipoParticipante==Constantes.TipoParticipante.ColaboradorMV)) > 0)
                                            && (remitente.TipoParticipante == Constantes.TipoParticipante.RemitenteDE || remitente.TipoParticipante==Constantes.TipoParticipante.RemitenteProveido || remitente.TipoParticipante==Constantes.TipoParticipante.OrganizadorMV)
                                          select new { usuario,operacion,remitente }).ToList();

                    var list2 = (from mensajealerta in db.MensajeAlertas

                                 join operacion in db.Operacions
                                 on mensajealerta.IDOperacion equals operacion.IDOperacion

                                 join evento in db.Conceptoes
                                 on mensajealerta.CodigoEvento equals evento.CodiConcepto

                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                 join tipodocumento in db.Conceptoes
                                 on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                 where tipodocumento.TipoConcepto.Equals("012")
                                       && tipooperacion.TipoConcepto.Equals("003")
                                       && evento.TipoConcepto.Equals("008")
                                       && mensajealerta.FechaAlerta.Value.Day == System.DateTime.Now.Day
                                       && mensajealerta.IDUsuario==IDUsuario
                                       //&& (operacion.UsuarioParticipantes.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario && (x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE || x.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD || x.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV)) > 0)

                                 select new { mensajealerta, tipooperacion, tipodocumento, operacion, evento }).ToList();

                    
                    foreach (var x in list2)
                    {
                        foreach (var item in listremitentes.Where(y => y.operacion.IDOperacion == x.operacion.IDOperacion))
                        {
                            if (x.mensajealerta.TipoAlerta == 4)
                            {
                                if (item.remitente.TipoParticipante == Constantes.TipoParticipante.RemitenteProveido)
                                {
                                    var lisProveidos = (from proveidos in db.MesaVirtualComentarios
                                                        where proveidos.IDOperacion == x.operacion.IDOperacion
                                                        && proveidos.IDUsuario == item.remitente.IDUsuario
                                                        select new { proveidos }).FirstOrDefault();

                                    if (item.remitente.TipoParticipante == Constantes.TipoParticipante.RemitenteProveido && x.mensajealerta.FechaAlerta == lisProveidos.proveidos.FechaPublicacion)
                                        remitentes.Add(item.usuario.NombreUsuario);
                                }
                            }
                            else
                            {
                                if (item.remitente.TipoParticipante != Constantes.TipoParticipante.RemitenteProveido)
                                    remitentes.Add(item.usuario.NombreUsuario);
                            }
                        }
                        listMensajeAlerta.Add(new EMensajeAlerta
                        {
                            IDMensajeAlerta = x.mensajealerta.IDMensajeAlerta,
                            IDOperacion = x.mensajealerta.IDOperacion,
                            FechaAlerta = x.mensajealerta.FechaAlerta,
                            TipoAlerta = x.mensajealerta.TipoAlerta,
                            CodigoEvento = x.mensajealerta.CodigoEvento,
                            EstadoMensajeAlerta = x.mensajealerta.EstadoMensajeAlerta,
                            IDUsuario = x.mensajealerta.IDUsuario,
                            Remitente=x.mensajealerta.Remitente,

                            TipoOperacion = new Concepto
                            {
                                DescripcionCorta = x.tipooperacion.DescripcionCorta,
                            },
                            TipoDocumento = new Concepto
                            {
                                DescripcionCorta = x.tipodocumento.DescripcionCorta,
                            },
                            Evento = new Concepto
                            {
                                DescripcionConcepto = x.evento.DescripcionConcepto,
                            },
                            Operacion = new Operacion
                            {
                                IDOperacion = x.operacion.IDOperacion,
                                NumeroOperacion = x.operacion.NumeroOperacion,
                                TipoOperacion = x.operacion.TipoOperacion,
                                NombreFinal = x.operacion.NombreFinal,
                                PrioridadOperacion=x.operacion.PrioridadOperacion,
                                FechaEnvio=x.operacion.FechaEnvio,
                            },
                            //Remitentes = string.Join(",", remitentes.ToArray()),
                        });
                        remitentes = new List<String>();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listMensajeAlerta;
        }
        public Int32 GrabarMensajeAlerta(MensajeAlerta mensajeAlerta)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.MensajeAlertas.Add(mensajeAlerta);
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
