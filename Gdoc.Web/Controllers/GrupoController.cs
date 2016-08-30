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
    public class GrupoController : Controller
    {
        //
        // GET: /Grupo/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarGrupo()
        {
            var listGrupo = new List<Grupo>();
            using (var oGrupo = new NGrupo())
            {
                listGrupo = oGrupo.ListarGrupo();
            }
            return new JsonResult { Data = listGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}