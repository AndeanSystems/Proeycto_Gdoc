using Gdoc.Entity.Extension;
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
            var listGrupo = new List<EGrupo>();
            using (var oGrupo = new NGrupo())
            {
                listGrupo = oGrupo.ListarGrupo().OrderBy(x => x.NombreGrupo).ToList();
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

                Grupo respuesta = null;
                if (grupo.IDGrupo > 0)
                    respuesta = oGrupo.EditarGrupo(grupo);
                else
                    respuesta = oGrupo.GrabarGrupoUsuarios(grupo);

                //var respuesta = oGrupo.GrabarGrupoUsuarios(grupo);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
            }
            //RedirectToAction("Index", "Grupo"); falta terminar redireccionar la pagina
            return new JsonResult { Data = mensajeRespuesta };

        }
        public JsonResult EliminarGrupo(Grupo grupo)
        {
            using (var oGrupo = new NGrupo())
            {
                grupo.EstadoGrupo = Gdoc.Web.Util.Estados.EstadoEmpresa.Inactivo;
                var respuesta = oGrupo.EliminarGrupo(grupo);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitoso";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }
        [HttpPost]
        public JsonResult BuscarGrupo(EGrupo grupo)
        {
            var listConcepto = new List<EGrupo>();
            using (var oConcepto = new NGrupo())
            {
                listConcepto = oConcepto.ListarGrupo().Where(x => x.NombreGrupo == grupo.NombreGrupo).ToList();
                //listConceptoRetorno.ForEach(x => listConcepto.Add(x));
            }
            return new JsonResult { Data = listConcepto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}