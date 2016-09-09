using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;


namespace Gdoc.Web.Controllers
{
    public class DocumentoDigitalController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /DocumentoDigital/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion, DocumentoDigitalOperacion documentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<IndexacionDocumento> listIndexacion)
        {
            try
            {
                //FALTA TERMINAR
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                operacion.FechaEmision = DateTime.Now;
                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();//FALTA

                operacion.FechaCierre = DateTime.Now.AddDays(5);//FALTA TRAER DE TABLA GNERAL
                operacion.FechaRegistro = DateTime.Now;
                operacion.FechaEnvio = DateTime.Now;//FALTA
                operacion.FechaVigente = DateTime.Now;//FALTA
                operacion.NotificacionOperacion = "S";//FALTA
                operacion.DocumentoAdjunto = "N";//FALTA

                using (var oOperacion = new NOperacion())
                {
                    var respuesta = oOperacion.GrabarDocumentoDigital(operacion, documentoDigitalOperacion, listEUsuarioGrupo, listIndexacion);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitoso";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult ListarOperacion()
        {
            var listOperacion = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listOperacion = oOperacion.ListarOperacion().Where(x =>x.TipoOperacion==Constantes.TipoOperacion.DocumentoDigital).ToList();
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult EliminarOperacion(Operacion operacion)
        {
            using (var oOperacion = new NOperacion())
            {
                operacion.EstadoOperacion = Estados.EstadoOperacion.Inactivo;
                var respuesta = oOperacion.EliminarOperacion(operacion);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}