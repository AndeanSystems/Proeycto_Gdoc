using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class UbigeoController : Controller
    {
        //
        // GET: /Ubigeo/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUbigeo()
        {
            var listUbigeo = new List<Ubigeo>();
            using (var oUbigeo = new NUbigeo())
            {
                listUbigeo = oUbigeo.ListarUbigeo();
            }
            return new JsonResult { Data = listUbigeo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpGet]
        public JsonResult ListarDepartamento()
        {
            var listUbigeo = new List<Ubigeo>();
            using (var oUbigeo = new NUbigeo())
            {
                listUbigeo = oUbigeo.ListarUbigeo().Where(x => x.CodigoProvincia == 0 && x.CodigoDistrito==0 && x.EstadoUbigeo==1).ToList();
            }
            return new JsonResult { Data = listUbigeo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarProvincias(Ubigeo ubigeo)
        {
            var listUbigeo = new List<Ubigeo>();
            using (var oUbigeo = new NUbigeo())
            {
                listUbigeo = oUbigeo.ListarUbigeo().Where(x => x.CodigoDepartamento==ubigeo.CodigoDepartamento && x.CodigoProvincia != 0 && x.CodigoDistrito == 0 && x.EstadoUbigeo == 1).ToList();
            }
            return new JsonResult { Data = listUbigeo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarDistritos(Ubigeo ubigeo)
        {
            var listUbigeo = new List<Ubigeo>();
            using (var oUbigeo = new NUbigeo())
            {
                listUbigeo = oUbigeo.ListarUbigeo().Where(x => x.CodigoDepartamento == ubigeo.CodigoDepartamento && x.CodigoProvincia ==ubigeo.CodigoProvincia && x.CodigoDistrito != 0 && x.EstadoUbigeo == 1).ToList();
            }
            return new JsonResult { Data = listUbigeo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}