using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class EmpresaController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Empresa/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 10 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }

        [HttpGet]
        public JsonResult ListarEmpresa()
        {
            var listEmpresa = new List<EEmpresa>();
            using (var oEmpresa = new NEmpresa())
            {
                listEmpresa = oEmpresa.ListarEmpresa();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listEmpresa, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarEmpresa(Empresa empresa)
        {
            try
            {
                using (var oEmpresa = new NEmpresa())
                {
                    empresa.FechaRegistro = System.DateTime.Now;
                    empresa.UsuarioRegistro = Session["NombreUsuario"].ToString();
                    Empresa respuesta = null;
                    if (empresa.IDEmpresa > 0)
                    {
                        respuesta = oEmpresa.EditarEmpresa(empresa);
                        mensajeRespuesta.Exitoso = true;
                        mensajeRespuesta.Mensaje = "Actualización Exitosa";
                    }
                    else
                    {
                        respuesta = oEmpresa.GrabarEmpresa(empresa);
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
        public JsonResult EliminarEmpresa(Empresa empresa)
        {
            try
            {
                using (var oEmpresa = new NEmpresa())
                {
                    empresa.EstadoEmpresa = Estados.EstadoEmpresa.Inactivo;
                    var respuesta = oEmpresa.EliminarEmpresa(empresa);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitoso";
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

	}
}