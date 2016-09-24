using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;

namespace Gdoc.Web.Controllers
{
    public class AlertasController : Controller
    {
        //
        // GET: /Alertas/

        public ActionResult Index()
        {
            using (var NUsuario = new NUsuario())
            {
                var usuarioe = new EUsuario() { IDUsuario = Convert.ToInt32(Session["IDUsuario"]), NombreUsuario = Session["NombreUsuario"].ToString() };


                var CantidadAlerta = NUsuario.CantidadAlerta(usuarioe);
                var CantidadDocumentosRecibidos = NUsuario.CantidadDocumentosRecibidos(usuarioe);
                var CantidadMesaVirtual = NUsuario.CantidadMesaVirtual(usuarioe);
                //CONTADORES
                if (CantidadAlerta != null) Session["CantidadAlerta"] = CantidadAlerta.CantidadAlerta;
                else Session["CantidadAlerta"] = 0;
                //---
                if (CantidadDocumentosRecibidos != null) Session["CantidadDocumentosRecibidos"] = CantidadDocumentosRecibidos.CantidadDocumentosRecibidos;
                else Session["CantidadDocumentosRecibidos"] = 0;
                //---
                if (CantidadMesaVirtual != null) Session["CantidadMesaVirtual"] = CantidadMesaVirtual.CantidadMesasVirtual;
                else Session["CantidadMesaVirtual"] = 0;
                return View();
            }
            
        }
        [HttpGet]
        public JsonResult ListarMensajeAlerta()
        {
            var listMensajeAlerta = new List<EMensajeAlerta>();
            using (var oMensajeAlerta = new NMensajeAlerta())
            {
                Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                listMensajeAlerta = oMensajeAlerta.ListarMensajeAlerta(IDusuario);
            }
            return new JsonResult { Data = listMensajeAlerta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult Alertas()
        {
            return View();
        }

        public ActionResult DocumentosRecibidos()
        {
            return View();
        }

        public ActionResult MesaTrabajoVirtual()
        {
            return View();
        }
    }
}
