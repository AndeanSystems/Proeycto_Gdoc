using Gdoc.Common.Utilitario;
using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
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
        [HttpPost]
        public JsonResult ListarUsuarioGrupo(Grupo grupo)
        {
            var listUsuarioGrupo = new List<UsuarioGrupo>();
            try
            {
                using (var oUsuarioGrupo = new NUsuarioGrupo())
                {
                    listUsuarioGrupo = oUsuarioGrupo.listarUsuarioG().Where(x => x.IDGrupo == grupo.IDGrupo).ToList();
                }
                return new JsonResult { Data = listUsuarioGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        public JsonResult GrabarGrupoUsuarios(Grupo grupo, List<EUsuarioGrupo> listUsuarioGrupo)
        {
            try
            {
                using (var oGrupo = new NGrupo())
                {
                    var listEusuarioGrupo = new List<UsuarioGrupo>();
                    var oUsuarioGrupo = new NUsuarioGrupo();

                    grupo.FechaModifica = System.DateTime.Now;
                    grupo.UsuarioModifica = Session["NombreUsuario"].ToString();
                    grupo.EstadoGrupo = 1;

                    //Grupo respuesta = null;
                    Grupo respuesta2;
                    short respuesta = 1;

                    if (grupo.IDGrupo > 0)
                        respuesta2 = oGrupo.EditarGrupo(grupo);
                    else
                    {
                        respuesta = oGrupo.GrabarGrupoUsuarios(grupo);

                        foreach (var participante in listUsuarioGrupo)
                        {
                            var eUsuarioGrupo = new UsuarioGrupo();

                            eUsuarioGrupo.IDUsuario = participante.IDUsuarioGrupo;
                            eUsuarioGrupo.IDGrupo = grupo.IDGrupo;
                            eUsuarioGrupo.UsuarioRegistro = Session["NombreUsuario"].ToString();
                            eUsuarioGrupo.FechaRegistro = System.DateTime.Now;
                            eUsuarioGrupo.EstadoUsuarioGrupo = 1;
                            listEusuarioGrupo.Add(eUsuarioGrupo);

                        }

                        oUsuarioGrupo.GrabarUsuarioGrupo(listEusuarioGrupo);
                    }

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
        public JsonResult EliminarGrupo(Grupo grupo)
        {
            using (var oGrupo = new NGrupo())
            {
                grupo.EstadoGrupo = Estados.EstadoEmpresa.Inactivo;
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
            }
            return new JsonResult { Data = listConcepto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}