using Gdoc.Entity.Extension;
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
    public class LogOperacionController : Controller
    {
        //
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 6 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        [HttpPost]
        public JsonResult ListarLogOperacion(Operacion operacion)
        {
            try
            {
                var listLogOperacion = new List<ELogOperacion>();
                using (var oLogOperacion = new NLogOperacion())
                {
                    listLogOperacion = oLogOperacion.ListarLogOperacion()
                        .Where(x=>x.Operacion.TipoOperacion==operacion.TipoOperacion && x.Operacion.NumeroOperacion==operacion.NumeroOperacion).ToList();
                }
                return new JsonResult { Data = listLogOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
	}
}