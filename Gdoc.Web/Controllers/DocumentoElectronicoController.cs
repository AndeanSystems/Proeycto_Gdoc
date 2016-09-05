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
    public class DocumentoElectronicoController : Controller
    {
        //
        // GET: /DocumentoElectronico/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo) {
            try
            {
                operacion.IDEmpresa = 1001;//Falta terminar
                operacion.TituloOperacion = "03";
                operacion.FechaEmision = DateTime.Now;
                operacion.FechaCierre = DateTime.Now;
                operacion.FechaVigente = DateTime.Now;
                operacion.FechaEnvio = DateTime.Now;
                operacion.FechaRegistro = DateTime.Now;
                operacion.NumeroOperacion = DateTime.Now.Ticks.ToString();
                operacion.NotificacionOperacion = "S";
                operacion.DocumentoAdjunto = "N";
                operacion.EstadoOperacion = "0";
                operacion.TituloOperacion = "Documento Electronico";
                operacion.DescripcionOperacion = "Documento Electronico";
                using (var oNOperacion = new NOperacion())
                {
                    var respuesta = oNOperacion.Grabar(operacion, eDocumentoElectronicoOperacion, listEUsuarioGrupo);
                }
                return new JsonResult { Data = null, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
	}
}