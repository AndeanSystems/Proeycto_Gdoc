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
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(Usuario usuario)
        {
            using (var NUsuario = new NUsuario())
            {
                var UsuarioEncontrado = NUsuario.ValidarLogin(usuario);
                if (UsuarioEncontrado != null)
                {
                    Session["IDEmpresa"] = 1001;
                    return RedirectToAction("Alertas", "Alertas");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public JsonResult ListarUsuario()
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuario = oUsuario.ListarUsuario();
            }
            return new JsonResult { Data = listUsuario, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}
