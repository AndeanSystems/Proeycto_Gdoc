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
        public JsonResult Grabar(Operacion operacion)
        {
            using (var oOperacion = new NOperacion())
            {
                //FALTA TERMINAR
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = "02";
                operacion.FechaEmision = DateTime.Now;
                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();
                //operacion.TituloOperacion = "Documento Digital";
                operacion.EstadoOperacion = "0";
                //operacion.DescripcionOperacion = "Documento Digital";

                operacion.FechaCierre = DateTime.Now;
                operacion.FechaRegistro = DateTime.Now;
                operacion.FechaEnvio = DateTime.Now;
                operacion.FechaVigente = DateTime.Now;
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";

               
                //var respuesta = oOperacion.Grabar(operacion);

                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}