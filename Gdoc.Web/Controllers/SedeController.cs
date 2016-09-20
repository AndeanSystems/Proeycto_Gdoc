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
    public class SedeController : Controller
    {
        //
        // GET: /Sede/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarSede()
        {
            var listSede = new List<Sede>();
            using (var oSede = new NSede())
            {
                listSede = oSede.ListarSede();
            }
            return new JsonResult { Data = listSede, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

	}
}