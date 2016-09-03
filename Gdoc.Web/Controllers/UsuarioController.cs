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
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
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
                    //string nombre = UsuarioEncontrado.Personal.NombrePers;
                    //string nombre_may = UsuarioEncontrado.Personal.NombrePers.ToUpper();

                    //nombre=nombre.ToLower();

                    //nombre = nombre.Replace(nombre[0], nombre_may[0]);

                    Session["IDEmpresa"] = 1001; //Pendiente falta terminar
                    Session["NombreUsuario"] = UsuarioEncontrado.NombreUsuario;
                    Session["NombreCompleto"] = string.Format("{0} {1}", FormatoNombre(UsuarioEncontrado.Personal.NombrePers), FormatoNombre(UsuarioEncontrado.Personal.ApellidoPersonal));
                    Session["CargoUsuario"] = UsuarioEncontrado.Personal.CodigoCargo;
                    return RedirectToAction("Index", "Alertas");
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
        public JsonResult GrabarUsuario(Usuario usuario,Personal persona)
        {
            using (var oUsuario = new NUsuario())
            {
                //POR TERMINAR
                usuario.ClaveUsuario = "";
                usuario.FechaRegistro = System.DateTime.Now;
                usuario.FirmaElectronica = "";
                usuario.FechaModifica = System.DateTime.Now;
                usuario.IntentoErradoClave = 3;
                usuario.IntentoerradoFirma = 2;
                usuario.CodigoConexion = "";
                usuario.UsuarioRegistro = Session["NombreUsuario"].ToString();
                usuario.ExpiraClave = "1";//0=EXPIRA || 1=NO EXPIRA
                //usuario.ExpiraFirma = "90";
                usuario.FechaExpiraClave = System.DateTime.Now;
                usuario.FechaExpiraFirma = System.DateTime.Now.AddDays(90);//falta terminar traer el valor de dias de experacion de firmas de la tabla general

                var respuesta = oUsuario.GrabarUsuario(usuario);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

        [HttpPost]
        public JsonResult BuscarUsuarioNombre(Usuario usuario)
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                    listUsuario = oUsuario.ListarUsuario().Where(x => x.NombreUsuario.Contains(usuario.NombreUsuario.ToUpper())).ToList();
                else
                    listUsuario = oUsuario.ListarUsuario();
            }

            return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
        }

        #region "Metodos"
        public string FormatoNombre(string nombre)
        {
            string nombre_orig = nombre;
            string nombre_may = nombre.ToUpper();

            nombre_orig = nombre_orig.ToLower();

            nombre_orig = nombre_orig.Replace(nombre_orig[0], nombre_may[0]);
            return nombre_orig;
        }
        #endregion
    }
}
