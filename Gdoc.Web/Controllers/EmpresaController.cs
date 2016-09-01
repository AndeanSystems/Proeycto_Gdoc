using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
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
            return View();
        }

        [HttpGet]
        public JsonResult ListarEmpresa()
        {
            var listEmpresa = new List<Empresa>();
            using (var oEmpresa = new NEmpresa())
            {
                listEmpresa = oEmpresa.ListarEmpresa();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listEmpresa, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarEmpresa(Empresa empresa)
        {
            using (var oEmpresa = new NEmpresa())
            {
                empresa.FechaRegistro = System.DateTime.Now;
                empresa.UsuarioRegistro = Session["NombreUsuario"].ToString(); 
                var respuesta = oEmpresa.GrabarEmpresa(empresa);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

	}
}