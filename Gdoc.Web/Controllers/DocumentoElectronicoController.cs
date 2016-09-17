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
                //FALTA TERMINAR QUITAR VALORES EN DURO
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;

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
                    
                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";

                //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                using (var oNOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                    var respuesta = oNOperacion.Grabar(operacion,listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario, null);
                }
                mensajeRespuesta.Mensaje = "Operación realizado correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = "Operación no realizado correctamente";
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
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString*()(,
                }).Where(x => x.TipoOperacion == Constants.TipoOperacion.DocumentoElectronico).ToList();
            }
            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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