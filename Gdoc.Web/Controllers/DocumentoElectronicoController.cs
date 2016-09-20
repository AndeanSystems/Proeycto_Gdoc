using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Negocio.Utils;

namespace Gdoc.Web.Controllers
{
    public class DocumentoElectronicoController : Controller
    {
        #region "Variables"
        private MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        //
        // GET: /DocumentoElectronico/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo) {
            try
            {
                using (var oNOperacion = new NOperacion())
                {
                    Int32 respuesta = 0;
                    if (operacion.IDOperacion > 0)
                    {
                        if (operacion.EstadoOperacion == 1)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                        }
                        else
                        {
                            //NO ESTOY SEGURO
                            //operacion.FechaRegistro = DateTime.Now;
                            //operacion.FechaEmision = DateTime.Now;
                        }
                        respuesta = oNOperacion.EditarOperacion(operacion,eDocumentoElectronicoOperacion);
                    }
                    else
                    {
                        //FALTA TERMINAR QUITAR VALORES EN DURO
                        operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                        operacion.TipoOperacion = Gdoc.Web.Util.Constantes.TipoOperacion.DocumentoElectronico;

                        if (operacion.EstadoOperacion == 1)
                        {
                            if (operacion.FechaRegistro == null)
                            {
                                operacion.FechaRegistro = DateTime.Now;
                                operacion.FechaEmision = DateTime.Now;
                            }
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                        }
                        else
                        {
                            operacion.FechaRegistro = DateTime.Now;
                            operacion.FechaEmision = DateTime.Now;
                        }

                        operacion.NumeroOperacion = "DE" + DateTime.Now.Ticks.ToString();
                        operacion.NotificacionOperacion = "S";

                        if (listDocumentosAdjuntos != null) 
                            operacion.DocumentoAdjunto = "S";
                        else
                            operacion.DocumentoAdjunto = "N";

                        //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                        //using (var oNOperacion = new NOperacion())
                        //{

                        Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                        respuesta = oNOperacion.Grabar(operacion, listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario);
                    }
                    
                }
                mensajeRespuesta.Mensaje = "Operación "+operacion.NumeroOperacion+" realizada correctamente";
                mensajeRespuesta.Exitoso = true;
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
            var listDocumentoElectronico = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listDocumentoElectronico = oOperacion.ListarOperacionElectronico(new UsuarioParticipante {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                }).Where(x => x.TipoOperacion == Gdoc.Web.Util.Constantes.TipoOperacion.DocumentoElectronico).ToList();
            }
            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante= new List<UsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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