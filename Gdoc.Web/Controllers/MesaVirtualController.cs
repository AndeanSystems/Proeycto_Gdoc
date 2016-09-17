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
        // GET: /MesaVirtual/
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

                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";

                //eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                using (var oNOperacion = new NOperacion())
                {
                    var respuesta = oNOperacion.GrabarMesaVirtual(operacion, listEUsuarioGrupo);
                }
                return new JsonResult { Data = null, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult ListarOperacion()
        {
            var listMesaVirtual = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listMesaVirtual = oOperacion.ListarMesaVirtual().Where(x => x.TipoOperacion == Constantes.TipoOperacion.MesaVirtual).ToList();
            }
            return new JsonResult { Data = listMesaVirtual, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
	}
}