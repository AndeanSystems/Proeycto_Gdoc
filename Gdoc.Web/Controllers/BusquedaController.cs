using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;
namespace Gdoc.Web.Controllers
{
    public class BusquedaController : Controller
    {

        #region "Variables"
        private MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        private NUsuarioParticipante nUsuarioParticipante = new NUsuarioParticipante();
        private NIndexacionDocumento nIndexacionDocumento = new NIndexacionDocumento();
        #endregion
        // GET: /Busqueda/
        public ActionResult Index()
        {
            if (Session["ListaAccesos"] == null)
                return RedirectToAction("Index", "Home");

            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 5 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult ListarOperacionBusqueda(EOperacion operacion, IndexacionDocumento indexacion)
        {
            var listOperacion = new List<EOperacion>();
            var listIndexacion = new List<IndexacionDocumento>();
            using (var oOperacion = new NOperacion())
            {

                //var tipoOepracion = operacion.TipoOperacion;
                var fechaDesde = operacion.FechaEmision;
                var fechaHasta = operacion.FechaRegistro;

                if (operacion.TipoOperacion == null)
                    operacion.TipoOperacion = "";
                if (operacion.TipoDocumento == null)
                    operacion.TipoDocumento = "";
                if (indexacion.DescripcionIndice == null)
                    indexacion.DescripcionIndice = "";

                listOperacion=oOperacion.ListarOperacionBusquedaTotal(operacion, indexacion);

                
                //if (operacion.TipoOperacion != null)
                //{
                //    if (operacion.TipoDocumento != null)
                //    {
                //        if (indexacion.DescripcionIndice != null)
                //        {
                //            listOperacion = oOperacion.ListarOperacionBusqueda().
                //                Where(x => x.TipoOperacion == operacion.TipoOperacion 
                //                    && x.TipoDocumento == operacion.TipoDocumento
                //                    && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).OrderByDescending(x => x.FechaEnvio).ToList();


                //        }
                //        else
                //        {
                //            listOperacion = oOperacion.ListarOperacionBusqueda().
                //                Where(x => x.TipoOperacion == operacion.TipoOperacion 
                //                    && x.TipoDocumento == operacion.TipoDocumento
                //                    && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).OrderByDescending(x=>x.FechaEnvio).ToList();

                //        }
                        
                //    }
                //    else
                //    {
                //        listOperacion = oOperacion.ListarOperacionBusqueda().
                //            Where(x => x.TipoOperacion == operacion.TipoOperacion && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).OrderByDescending(x => x.FechaEnvio).ToList();
                //    }
                //}
                //else
                //{
                //    listOperacion = oOperacion.ListarOperacionBusqueda().Where(x => x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).OrderByDescending(x => x.FechaEnvio).ToList();
                //}

                Session["listBusqueda"] = listOperacion;
                
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarDocumentoPDF(EOperacion operacion)
        {
            try
            {
                var ruta = "vacio";
                string sWebSite = ConfigurationManager.AppSettings.Get("Documentos");
                if (operacion.AccesoOperacion == "1 ")
                {
                    var listparticipantes = nUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                    foreach (var item in listparticipantes)
                    {
                        if (item.IDUsuario == Convert.ToInt32(Session["IDUsuario"]))
                            ruta = sWebSite + operacion.NombreFinal;
                    }
                }
                else
                    ruta = sWebSite + operacion.NombreFinal;

                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult ListarAdjunto(Operacion operacion)
        {
            var listAdjunto = new List<EAdjunto>();
            using (var oAdjunto = new NAdjunto())
            {
                if (operacion.AccesoOperacion == "1 ")
                {
                    var listparticipantes = nUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                    foreach (var item in listparticipantes)
                    {
                        if (item.IDUsuario == Convert.ToInt32(Session["IDUsuario"]))
                        {
                            listAdjunto = oAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion && x.EstadoAdjunto == Estados.EstadoAdjunto.Activo).ToList(); 
                            return new JsonResult { Data = listAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
                        }
                   }
                }
                else
                {
                    listAdjunto = oAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion && x.EstadoAdjunto == Estados.EstadoAdjunto.Activo).ToList();
                    return new JsonResult { Data = listAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
                }
                   
            }
            return new JsonResult { Data = "vacio", JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult DescargarExcel()
        {
            try
            {
                List<EListaExcel> listOperacion = new List<EListaExcel>();

                List<EOperacion> operacion = (List<EOperacion>)Session["listBusqueda"];
                foreach (var item in operacion)
                {
                    var listaExcel = new EListaExcel();
                    listaExcel.IDEmpresa = item.IDEmpresa;
                    listaExcel.TipoOperacion = item.TipoOperacion;
                    listaExcel.FechaEmision = item.FechaEmision;
                    listaExcel.NumeroOperacion = item.NumeroOperacion;
                    listaExcel.TituloOperacion = item.TituloOperacion;
                    listaExcel.EstadoOperacion = item.EstadoOperacion;
                    listaExcel.FechaRegistro = item.FechaRegistro;
                    listaExcel.FechaEnvio = item.FechaEnvio;
                    listaExcel.FechaVigente = item.FechaVigente;
                    listaExcel.FechaCierre = item.FechaCierre;
                    listaExcel.TipoDocumento = item.TipoDocumento;
                    listaExcel.NombreFinal = item.NombreFinal;
                    listaExcel.TipoDoc = item.TipoDoc.DescripcionConcepto;
                    listaExcel.TipoOpe = item.TipoOpe.DescripcionConcepto;
                    listaExcel.Estado = item.Estado.DescripcionConcepto;

                    listOperacion.Add(listaExcel);
                }

                Excel converter = new Excel();
                DataTable dt = converter.ToDataTable(listOperacion);

                string attachment = "attachment; filename=ReporteBusqueda-"+System.DateTime.Now.ToShortDateString().ToString()+".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";


                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName.ToLower());
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Index", "Busqueda"); 
        }

	}
}