using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGdoc_MVC.Controllers
{
    public class AdministracionController : Controller
    {
        //
        // GET: /Administracion/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Busqueda()
        {
            return View();
        }
    }
}
