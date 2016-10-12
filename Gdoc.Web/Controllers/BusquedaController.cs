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
        #endregion
        // GET: /Busqueda/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 5 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult ListarOperacionBusqueda(Operacion operacion)
        {
            var listOperacion = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {

                //var tipoOepracion = operacion.TipoOperacion;
                var fechaDesde = operacion.FechaEmision;
                var fechaHasta = operacion.FechaRegistro;

                if (operacion.TipoOperacion != null)
                {
                    if (operacion.TipoDocumento != null)
                    {
                        //if (operacion.TituloOperacion!=null )
                        //{
                        //    listOperacion = oOperacion.ListarOperacionBusqueda().
                        //        Where(x => x.TipoOperacion == operacion.TipoOperacion 
                        //            && x.TipoDocumento == operacion.TipoDocumento 
                        //            && x.TituloOperacion.Contains(operacion.TituloOperacion)
                        //            && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).ToList();
                        //}
                        //else
                        //{
                            listOperacion = oOperacion.ListarOperacionBusqueda().
                                Where(x => x.TipoOperacion == operacion.TipoOperacion 
                                    && x.TipoDocumento == operacion.TipoDocumento
                                    && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).ToList();

                        //}
                        
                    }
                    else
                    {
                        listOperacion = oOperacion.ListarOperacionBusqueda().
                            Where(x => x.TipoOperacion == operacion.TipoOperacion && x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).ToList();
                    }
                }
                else
                {
                    listOperacion = oOperacion.ListarOperacionBusqueda().Where(x => x.FechaRegistro >= fechaDesde && x.FechaRegistro <= fechaHasta).ToList();
                }

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
        [HttpGet]
        public virtual ActionResult Download(string file)
        {
            string fullPath = Path.Combine(Server.MapPath("~/MyFiles"), file);
            return File(fullPath, "application/vnd.ms-excel", file);
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
        public JsonResult ListToExcel(List<Operacion> operacion)
        {
            try
            {
                List<EListaExcel> listOperacion = new List<EListaExcel>();

                foreach (var item in operacion)
                {
                    var listaExcel=new EListaExcel();
                    listaExcel.IDEmpresa = item.IDEmpresa;
                    listaExcel.TipoOperacion = item.TipoOperacion;


                    listOperacion.Add(listaExcel);
                }

                Excel converter = new Excel();
                DataTable dt = converter.ToDataTable(listOperacion);

                string attachment = "attachment; filename=Resultado.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
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

                mensajeRespuesta.Mensaje = "Se exporto Correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = ex.Message;
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
           
        }

        public class Export
        {
            public void ToExcel(HttpResponseBase Response, object clientsList)
            {
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = clientsList;
                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=FileName.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
	}
}