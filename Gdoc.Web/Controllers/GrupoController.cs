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
    public class GrupoController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Grupo/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarGrupo()
        {
            var listGrupo = new List<Grupo>();
            using (var oGrupo = new NGrupo())
            {
                listGrupo = oGrupo.ListarGrupo();
            }
            return new JsonResult { Data = listGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarGrupoUsuarios(Grupo grupo)
        {
            using (var oGrupo = new NGrupo())
            {
                grupo.FechaModifica = System.DateTime.Now;
                grupo.UsuarioModifica = Session["NombreUsuario"].ToString(); 
                grupo.EstadoGrupo = 1;
                var respuesta = oGrupo.GrabarGrupoUsuarios(grupo);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
            }
            //RedirectToAction("Index", "Grupo"); falta terminar redireccionar la pagina
            return new JsonResult { Data = mensajeRespuesta };

        }
	}
}