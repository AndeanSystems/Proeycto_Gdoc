using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Common.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace Gdoc.Web.Controllers
{
    public class DocumentoElectronicoController : Controller
    {
        #region "Variables"
        private MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        private NUsuario nUsuario = new NUsuario();
        private NPersonal nPersonal = new NPersonal();
        private NOperacion nOperacion = new NOperacion();
        #endregion
        //
        // GET: /DocumentoElectronico/
        public ActionResult Index()
        {
            var listAcceso = ((List<AccesoSistema>)Session["ListaAccesos"]).Where(x=>x.IDModuloPagina==2 && x.EstadoAcceso==1).FirstOrDefault();

            if (listAcceso != null)
                return View();
            else
            {
                TempData["Message"] = 1;
                return RedirectToAction("Index", "Blanco");
            }
        }
        public JsonResult Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo) {
            try
            {
                var remitentes = new List<string>();
                var destinatarios = new List<string>();
                using (var oNOperacion = new NOperacion())
                {
                    Int64 IDusuario = Convert.ToInt64(Session["IDUsuario"]);
                   
                    if (operacion.IDOperacion > 0)
                    {
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                            operacion.NombreFinal = operacion.NumeroOperacion + ".pdf";
                        }

                        oNOperacion.EditarDocumentoElectronico(operacion, listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario);

                        GenerarPdfDatos(oNOperacion,operacion,eDocumentoElectronicoOperacion,listEUsuarioGrupo);
                    }
                    else
                    {
                        operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                        operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;

                        operacion.NumeroOperacion = oNOperacion.NumeroOperacion(IDusuario, Constantes.TipoOperacion.DocumentoElectronico);
                        //operacion.NumeroOperacion = "DE" + DateTime.Now.Ticks.ToString();
                        operacion.NotificacionOperacion = "S";

                        if (listDocumentosAdjuntos != null)
                            operacion.DocumentoAdjunto = "S";
                        else
                            operacion.DocumentoAdjunto = "N";

                        operacion.FechaRegistro = DateTime.Now;
                        operacion.FechaEmision = DateTime.Now;
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            operacion.FechaEnvio = DateTime.Now;
                            operacion.FechaVigente = DateAgregarLaborales(5, DateTime.Now);
                            operacion.NombreFinal = operacion.NumeroOperacion + ".pdf";
                        }

                        oNOperacion.GrabarDocumentoElectronico(operacion, listDocumentosAdjuntos, eDocumentoElectronicoOperacion, listEUsuarioGrupo, IDusuario);

                        GenerarPdfDatos(oNOperacion, operacion, eDocumentoElectronicoOperacion, listEUsuarioGrupo);
                    }
                    
                }
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                    mensajeRespuesta.Mensaje = "La operación " + operacion.NumeroOperacion + " se envió correctamente";
                else
                    mensajeRespuesta.Mensaje = "La operacion " + operacion.NumeroOperacion + " se grabó correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Mensaje = ex.Message;
                mensajeRespuesta.Exitoso = false;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
        public JsonResult ListarOperacion()
        {
            var listDocumentoElectronico = new List<EOperacion>();
            using (var oOperacion = new NOperacion())
            {
                listDocumentoElectronico = oOperacion.ListarOperacionElectronico(new UsuarioParticipante {
                    IDUsuario = Convert.ToInt32(Session["IDUsuario"].ToString()),
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico).OrderByDescending(x => x.FechaEnvio).ToList();
            }

            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult ListarOperacionReferencia(Operacion operacion)
        {
            try
            {
                var DocumentoReferencia = new EOperacion();
                var listAdjunto = new List<EAdjunto>();
                var AdjuntoPDF = new EAdjunto();
                using (var oOperacion = new NOperacion())
                {
                    DocumentoReferencia = oOperacion.ListarOperacionBusqueda().Where(x => x.NumeroOperacion == operacion.NumeroOperacion).FirstOrDefault();
                }

                using (var oAdjunto = new NAdjunto())
                {
                    listAdjunto = oAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == DocumentoReferencia.IDOperacion && x.EstadoAdjunto == Estados.EstadoAdjunto.Activo).ToList();
                }

                AdjuntoPDF.NombreOriginal = DocumentoReferencia.NombreFinal;
                AdjuntoPDF.RutaArchivo = string.Format(@"{0}\{1}", Session["RutaGdocPDF"].ToString(), DocumentoReferencia.NombreFinal);
                AdjuntoPDF.TipoArchivo = "application/pdf";
                listAdjunto.Add(AdjuntoPDF);

                return new JsonResult { Data = listAdjunto, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
       
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante= new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante==Constantes.EstadoParticipante.Activo && x.TipoParticipante!=Constantes.TipoParticipante.DestinatarioProveido && x.TipoParticipante!=Constantes.TipoParticipante.RemitenteProveido).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult EliminarOperacion(Operacion operacion)
        {
            try
            {
                using (var oOperacion = new NOperacion())
                {
                    operacion.EstadoOperacion = Estados.EstadoOperacion.Inactivo;
                    var respuesta = oOperacion.AnularDocumentoElectronico(operacion);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Documento Electronico Inactivo";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {

                throw;
            }

        }
        #region "Metodos"
        protected DateTime DateAgregarLaborales(Int32 add, DateTime FechaInicial)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday) { FechaInicial = FechaInicial.AddDays(2); }
            if (FechaInicial.DayOfWeek == DayOfWeek.Sunday) { FechaInicial = FechaInicial.AddDays(1); }
            Int32 weeks = add / 5;
            add += weeks * 2;
            if (FechaInicial.DayOfWeek > FechaInicial.AddDays(add).DayOfWeek)
                add += 2;

            if (FechaInicial.AddDays(add).DayOfWeek == DayOfWeek.Saturday)
                add += 2;

            return FechaInicial.AddDays(add);
        }
        protected void GenerarPdfDatos(NOperacion oNOperacion, Operacion operacion, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo)
        {
            var remitentes = new List<string>();
            var destinatarios = new List<string>();
            var listfirmaUsuario = new List<string>();
            var listremitentes = new List<string>();
            foreach (var item in listEUsuarioGrupo)
            {
                var usuario = nUsuario.ListarUsuario().Where(x => x.NombreUsuario == item.Nombre).FirstOrDefault();

                var personal = nPersonal.ListarPersonal().Where(x => x.IDPersonal == usuario.IDPersonal).FirstOrDefault();

                if (item.TipoParticipante == Constantes.TipoParticipante.RemitenteDE){
                    remitentes.Add(string.Format(@"{0} {1}{2}", personal.NombrePers, personal.ApellidoPersonal + Environment.NewLine, personal.Cargo.DescripcionConcepto));
                    if (usuario.FirmaElectronica != null || usuario.FirmaElectronica != string.Empty)
                    {
                        listfirmaUsuario.Add(usuario.NombreUsuario + ".jpg");
                        listremitentes = remitentes;
                    }
                }
                else
                    destinatarios.Add(string.Format(@"{0} {1}{2}", personal.NombrePers, personal.ApellidoPersonal + Environment.NewLine, personal.Cargo.DescripcionConcepto));
            }
            var documento = (string.Format(@"{0} {1}", oNOperacion.ListarOperacionBusqueda().Where(x => x.IDOperacion == operacion.IDOperacion).FirstOrDefault().TipoDoc.DescripcionCorta,operacion.NumeroOperacion));

            if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                new UtilPdf().GenerarArchivoPDF(operacion.NumeroOperacion, 
                    eDocumentoElectronicoOperacion.Memo, 
                    operacion.IDEmpresa, 
                    Session["RutaGdocPDF"].ToString(), 
                    documento,
                    string.Join(Environment.NewLine + Environment.NewLine, destinatarios.ToArray()),
                    string.Join(Environment.NewLine + Environment.NewLine, remitentes.ToArray()), 
                    operacion.DescripcionOperacion, 
                    operacion.TipoComunicacion,
                    listremitentes,
                    Session["RutaGdocImagenes"].ToString(),
                    listfirmaUsuario,
                    operacion.TipoDocumento);

        }
        #endregion
	}
}