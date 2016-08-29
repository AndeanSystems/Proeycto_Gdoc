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
    public class EmpresaController : Controller
    {
        //
        // GET: /Empresa/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarEmpresa()
        {
            var listEmpresa = new List<Empresa>();
            using (var oEmpresa = new NEmpresa())
            {
                listEmpresa = oEmpresa.ListarEmpresa();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listEmpresa, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}