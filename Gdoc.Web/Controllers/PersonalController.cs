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
    public class PersonalController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Personal/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 9 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult ListarPersonal()
        {
            var listPersonal = new List<EPersonal>();
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
                    Personal respuesta = null;
                    if (personal.IDPersonal > 0)
                    {
                        respuesta = oPersonal.EditarPersonal(personal);
                        mensajeRespuesta.Exitoso = true;
                        mensajeRespuesta.Mensaje = "Actualización Exitosa";
                    }
                    else
                    {
                        respuesta = oPersonal.GrabarPersonal(personal);
                        mensajeRespuesta.Exitoso = true;
                        mensajeRespuesta.Mensaje = "Grabación Exitosa";
                    }
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception ex)
            {
                 mensajeRespuesta.Exitoso = false;
                mensajeRespuesta.Mensaje = ex.Message;
                return new JsonResult { Data = mensajeRespuesta };
            }
            
        }
        public JsonResult BuscarPersonalNombre(Personal personal)
        {
            var listUsuario = new List<EPersonal>();
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