using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
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
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion, List<DocumentoDigitalOperacion> listDocumentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<IndexacionDocumento> listIndexacion)
        {
            try
            {
                //FALTA TERMINAR
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                operacion.NumeroOperacion ="DD"+ DateTime.Now.Ticks.ToString();//FALTA

                
                

                if (operacion.EstadoOperacion == 1)
                {
                    if (operacion.FechaRegistro == null)
                        operacion.FechaRegistro = DateTime.Now;
                    operacion.FechaEnvio = DateTime.Now;
                    operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                }
                else
                    operacion.FechaRegistro = DateTime.Now;

                operacion.NotificacionOperacion = "S";//FALTA
                operacion.DocumentoAdjunto = "N";//FALTA

                using (var oOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oOperacion.GrabarDocumentoDigital(operacion, listDocumentoDigitalOperacion, listEUsuarioGrupo, listIndexacion, IDusuario);
                    
                }
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Operación " + operacion.NumeroOperacion + " realizada correctamente";
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = "Operación no realizada correctamente";
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
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital).ToList();
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult EliminarOperacion(Operacion operacion)
        {
            using (var oOperacion = new NOperacion())
            {
                operacion.EstadoOperacion = Estados.EstadoOperacion.Inactivo;
                var respuesta = oOperacion.EliminarOperacion(operacion);
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
            }
            return new JsonResult { Data = mensajeRespuesta };
        }

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
	}
}