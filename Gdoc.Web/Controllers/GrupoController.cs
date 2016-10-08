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
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 13 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
                //return View("../Alertas/Index");
                return RedirectToAction("Index", "Blanco");
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
        public JsonResult ListarUsuarioGrupo(Grupo grupo)
        {
            var listUsuarioGrupo = new List<EUsuarioGrupo2>();
            try
            {
                using (var oUsuarioGrupo = new NUsuarioGrupo())
                {
                    listUsuarioGrupo = oUsuarioGrupo.listarUsuarioG().Where(x => x.IDGrupo == grupo.IDGrupo && x.EstadoUsuarioGrupo == Constantes.EstadoParticipante.Activo).ToList();
                }
                return new JsonResult { Data = listUsuarioGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        public JsonResult GrabarGrupoUsuarios(Grupo grupo, List<UsuarioGrupo> listUsuarioGrupo)
        {
            try
            {
                using (var oGrupo = new NGrupo())
                {
                    var listEusuarioGrupo = new List<UsuarioGrupo>();
                    var oUsuarioGrupo = new NUsuarioGrupo();

                    grupo.FechaModifica = System.DateTime.Now;
                    grupo.UsuarioModifica = Session["NombreUsuario"].ToString();
                    //grupo.EstadoGrupo = 1;

                    //Grupo respuesta = null;
                    short respuesta = 1;

                    if (grupo.IDGrupo > 0)
                    {
                        oGrupo.EditarGrupo(grupo);

                        var ugrupoguardados = oUsuarioGrupo.listarUsuarioG().Where(x => x.IDGrupo == grupo.IDGrupo && x.EstadoUsuarioGrupo == 1);
                        var usuariosactuales = listUsuarioGrupo.Select(x => x.IDUsuarioGrupo).ToList();
                        IEnumerable<UsuarioGrupo> nuevos = ugrupoguardados.Where(x => !usuariosactuales.Contains(x.IDUsuarioGrupo));
                        foreach (var item in nuevos)
                        {
                            if (item.EstadoUsuarioGrupo != 0)
                            {
                                item.EstadoUsuarioGrupo = 2;
                                oUsuarioGrupo.EditarUsuarioGrupo(item);
                            }
                        }
                        foreach (var participante in listUsuarioGrupo)
                        {
                            var eUsuarioGrupo = new UsuarioGrupo();

                            var usuarioencontrado = oUsuarioGrupo.listarUsuarioG().Where(x => x.IDUsuarioGrupo == participante.IDUsuarioGrupo).FirstOrDefault();

                            if (usuarioencontrado != null)
                            {
                                usuarioencontrado.EstadoUsuarioGrupo = 1;
                                oUsuarioGrupo.EditarUsuarioGrupo(usuarioencontrado);
                            }
                            else 
                            {
                                if (participante.EstadoUsuarioGrupo != 1 && participante.EstadoUsuarioGrupo != 2)
                                {
                                    eUsuarioGrupo.IDUsuario = participante.IDUsuarioGrupo;
                                    eUsuarioGrupo.IDGrupo = grupo.IDGrupo;
                                    eUsuarioGrupo.UsuarioRegistro = Session["NombreUsuario"].ToString();
                                    eUsuarioGrupo.FechaRegistro = System.DateTime.Now;
                                    eUsuarioGrupo.EstadoUsuarioGrupo = 1;
                                    listEusuarioGrupo.Add(eUsuarioGrupo);
                                }
                            }
                        }
                        oUsuarioGrupo.GrabarUsuarioGrupo(listEusuarioGrupo);
                    }
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