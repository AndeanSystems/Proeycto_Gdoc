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
            return View();
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