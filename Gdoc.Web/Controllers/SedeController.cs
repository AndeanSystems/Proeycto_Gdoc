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
    public class SedeController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Sede/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarSede()
        {
            var listSede = new List<Sede>();
            using (var oSede = new NSede())
            {
                listSede = oSede.ListarSede();
            }
            return new JsonResult { Data = listSede, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarSede(Empresa empresa)
        {
            var listSede = new List<Sede>();
            using (var oSede = new NSede())
            {
                listSede = oSede.ListarSede().Where(x=>x.IDEmpresa==empresa.IDEmpresa).ToList();
            }
            return new JsonResult { Data = listSede, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarSede(Sede sede)
        {
            try
            {
                using (var oSede = new NSede())
                {
                    sede.FechaModifica = System.DateTime.Now;
                    sede.UsuarioModifica = Session["NombreUsuario"].ToString();

                    if (sede.IDSede > 0){
                        oSede.EditarSede(sede);
                        mensajeRespuesta.Mensaje = "Actualizacion Exitoso";
                    }
                        
                    else{
                        oSede.GrabarSede(sede);
                        mensajeRespuesta.Mensaje = "Grabación Exitoso";
                    }
                        
                    mensajeRespuesta.Exitoso = true;


                    return new JsonResult { Data = mensajeRespuesta };
                }
            }
            catch (Exception)
            {
                mensajeRespuesta.Exitoso = false;
                mensajeRespuesta.Mensaje = "Erroe en la Grabacion";
                return new JsonResult { Data = mensajeRespuesta };
                throw;
            }
        }
        public JsonResult EliminarSede(Sede sede)
        {
            using (var oSede = new NSede())
            {
                sede.EstadoSede = Estados.EstadoEmpresa.Inactivo;
                oSede.EliminarSede(sede);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Eliminacion Exitosa";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
	}
}