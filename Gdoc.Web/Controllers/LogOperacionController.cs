using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;


namespace Gdoc.Web.Controllers
{
    public class LogOperacionController : Controller
    {
        //
        public ActionResult Index()
        {
            if (Session["ListaAccesos"] == null)
                return RedirectToAction("Index", "Home");

            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 6 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult ListarLogOperacion(Operacion operacion)
        {
            try
            {
                var listLogOperacion = new List<ELogOperacion>();
                using (var oLogOperacion = new NLogOperacion())
                {
                    listLogOperacion = oLogOperacion.ListarLogOperacion()
                        .Where(x=>x.Operacion.TipoOperacion==operacion.TipoOperacion
                            && x.Operacion.TipoDocumento == operacion.TipoDocumento
                            && x.Operacion.NumeroOperacion == operacion.NumeroOperacion
                            ).OrderByDescending(x => x.FechaEvento).ToList();
                }
                Session["listLogOperacion"] = listLogOperacion;
                return new JsonResult { Data = listLogOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult ListarLogOperacionPorFechas(Nullable<System.DateTime> fecha)
        {
            try
            {
                var listLosOperacion = new List<ELogOperacion>();
                using (var nLogOperacion = new NLogOperacion())
                {
                    listLosOperacion = nLogOperacion.ListarLogOperacion().
                        Where(x => Convert.ToDateTime(x.FechaEvento).ToString("dd/MM/yyyy") == Convert.ToDateTime(fecha).ToString("dd/MM/yyyy")).
                         OrderByDescending(x => x.FechaEvento).ToList();
                }
                return new JsonResult { Data = listLosOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public ActionResult DescargarExcel()
        {
            try
            {
                List<EListaExcelLog> listLogOperacion = new List<EListaExcelLog>();

                List<ELogOperacion> logoperacion = (List<ELogOperacion>)Session["listLogOperacion"];
                foreach (var item in logoperacion)
                {
                    var listaExcel = new EListaExcelLog();
                    listaExcel.IDLogOperacion = item.IDLogOperacion;
                    listaExcel.NumeroOperacion = item.Operacion.NumeroOperacion;
                    listaExcel.NombreUsuario = item.Usuario.NombreUsuario;
                    listaExcel.FechaEvento = item.FechaEvento;
                    listaExcel.Evento = item.Evento.DescripcionConcepto;

                    listLogOperacion.Add(listaExcel);
                }

                Excel converter = new Excel();
                DataTable dt = converter.ToDataTable(listLogOperacion);

                string attachment = "attachment; filename=ReporteLogOperacion-"+listLogOperacion.FirstOrDefault().NumeroOperacion+"-" + System.DateTime.Now.ToShortDateString().ToString() + ".xls";
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

            return RedirectToAction("Index", "LogOperacion");
        }
	}
}