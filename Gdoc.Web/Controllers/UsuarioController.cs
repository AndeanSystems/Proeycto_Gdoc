using Gdoc.Entity.Models;
using Gdoc.Negocio;
using System.Web.Mvc;

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
    }
}
