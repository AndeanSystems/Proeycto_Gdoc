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
namespace Gdoc.Web.Controllers
{
    public class BusquedaController : Controller
    {
        #region "Variables"
        private MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: /Busqueda/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListarOperacionBusqueda(Operacion operacion)
        {
            var listOperacion = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listOperacion = oOperacion.ListarOperacionBusqueda().
                    Where(x=>x.TipoOperacion==operacion.TipoOperacion || x.TipoDocumento==operacion.TipoDocumento 
                            || x.FechaRegistro==operacion.FechaRegistro || x.FechaCierre==operacion.FechaCierre).ToList();
            }
            return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        public JsonResult ListToExcel(List<Operacion> operacion)
        {
            try
            {
                Excel converter = new Excel();
                DataTable dt = converter.ToDataTable(operacion);

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
	}
}