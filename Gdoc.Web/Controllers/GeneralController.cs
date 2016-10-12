using Gdoc.Common.Utilitario;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class GeneralController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /General/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 16 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        [HttpGet]
        public JsonResult ListarGeneralParametros()
        {
            var listGeneralParametros = new List<General>();
            using (var oGeneral = new NGeneral())
            {
                listGeneralParametros = oGeneral.ListarGeneralParametros();
            }
            return new JsonResult { Data = listGeneralParametros, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarGeneralParametros(General general)
        {
            var listGeneralParametros = new List<General>();
            using (var oGeneral = new NGeneral())
            {
                listGeneralParametros = oGeneral.ListarGeneralParametros().Where(x => x.IDEmpresa == general.IDEmpresa).ToList();
            }
            return new JsonResult { Data = listGeneralParametros, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult EditarGeneralParametros(General general)
        {
            using (var oGeneral = new NGeneral())
            {
                General respuesta = null;
                if (general.IDCodigoParametro > 0)
                    respuesta = oGeneral.EditarGeneralParametros(general);
                else
                    respuesta = oGeneral.GrabarGeneralParametros(general);

                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}