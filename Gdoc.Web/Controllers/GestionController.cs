using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGdoc_MVC.Controllers
{
    public class GestionController : Controller
    {
        //
        // GET: /Gestion/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DocumentoElectronico()
        {
            return View();
        }

        public ActionResult DocumentoDigital()
        {
            return View();
        }

        public ActionResult MesaVirtual()
        {
            return View();
        }

    }
}
