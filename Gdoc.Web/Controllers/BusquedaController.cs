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
    public class BusquedaController : Controller
    {
        //
        // GET: /Busqueda/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListarOperacionBusqueda(Operacion operacion)
        {
            var listOperacion = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listOperacion = oOperacion.ListarOperacionBusqueda().Where(x=>x.TipoOperacion==operacion.TipoOperacion).ToList();
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}