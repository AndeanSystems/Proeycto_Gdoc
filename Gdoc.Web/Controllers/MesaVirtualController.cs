using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class MesaVirtualController : Controller
    {
        //
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion,List<Adjunto> listAdjuntos, List<EUsuarioGrupo> listEUsuarioGrupo)
        {
            try
            {
                //FALTA TERMINAR QUITAR VALORES EN DURO
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.MesaVirtual;


                if (operacion.EstadoOperacion == 1)
                {
                    //if (operacion.FechaRegistro == null)
                        operacion.FechaRegistro = DateTime.Now;
                    operacion.FechaEnvio = DateTime.Now;
                }
                else
                    operacion.FechaRegistro = DateTime.Now;

                operacion.NumeroOperacion = "MV"+DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";

                if (listAdjuntos != null)
                    operacion.DocumentoAdjunto = "S";
                else
                    operacion.DocumentoAdjunto = "N";

                //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                using (var oNOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oNOperacion.GrabarMesaVirtual(operacion, listAdjuntos, listEUsuarioGrupo, IDusuario);
                }
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Operación " + operacion.NumeroOperacion + " realizada correctamente";
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = "Operación no realizada correctamente";
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }

        [HttpPost]
        public JsonResult GrabarMesaVirtualComentario(Operacion operacion,List<Adjunto> listAdjuntos, MesaVirtualComentario mesaVirtualComentario)
        {
            try
            {
                mesaVirtualComentario.FechaPublicacion = System.DateTime.Now;
                mesaVirtualComentario.EstadoComentario = 1;
                mesaVirtualComentario.IDOperacion = operacion.IDOperacion;
                mesaVirtualComentario.IDUsuario = Convert.ToInt64(Session["IDUsuario"]);

                using (var oNMesaVirtualComentario = new NMesaVirtualComentario())
                {
                    Int64 IDUsuario= Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oNMesaVirtualComentario.GrabarMesaVirtualComentario(operacion, listAdjuntos, mesaVirtualComentario, IDUsuario);
                }
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
        public JsonResult ListarOperacion()
        {
            var listMesaVirtual = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listMesaVirtual = oOperacion.ListarMesaVirtual(new UsuarioParticipante
                {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString())
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();

                //listMesaVirtual = oOperacion.ListarMesaVirtual().Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();

            }
            return new JsonResult { Data = listMesaVirtual, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarMesaTrabajoVirtual()
        {
            var listMesaVirtual = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listMesaVirtual = oOperacion.ListarMesaTrabajoVirtual(new UsuarioParticipante
                {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString())
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();

                //listMesaVirtual = oOperacion.ListarMesaVirtual().Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();

            }
            return new JsonResult { Data = listMesaVirtual, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarUsuarioParticipanteMV(Operacion operacion)
        {
            var listUsuarioParticipante = new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };      
        }
        [HttpPost]
        public JsonResult ListarComentarioMesaVirtual(Operacion operacion)
        {
            var listComentarioMesaVirtual = new List<MesaVirtualComentario>();
            using (var oMesaVirtualComentario = new NMesaVirtualComentario())
            {
                listComentarioMesaVirtual = oMesaVirtualComentario.ListarMesaVirtualComentario().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listComentarioMesaVirtual, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarDocumentoAdjunto(MesaVirtualComentario mesaVirtualComentario)
        {
            var listDocumentoAdjunto = new List<EDocumentoAdjunto>();
            using (var oDocumentoAdjunto = new NDocumentoAdjunto())
            {
                listDocumentoAdjunto = oDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDComentarioMesaVirtual == mesaVirtualComentario.IDComentarioMesaVirtual).ToList();
            }
            return new JsonResult { Data = listDocumentoAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarDocumentoAdjuntoOperacion(Operacion operacion)
        {
            var listDocumentoAdjunto = new List<EDocumentoAdjunto>();
            using (var oDocumentoAdjunto = new NDocumentoAdjunto())
            {
                listDocumentoAdjunto = oDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDOperacion == operacion.IDOperacion && x.IDComentarioMesaVirtual==null).ToList();
            }
            return new JsonResult { Data = listDocumentoAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult EliminarOperacion(Operacion operacion)
        {
            try
            {
                using (var oOperacion = new NOperacion())
                {
                    operacion.EstadoOperacion = Estados.EstadoOperacion.Inactivo;
                    var respuesta = oOperacion.AnularMesaVirtual(operacion);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grupo de Trabajo Inactivo";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {

                throw;
            }

        }
	}
}