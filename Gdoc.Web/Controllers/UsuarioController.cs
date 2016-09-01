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
                    Session["IDEmpresa"] = 1001; //Pendiente falta terminar
                    Session["NombreUsuario"] = UsuarioEncontrado.NombreUsuario;
                    Session["NombreCompleto"] = string.Format("{0}, {1}", UsuarioEncontrado.Personal.NombrePers, UsuarioEncontrado.Personal.ApellidoPersonal);
                    Session["CargoUsuario"] = UsuarioEncontrado.Personal.CodigoCargo;
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
                listUsuario = oUsuario.ListarUsuario();
            }
            return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarUsuarioGrupo(Usuario usuario)
        {
            var listUsuarioGrupo = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuarioGrupo = oUsuario.ListarUsuario().Where(x => x.IDUsuario == usuario.IDUsuario).ToList();
            }
            return new JsonResult { Data = listUsuarioGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}
