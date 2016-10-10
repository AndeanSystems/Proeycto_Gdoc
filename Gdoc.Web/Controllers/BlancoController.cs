using Gdoc.Entity.Extension;
using Gdoc.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class BlancoController : Controller
    {
        //
        // GET: /Blanco/
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

    }
}