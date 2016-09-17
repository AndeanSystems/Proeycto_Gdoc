using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
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
                //FALTA TERMINAR QUITAR VALORES EN DURO
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                operacion.FechaEmision = DateTime.Now;
                operacion.FechaCierre = DateTime.Now;
                operacion.FechaVigente = DateTime.Now;
                operacion.FechaEnvio = DateTime.Now;
                operacion.FechaRegistro = DateTime.Now;
                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";
                //operacion.EstadoOperacion = "0";

                //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                using (var oNOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oNOperacion.Grabar(operacion,listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario, null);
                }
                mensajeRespuesta.Mensaje = "Operación realizado correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = "Operación no realizado correctamente";
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
        public JsonResult ListarOperacion()
        {
            var listDocumentoElectronico = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listDocumentoElectronico = oOperacion.ListarOperacionElectronico().Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico).ToList();
            }
            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}