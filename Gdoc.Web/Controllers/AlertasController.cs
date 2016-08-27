using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGdoc_MVC.Controllers
{
    public class AlertasController : Controller
    {
        //
        // GET: /Alertas/

        public ActionResult Index()
        {
            ViewBag.user="Anderson";
            return View();
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
