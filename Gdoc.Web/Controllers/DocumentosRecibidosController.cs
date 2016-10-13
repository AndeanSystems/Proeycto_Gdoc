﻿using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
using System.Configuration;

namespace Gdoc.Web.Controllers
{
    public class DocumentosRecibidosController : Controller
    {
        //
        // GET: /DocumentosRecibidos/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x => x.IDModuloPagina == 19 && x.EstadoAcceso == 1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
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
                    }).Where(x => x.EstadoOperacion==Estados.EstadoOperacion.Activo && (x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico || x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital)).ToList();
                }
                return new JsonResult { Data = listOperacion, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante = new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarOperacionPorFecha(Nullable<System.DateTime> fecha)
        {
            try
            {
                DateTime a123 = Convert.ToDateTime(fecha);
                var listOperacion = new List<EOperacion>();
                using (var oOperacion = new NOperacion())
                {
                    listOperacion = oOperacion.ListarDocumentosRecibidos(new UsuarioParticipante
                    {
                        IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                    }).Where(x => x.EstadoOperacion == Estados.EstadoOperacion.Activo &&
                        (x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico || x.TipoOperacion == Constantes.TipoOperacion.DocumentoDigital) && Convert.ToDateTime(x.FechaEnvio).ToString("dd/MM/yyyy") == Convert.ToDateTime(fecha).ToString("dd/MM/yyyy")).ToList();
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
                string sWebSite = ConfigurationManager.AppSettings.Get("Documentos");
                var ruta = sWebSite + operacion.NombreFinal;

                return new JsonResult { Data = ruta, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult ListarAdjuntos(EAdjunto adjunto)
        {
            try
            {

                string sWebSite = ConfigurationManager.AppSettings.Get("Adjunto");

                var ruta = sWebSite + adjunto.Archivo;
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
                listDocumentoAdjunto = oDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoDoctoAdjunto==Estados.EstadoAdjunto.Activo).ToList();
            }
            return new JsonResult { Data = listDocumentoAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarAdjunto(Operacion operacion)
        {
            var listAdjunto = new List<EAdjunto>();
            using (var oAdjunto = new NAdjunto())
            {
                listAdjunto = oAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion && x.EstadoAdjunto == Estados.EstadoAdjunto.Activo).ToList();
            }
            return new JsonResult { Data = listAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}