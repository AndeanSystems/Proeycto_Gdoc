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
        #endregion
        //
        // GET: /DocumentoElectronico/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo) {
            //EnviarCorreo();
            try
            {
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
                        
                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            new UtilPdf().GenerarArchivoPDF(operacion.NumeroOperacion, "Electronico", eDocumentoElectronicoOperacion.Memo, operacion.IDEmpresa, Session["RutaGdocPDF"].ToString());
                    }
                    else
                    {
                        operacion.IDEmpresa = Convert.ToInt32(Session["IDEmpresa"]);
                        operacion.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;

                        operacion.NumeroOperacion = "DE" + DateTime.Now.Ticks.ToString();
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

                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            new UtilPdf().GenerarArchivoPDF(operacion.NumeroOperacion, "Electronico", eDocumentoElectronicoOperacion.Memo, operacion.IDEmpresa, Session["RutaGdocPDF"].ToString());
                    }
                    
                }
                mensajeRespuesta.Mensaje = "Operación "+operacion.NumeroOperacion+" realizada correctamente";
                mensajeRespuesta.Exitoso = true;
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                //mensajeRespuesta.Mensaje = "Operación no realizada correctamente";
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
                }).Where(x => x.TipoOperacion == Constantes.TipoOperacion.DocumentoElectronico).ToList();
            }

            return new JsonResult { Data = listDocumentoElectronico, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult ListarUsuarioParticipanteDE(Operacion operacion)
        {
            var listUsuarioParticipante= new List<EUsuarioParticipante>();
            using (var oUsuarioParticipante = new NUsuarioParticipante())
            {
                listUsuarioParticipante = oUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante==Constantes.EstadoParticipante.Activo).ToList();
            }
            return new JsonResult { Data = listUsuarioParticipante, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
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

        protected void EnviarCorreo()
        {
            /*-------------------------MENSAJE DE CORREO----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            //Direccion de correo electronico a la que queremos enviar el mensaje
            mmsg.To.Add("andersonberrocal94@gmail.com");

            //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

            //Asunto
            mmsg.Subject = "Asunto del correo";
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Direccion de correo electronico que queremos que reciba una copia del mensaje
            //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

            //Cuerpo del Mensaje
            mmsg.Body = "Texto del contenio del mensaje de correo";
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = false; //Si no queremos que se envíe como HTML

            //Correo electronico desde la que enviamos el mensaje
            mmsg.From = new System.Net.Mail.MailAddress("jmorales@fpcmac.org.pe");


            /*-------------------------CLIENTE DE CORREO----------------------*/

            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            //Hay que crear las credenciales del correo emisor
            cliente.Credentials =
                new System.Net.NetworkCredential("jmorales@fpcmac.org.pe", "Peru2015");

            //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail

            cliente.Port = 25;
            cliente.EnableSsl = false;
            cliente.Host = "smtp.office365.com"; //Para Gmail "smtp.gmail.com"; 
            /*-------------------------ENVIO DE CORREO----------------------*/

            try
            {
                //Enviamos el mensaje      
                cliente.Send(mmsg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
            }
            //var eMailSent = false;

            //eMailSent = true;
            //var eMailSubject = Request["subject"];
            //if (eMailSubject == null)
            //{
            //    eMailSubject = "Asunto vacío";
            //}
            //var eMailMessage = Request["message"];
            //if (eMailMessage == null)
            //{
            //    eMailMessage = "Mensaje vacío";
            //}

            //WebMail.SmtpServer = "smtp.gmail.com";
            //WebMail.SmtpPort = 587;
            //WebMail.EnableSsl = true;
            //WebMail.UserName = "andersonberrocal94@gmail.com";
            //WebMail.From = "andersonberrocal94@gmail.com";
            //WebMail.Password = "anderson147";
            //WebMail.Send(to: "apacaya1@gmail.com", subject: eMailSubject, body: eMailMessage);
            
        }
	}
}