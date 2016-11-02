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
            if (Session["ListaAccesos"] == null)
                return RedirectToAction("Index", "Home");

            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 12 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
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
            try
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
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult DesactivarTodosAcceso(List<EModuloPaginaUrl> accesosistema)
        {
            try
            {
                using (var oAccesosistema = new NAccesoSistema())
                {
                    foreach (var item in accesosistema)
                    {
                        if (item.AccesoSistema.IDAcceso != 0)
                        {
                            item.AccesoSistema.EstadoAcceso = Estados.EstadoAcceso.Inactivo;
                            var respuesta = oAccesosistema.CambiarEstadoAcceso(item.AccesoSistema);
                            mensajeRespuesta.Exitoso = true;
                            mensajeRespuesta.Mensaje = "Grabación Exitoso";
                        }
                    }


                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult ActivarTodosAcceso(List<EModuloPaginaUrl> accesosistema)
        {
            using (var oAccesosistema = new NAccesoSistema())
            {
                foreach (var item in accesosistema)
                {
                    var acceso = new AccesoSistema();
                    if (item.AccesoSistema.IDAcceso != 0)
                    {
                        acceso.IDUsuario = item.AccesoSistema.IDUsuario;
                        acceso.IDAcceso = item.AccesoSistema.IDAcceso;
                        acceso.IDModuloPagina = item.IDModuloPagina;
                        acceso.EstadoAcceso = Estados.EstadoAcceso.Activo;
                        oAccesosistema.CambiarEstadoAcceso(acceso);
                    }
                    else
                    {
                        //accesosistema.IDAcceso = null;
                        acceso.IDUsuario = item.AccesoSistema.IDUsuario;
                        acceso.IDModuloPagina = item.AccesoSistema.IDModuloPagina2;
                        acceso.IdeUsuarioRegistro = Session["NombreUsuario"].ToString();
                        acceso.FechaModificacion = System.DateTime.Now;
                        acceso.EstadoAcceso = 1;
                        oAccesosistema.grabarAcceso(acceso);
                    }
                }
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
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