using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;

namespace Gdoc.Web.Controllers
{
    public class DocumentosRecibidosController : Controller
    {
        //
        // GET: /DocumentosRecibidos/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarOperacion()
        {
            try
            {
                var listOperacion = new List<EOperacion>();
                using (var oOperacion = new NOperacion())
                {
                    listOperacion = oOperacion.ListarDocumentosRecibidos(new UsuarioParticipante
                    {
                        IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                    }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico || x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital).ToList();
                }
                return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public JsonResult ListarDocumentoPDF(EOperacion operacion)
        {
            try
            {
                var extension = operacion.DocumentoDigitalOperacion.NombreOriginal;
                var n = extension.LastIndexOf(".");
                var ext = extension.Substring(n);
                //var ruta = Session["RutaGdocPDF"].ToString().Substring(1)+"" + operacion.NumeroOperacion + ".pdf";
                var ruta = "http://192.168.100.29:85/PDF/" + operacion.NumeroOperacion + ext;
                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public JsonResult ListarDocumentoAdjunto(Operacion operacion)
        {
            var listDocumentoAdjunto = new List<EDocumentoAdjunto>();
            using (var oDocumentoAdjunto = new NDocumentoAdjunto())
            {
                listDocumentoAdjunto = oDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listDocumentoAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}