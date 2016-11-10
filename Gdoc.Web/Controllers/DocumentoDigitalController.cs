using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
using System.IO;
using System.Drawing;

namespace Gdoc.Web.Controllers
{
    public class DocumentoDigitalController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /DocumentoDigital/
        public ActionResult Index()
        {
            if (Session["ListaAccesos"] == null)
                return RedirectToAction("Index", "Home");

            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 3 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult Grabar(Operacion operacion, List<DocumentoDigitalOperacion> listDocumentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<IndexacionDocumento> listIndexacion)
        {
            
            try
            {
                using (var oOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    if (operacion.IDOperacion > 0)
                    {
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                        }

                        oOperacion.EditarDocumentoDigital(operacion, listDocumentoDigitalOperacion, listEUsuarioGrupo, listIndexacion, IDusuario);
                    }
                    else
                    {
                        operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                        operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital; 
                        operacion.NotificacionOperacion = "S";//FALTA
                        operacion.NumeroOperacion = oOperacion.NumeroOperacion(IDusuario, Constantes.TipoOperacion.DocumentoDigital, operacion.TipoDocumento, Convert.ToInt32(Session["IDEmpresa"]));
                        //operacion.NumeroOperacion = "DD" + DateTime.Now.Ticks.ToString();
                        operacion.DocumentoAdjunto = "S";

                        if (operacion.EstadoOperacion == 1)
                        {
                            operacion.FechaRegistro = DateTime.Now;
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                        }
                        else
                            operacion.FechaRegistro = DateTime.Now;

                        oOperacion.GrabarDocumentoDigital(operacion, listDocumentoDigitalOperacion, listEUsuarioGrupo, listIndexacion, IDusuario);

                    }
                    mensajeRespuesta.Exitoso = true;
                    if(operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                        mensajeRespuesta.Mensaje = "La operación " + operacion.NumeroOperacion + " se envió correctamente";
                    else
                        mensajeRespuesta.Mensaje = "La operacion " + operacion.NumeroOperacion + " se grabó correctamente";
                    return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
                }
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = ex.Message;
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
        public JsonResult ListarOperacion()
        {
            var listOperacion = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listOperacion = oOperacion.ListarOperacionDigital(new UsuarioParticipante
                {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital).OrderByDescending(x => x.FechaRegistro).ToList();
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarReferencia(Operacion operacion)
        {
            var listReferencia = new List<IndexacionDocumento>();
            try
            {
                using(var oIndexacion = new NIndexacionDocumento())
                {
                    listReferencia = oIndexacion.ListarIndexacionDocumento().Where(x=>x.IDOperacion==operacion.IDOperacion && x.EstadoIndice==1).ToList();
                }
                return new JsonResult { Data = listReferencia, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult EliminarOperacion(Operacion operacion)
        {
            try
            {
                Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                using (var oOperacion = new NOperacion())
                {
                    operacion.EstadoOperacion = Estados.EstadoOperacion.Inactivo;
                    var respuesta = oOperacion.AnularDocumentoDigital(operacion, IDusuario);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Documento Digital Inactivo";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult ListarDocumentoDigitalOperacion(Operacion operacion)
        {
            var listDocumentoDigitalOperacion= new List<DocumentoDigitalOperacion>();
            using (var oDocumentoDigitalOperacion = new NDocumentoDigitalOperacion())
            {
                listDocumentoDigitalOperacion = oDocumentoDigitalOperacion.ListarDocumentoDigitalOperacion().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listDocumentoDigitalOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        #region Metodos
        protected DateTime DateAgregarLaborales(Int32 add, DateTime FechaInicial)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday) { FechaInicial = FechaInicial.AddDays(2); }
            if (FechaInicial.DayOfWeek == DayOfWeek.Sunday) { FechaInicial = FechaInicial.AddDays(1); }
            Int32 weeks = add / 5;
            add += weeks * 2;
            if (FechaInicial.DayOfWeek > FechaInicial.AddDays(add).DayOfWeek)
                add += 2;

            if (FechaInicial.AddDays(add).DayOfWeek == DayOfWeek.Saturday)
                add += 2;

            return FechaInicial.AddDays(add);
        }
        #endregion
	}
}