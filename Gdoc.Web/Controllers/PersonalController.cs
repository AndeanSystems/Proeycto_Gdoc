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
    public class PersonalController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Personal/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GrabarPersonal(Personal personal)
        {
            using (var oPersonal = new NPersonal())
            {
                personal.IDEmpresa = 1001;
                //personal.UsuarioRegistro = Session["NombreUsuario"].ToString();
                var respuesta = oPersonal.GrabarPersonal(personal);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}