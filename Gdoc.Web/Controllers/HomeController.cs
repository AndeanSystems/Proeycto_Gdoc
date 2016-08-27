using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebGdoc_MVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string con)
        {
            ViewBag.id = id;
            ViewBag.con = con;

            if (id.Equals("123")&&con.Equals("123"))
            {
                Session["trabajador"] = "Anderson";
                return RedirectToAction("Alertas", "Alertas");
            }
            else
            {
                return View();
            }
            //return RedirectToAction("Alertas","Alertas");
        }

        public ActionResult Logueo()
        {
            return View();
        }

    }
}
