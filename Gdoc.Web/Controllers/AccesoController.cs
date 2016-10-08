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
    public class AccesoController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Acceso/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 12 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
                //return View("../Alertas/Index");
                return RedirectToAction("Index", "Blanco");
        }
        [HttpGet]
        public JsonResult ListarAccesoSistema()
        {
            var listAccesoSistema = new List<EAccesoSistema>();
            using (var oAccesoSistema = new NAccesoSistema())
            {
                listAccesoSistema = oAccesoSistema.ListarAccesoSistema();
            }
            return new JsonResult { Data = listAccesoSistema, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarAccesoSistema(EUsuario usuario)
        {
            var listAccesoSistema = new List<EAccesoSistema>();
            using (var oAccesoSistema = new NAccesoSistema())
            {
                if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                    listAccesoSistema = oAccesoSistema.ListarAccesoSistema().Where(x => x.Usuario.NombreUsuario.Contains(usuario.NombreUsuario.ToUpper())).ToList();//por terminar
                else
                    listAccesoSistema = oAccesoSistema.ListarAccesoSistema();
                }
            return new JsonResult { Data = listAccesoSistema, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult DesactivarAcceso(AccesoSistema accesosistema)
        {
            using (var oAccesosistema = new NAccesoSistema())
            {
                if (accesosistema.IDAcceso != 0)
                {
                    accesosistema.EstadoAcceso = Estados.EstadoAcceso.Inactivo;
                    var respuesta = oAccesosistema.CambiarEstadoAcceso(accesosistema);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitoso";
                }
                
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

        public JsonResult ActivarAcceso(EAccesoSistema accesosistema)
        {
            using (var oAccesosistema = new NAccesoSistema())
            {

                var acceso = new AccesoSistema();
                if (accesosistema.IDAcceso != 0)
                {
                    acceso.IDUsuario = accesosistema.IDUsuario;
                    acceso.IDAcceso = accesosistema.IDAcceso;
                    acceso.IDModuloPagina = accesosistema.IDModuloPagina;
                    acceso.EstadoAcceso = Estados.EstadoAcceso.Activo;
                    oAccesosistema.CambiarEstadoAcceso(acceso);
                }
                else
                {
                    //accesosistema.IDAcceso = null;
                    acceso.IDUsuario = accesosistema.IDUsuario;
                    acceso.IDModuloPagina = accesosistema.IDModuloPagina2;
                    acceso.IdeUsuarioRegistro = Session["NombreUsuario"].ToString();
                    acceso.FechaModificacion = System.DateTime.Now;
                    acceso.EstadoAcceso = 1;
                    oAccesosistema.grabarAcceso(acceso);
                }
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}