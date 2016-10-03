using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
namespace Gdoc.Web.Controllers
{
    public class DocumentoAdjuntoController : Controller
    {
        //
        // GET: /DocumentoAdjunto/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ListarAdjuntos(string archivo)
        {
            try
            {
                var ruta = "http://192.168.100.29:85/ADJUNTOS/" + archivo;
                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
	}
}