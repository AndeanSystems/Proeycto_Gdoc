using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
using System.Configuration;

namespace Gdoc.Web.Controllers
{
    public class DocumentosRecibidosController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        protected string Usuario = "U";
        protected string Grupo = "G";
        protected NUsuarioParticipante dUsuarioParticipante = new NUsuarioParticipante();
        protected NMensajeAlerta dMensajeAlerta = new NMensajeAlerta();
        protected NUsuarioGrupo dUsuarioGrupo = new NUsuarioGrupo();
        protected NUsuario dUsuario = new NUsuario();
        #endregion
        // GET: /DocumentosRecibidos/
        public ActionResult Index()
        {
            if (Session["ListaAccesos"] == null)
                return RedirectToAction("Index", "Home");

            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 19 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult ListarOperacion()
        {
            try
            {
                var listOperacion = new List<EOperacion>();
                using (var oOperacion = new NOperacion())
                {
                    listOperacion = oOperacion.ListarDocumentosRecibidos(new UsuarioParticipante
                    {
                        IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                    }).Where(x => x.EstadoOperacion==Estados.EstadoOperacion.Activo && 
                        (x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico || x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital)
                        && x.FechaRegistro.Value.Day == System.DateTime.Now.Day).OrderByDescending(x => x.FechaEnvio).ToList();
                }
                return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante = new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion 
                    && x.TipoParticipante!=Constantes.TipoParticipante.DestinatarioProveido
                    && x.TipoParticipante!=Constantes.TipoParticipante.RemitenteProveido).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarOperacionPorFecha(Nullable<System.DateTime> fecha)
        {
            try
            {
                DateTime a123 = Convert.ToDateTime(fecha);
                var listOperacion = new List<EOperacion>();
                using (var oOperacion = new NOperacion())
                {
                    listOperacion = oOperacion.ListarDocumentosRecibidos(new UsuarioParticipante
                    {
                        IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                    }).Where(x => x.EstadoOperacion == Estados.EstadoOperacion.Activo &&
                        (x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico || x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital) && Convert.ToDateTime(x.FechaEnvio).ToString("dd/MM/yyyy") == Convert.ToDateTime(fecha).ToString("dd/MM/yyyy")).ToList();
                }
                return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }

        }
        public JsonResult ListarDocumentoPDF(EOperacion operacion)
        {
            try
            {
                string sWebSite = ConfigurationManager.AppSettings.Get("Documentos");
                var ruta = sWebSite + operacion.NombreFinal;

                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult ListarAdjuntos(EAdjunto adjunto)
        {
            try
            {
                string sWebSite = string.Empty;
                if (adjunto.Archivo == null)
                {
                    sWebSite = ConfigurationManager.AppSettings.Get("Documentos");
                    adjunto.Archivo = adjunto.NombreOriginal;
                }
                else
                    sWebSite = ConfigurationManager.AppSettings.Get("Adjunto");

                var ruta = sWebSite + adjunto.Archivo;
                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult ListarDocumentoAdjunto(Operacion operacion)
        {
            var listDocumentoAdjunto = new List<EDocumentoAdjunto>();
            using (var oDocumentoAdjunto = new NDocumentoAdjunto())
            {
                listDocumentoAdjunto = oDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoDoctoAdjunto==Estados.EstadoAdjunto.Activo).ToList();
            }
            return new JsonResult { Data = listDocumentoAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarAdjunto(Operacion operacion)
        {
            var listAdjunto = new List<EAdjunto>();
            using (var oAdjunto = new NAdjunto())
            {
                listAdjunto = oAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion && x.EstadoAdjunto == Estados.EstadoAdjunto.Activo).ToList();
            }
            return new JsonResult { Data = listAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarComentarioProveido(Operacion alerta)
        {
            var listComentarioProveido = new List<EMesaVirtualComentario>();
            using (var oMesaVirtualComentario = new NMesaVirtualComentario())
            {
                listComentarioProveido = oMesaVirtualComentario.ListarMesaVirtualComentario().
                    Where(x => x.IDOperacion == alerta.IDOperacion).
                    OrderByDescending(x => x.FechaPublicacion).ToList();
            }
            return new JsonResult { Data = listComentarioProveido, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarComentarioProveido(Operacion operacion, MesaVirtualComentario mesaVirtualComentario, List<EUsuarioGrupo> listUsuariosDestinatarios)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var eUsuarioParticipante = new UsuarioParticipante();
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                mesaVirtualComentario.FechaPublicacion = System.DateTime.Now;
                mesaVirtualComentario.EstadoComentario = 1;
                mesaVirtualComentario.IDOperacion = operacion.IDOperacion;
                mesaVirtualComentario.IDUsuario = Convert.ToInt64(Session["IDUsuario"]);

                using (var oNMesaVirtualComentario = new NMesaVirtualComentario())
                {
                    Int64 IDUsuario = Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oNMesaVirtualComentario.GrabarMesaVirtualComentario(operacion,null, mesaVirtualComentario, IDUsuario);
                }


                eUsuarioParticipante.IDUsuario = Convert.ToInt32(Session["IDUsuario"]);
                eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                eUsuarioParticipante.TipoParticipante = Constantes.TipoParticipante.RemitenteProveido;
                eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                eUsuarioParticipante.ReenvioOperacion = "S";
                eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                listEusuarioParticipante.Add(eUsuarioParticipante);
                eUsuarioRemitente.Add(eUsuarioParticipante);
                foreach (var participante in listUsuariosDestinatarios)
                {
                    if (participante.Tipo.Equals(Usuario))
                    {
                        //Grabar solo Usuarios
                        eUsuarioParticipante = new UsuarioParticipante();
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                        eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                        eUsuarioParticipante.FechaNotificacion = System.DateTime.Now;
                        eUsuarioParticipante.ReenvioOperacion = "S";
                        eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                        //listEusuarioParticipante.Add(eUsuarioParticipante);
                        if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                            listEusuarioParticipante.Add(eUsuarioParticipante);

                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioProveido && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            GrabarMensajeAlerta("047", operacion, eUsuarioParticipante.IDUsuario, 4, mesaVirtualComentario, eUsuarioRemitente);
                        }
                    }
                    else
                    {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante = new UsuarioParticipante();
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = System.DateTime.Now;
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                listEusuarioParticipante.Add(eUsuarioParticipante);

                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioProveido && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            {
                                GrabarMensajeAlerta("047", operacion, eUsuarioParticipante.IDUsuario, 4, mesaVirtualComentario, eUsuarioRemitente);
                            }
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Comentario realizado correctamente";
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                mensajeRespuesta.Mensaje = "Operación no realizada correctamente";
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
        #region "Metodos"
        protected void GrabarMensajeAlerta(string codigoevento, Operacion operacion, Int64 IDusuario,int tipoalerta, MesaVirtualComentario mesaVirtualComentario, List<UsuarioParticipante> listRemitente)
        {
            try
            {
                var mensajeAlerta = new MensajeAlerta();
                var eUsuario = new List<string>();
                foreach (var item in listRemitente)
	            {
                    eUsuario.Add(dUsuario.ListarUsuario().Where(x => x.IDUsuario == item.IDUsuario).FirstOrDefault().NombreUsuario);
		 
	            }

                mensajeAlerta.IDOperacion = operacion.IDOperacion;
                if (codigoevento == "047")
                    mensajeAlerta.FechaAlerta = mesaVirtualComentario.FechaPublicacion;
                else 
                    mensajeAlerta.FechaAlerta = System.DateTime.Now;
                mensajeAlerta.TipoAlerta = tipoalerta;
                mensajeAlerta.EstadoMensajeAlerta = 1;
                mensajeAlerta.CodigoEvento = codigoevento;
                mensajeAlerta.IDUsuario = IDusuario;
                mensajeAlerta.Remitente = string.Join(",", eUsuario.ToArray());
                mensajeAlerta.IDComentarioMesaVirtual = mesaVirtualComentario.IDComentarioMesaVirtual;

                dMensajeAlerta.GrabarMensajeAlerta(mensajeAlerta);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}