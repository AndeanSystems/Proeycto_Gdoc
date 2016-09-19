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
    public class MesaVirtualController : Controller
    {
        //
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion, List<EUsuarioGrupo> listEUsuarioGrupo)
        {
            try
            {
                //FALTA TERMINAR QUITAR VALORES EN DURO
                operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                operacion.TipoOperacion = Constantes.TipoOperacion.MesaVirtual;


                if (operacion.EstadoOperacion == 1)
                {
                    if (operacion.FechaRegistro == null)
                        operacion.FechaRegistro = DateTime.Now;
                    operacion.FechaEnvio = DateTime.Now;
                }
                else
                    operacion.FechaRegistro = DateTime.Now;

                operacion.NumeroOperacion = "MV"+DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";

                //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                using (var oNOperacion = new NOperacion())
                {
                    var respuesta = oNOperacion.GrabarMesaVirtual(operacion, listEUsuarioGrupo);
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
            var listMesaVirtual = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listMesaVirtual = oOperacion.ListarMesaVirtual(new UsuarioParticipante 
                { 
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString())
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();
            }
            return new JsonResult { Data = listMesaVirtual, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}