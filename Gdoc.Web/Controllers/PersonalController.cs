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
        [HttpGet]
        public JsonResult ListarPersonal()
        {
            var listPersonal = new List<Personal>();
            using (var oPersonal = new NPersonal())
            {
                listPersonal = oPersonal.ListarPersonal();
            }
            return new JsonResult { Data = listPersonal, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarPersonal(Personal personal)
        {
            try
            {
                using (var oPersonal = new NPersonal())
                {
                    //personal.IDEmpresa = 1001;
                    //personal.UsuarioRegistro = Session["NombreUsuario"].ToString();
                    //var respuesta = oPersonal.GrabarPersonal(personal);
                    Personal respuesta = null;
                    if (personal.IDPersonal > 0)
                        respuesta = oPersonal.EditarPersonal(personal);
                    else
                        respuesta = oPersonal.GrabarPersonal(personal);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitosa";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        [HttpPost]
        public JsonResult BuscarPersonalNombre(Personal personal)
        {
            var listUsuario = new List<Personal>();
            using (var oPersonal = new NPersonal())
            {
                if (!string.IsNullOrEmpty(personal.NombrePers))
                    listUsuario = oPersonal.ListarPersonal().Where(x => x.NombrePers.Contains(personal.NombrePers.ToUpper())).ToList();
                else
                    listUsuario = oPersonal.ListarPersonal();
            }

            return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
        }
	}
}