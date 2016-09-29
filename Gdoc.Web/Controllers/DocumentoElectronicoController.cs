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
    public class DocumentoElectronicoController : Controller
    {
        #region "Variables"
        private MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        //
        // GET: /DocumentoElectronico/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo) {
            try
            {
                using (var oNOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    Int32 respuesta = 0;
                    if (operacion.IDOperacion > 0)
                    {
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                            new UtilPdf().GenerarArchivoPDF(operacion.NumeroOperacion, "Electronico", eDocumentoElectronicoOperacion.Memo, operacion.IDEmpresa, Session["RutaGdocPDF"].ToString());
                        }

                        respuesta = oNOperacion.EditarDocumentoElectronico(operacion, listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario);
                    }
                    else
                    {
                        operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                        operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;

                        operacion.NumeroOperacion = "DE" + DateTime.Now.Ticks.ToString();
                        operacion.NotificacionOperacion = "S";

                        if (listDocumentosAdjuntos != null)
                            operacion.DocumentoAdjunto = "S";
                        else
                            operacion.DocumentoAdjunto = "N";

                        operacion.FechaRegistro = DateTime.Now;
                        operacion.FechaEmision = DateTime.Now;
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                            operacion.NombreFinal = operacion.NumeroOperacion + ".pdf";
                        }

                        respuesta = oNOperacion.Grabar(operacion, listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario);

                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            new UtilPdf().GenerarArchivoPDF(operacion.NumeroOperacion, "Electronico", eDocumentoElectronicoOperacion.Memo, operacion.IDEmpresa, Session["RutaGdocPDF"].ToString());
                    }
                    
                }
                mensajeRespuesta.Mensaje = "Operación "+operacion.NumeroOperacion+" realizada correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = "Operación no realizada correctamente";
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
        public JsonResult ListarOperacion()
        {
            var listDocumentoElectronico = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listDocumentoElectronico = oOperacion.ListarOperacionElectronico(new UsuarioParticipante {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico).ToList();
            }

            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante= new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        protected DateTime DateAgregarLaborales(Int32 add, DateTime FechaInicial)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday) { FechaInicial = FechaInicial.AddDays(2); }
            if (FechaInicial.DayOfWeek == DayOfWeek.Sunday) { FechaInicial = FechaInicial.AddDays(1); }
            Int32 weeks = add / 5;
            add += weeks * 2;
            if (FechaInicial.DayOfWeek > FechaInicial.AddDays(add).DayOfWeek)
                add += 2;

            if (FechaInicial.AddDays(add).DayOfWeek == DayOfWeek.Saturday)
                add += 2;

            return FechaInicial.AddDays(add);
        }
	}
}