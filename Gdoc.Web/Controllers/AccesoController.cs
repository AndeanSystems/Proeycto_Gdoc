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
    public class AccesoController : Controller
    {
        //
        // GET: /Acceso/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarAccesoSistema()
        {
            var listAccesoSistema = new List<EAccesoSistema>();
            using (var oAccesoSistema = new NAccesoSistema())
            {
                listAccesoSistema = oAccesoSistema.ListarAccesoSistema();
            }
            return new JsonResult { Data = listAccesoSistema, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarAccesoSistema(EAccesoSistema acceso)
        {
            var listAccesoSistema = new List<EAccesoSistema>();
            using (var oAccesoSistema = new NAccesoSistema())
            {
                if (!string.IsNullOrEmpty(acceso.Usuario.NombreUsuario))
                    listAccesoSistema = oAccesoSistema.ListarAccesoSistema().Where(x => x.Usuario.NombreUsuario.Contains(acceso.Usuario.NombreUsuario.ToUpper())).ToList();
                else
                    listAccesoSistema = oAccesoSistema.ListarAccesoSistema();
                }
            return new JsonResult { Data = listAccesoSistema, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}