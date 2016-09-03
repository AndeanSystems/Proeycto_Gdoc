using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Gdoc.Web.Util.Estados;

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
                Empresa respuesta = null;
                if (empresa.IDEmpresa > 0)
                    respuesta = oEmpresa.EditarEmpresa(empresa);
                else
                    respuesta = oEmpresa.GrabarEmpresa(empresa);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

        public JsonResult EliminarEmpresa(Empresa empresa)
        {
            using (var oEmpresa = new NEmpresa())
            {
                empresa.EstadoEmpresa = EstadoEmpresa.Inactivo;
                var respuesta = oEmpresa.EliminarEmpresa(empresa);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

	}
}