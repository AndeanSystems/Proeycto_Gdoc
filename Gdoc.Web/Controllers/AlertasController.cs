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
    public class AlertasController : Controller
    {
        //
        // GET: /Alertas/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarMensajeAlerta()
        {
            var listMensajeAlerta = new List<EMensajeAlerta>();
            using (var oMensajeAlerta = new NMensajeAlerta())
            {
                listMensajeAlerta = oMensajeAlerta.ListarMensajeAlerta();
            }
            return new JsonResult { Data = listMensajeAlerta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult Alertas()
        {
            return View();
        }

        public ActionResult DocumentosRecibidos()
        {
            return View();
        }

        public ActionResult MesaTrabajoVirtual()
        {
            return View();
        }
    }
}
