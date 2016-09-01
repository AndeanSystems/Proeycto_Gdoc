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
    public class ConceptoController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: Concepto
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarConcepto() {
            var listConcepto = new List<Concepto>();
            using (var oConcepto = new NConcepto())
            {
                listConcepto = oConcepto.ListarConcepto();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listConcepto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarConcepto(Concepto concepto)
        {
            var listConcepto = new List<Concepto>();
            using (var oConcepto = new NConcepto())
            {
                listConcepto = oConcepto.ListarConcepto().Where(x => x.TipoConcepto == concepto.TipoConcepto).ToList();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listConcepto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GrabarConcepto(Concepto concepto) {
            using (var oConcepto = new NConcepto())
            {
                concepto.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                concepto.EditarRegistro = 1;//por terminar
                concepto.UsuarioModifica = Session["NombreUsuario"].ToString();
                concepto.FechaModifica=System.DateTime.Now;
                var respuesta = oConcepto.GrabarConcepto(concepto);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

    }
}