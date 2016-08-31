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
        [HttpPost]
        public JsonResult ListarUsuario(Usuario usuario)
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                //listUsuario = oUsuario.ListarUsuario();
                listUsuario.AddRange(new List<EUsuario> {
                    new EUsuario {IDUsuario = 19, NombreUsuario = "APACAYA", Personal = new Personal { NombrePers = "ALEXANDER", ApellidoPersonal = "PACAYA" ,EmailTrabrajo = "@gmail.com"} },
                    new EUsuario { IDUsuario = 20, NombreUsuario = "JSANCHEZ", Personal = new Personal { NombrePers = "JUAN", ApellidoPersonal = "SANCHEZ SANCHEZ" ,EmailTrabrajo = "@gmail.com"} },
                    new EUsuario { IDUsuario = 21, NombreUsuario = "JMORALES", Personal = new Personal { NombrePers = "JUAN", ApellidoPersonal = "MORALES FERNANDEZ" ,EmailTrabrajo = "@gmail.com"} },
                    new EUsuario { IDUsuario = 22, NombreUsuario = "FSALINAS", Personal = new Personal { NombrePers = "FRANCISCO", ApellidoPersonal = "SALINAS PEREZ" ,EmailTrabrajo = "@gmail.com"} }
                    }
                );
            }
            return new JsonResult { Data = listUsuario, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}
