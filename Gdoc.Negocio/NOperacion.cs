using Gdoc.Common.Utilitario;
using Gdoc.Dao;
using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Negocio
{
    //Comentario
    public class NOperacion:IDisposable
    {
        #region "Variable"
        protected DOperacion dOperacion = new DOperacion();
        protected DUsuarioParticipante dUsuarioParticipante = new DUsuarioParticipante();
        protected DUsuarioGrupo dUsuarioGrupo = new DUsuarioGrupo();
        protected DDocumentoElectronicoOperacion dDocumentoElectronicoOperacion = new DDocumentoElectronicoOperacion();
        protected DDocumentoDigitalOperacion dDocumentoDigitalOperacion = new DDocumentoDigitalOperacion();
        protected DAdjunto dAdjunto = new DAdjunto();
        protected DIndexacionDocumento dIndexacionDocumento = new DIndexacionDocumento();
        protected DGeneral dGeneral = new DGeneral();
        protected DLogOperacion dLogOperacion = new DLogOperacion();
        protected DDocumentoAdjunto dDocumentoAdjunto = new DDocumentoAdjunto();
        protected DMensajeAlerta dMensajeAlerta = new DMensajeAlerta();
        protected DUsuario dUsuario = new DUsuario();
        protected DConcepto dConcepto = new DConcepto();

        protected string Usuario = "U";
        protected string Grupo = "G";
        protected string ArchivoImagen = "image";
        protected string ArchivoTXT = "text/plain";
        #endregion
        public void Dispose()
        {
            dOperacion = null;
            dUsuarioParticipante = null;
            dUsuarioGrupo = null;
            dGeneral = null;
            dAdjunto = null;
        }

        public List<EOperacion> ListarOperacionBusqueda()
        {
            try
            {
                return dOperacion.ListarOperacionBusqueda();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarDocumentosRecibidos(UsuarioParticipante eUsuarioParticipante)
        {
            try
            {
                return dOperacion.ListarDocumentosRecibidos(eUsuarioParticipante);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarDocumentoElectronico(Operacion operacion,List<Adjunto> listDocumentosAdjuntos, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo,Int64 IDusuario )
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var listDocumentoAdjunto = new List<DocumentoAdjunto>();
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eMensajeAlerta = new MensajeAlerta();
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                //GRABAR OPERACION
                dOperacion.Grabar(operacion);

                //GRABAR DOCUMENTO ELECTRONICO OPERACION
                eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);

                //COPIAR ADJUNTO Y GRABAR 
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                
                if(listDocumentosAdjuntos!=null)
                {
                    foreach (var adjunto in listDocumentosAdjuntos)
                    {
                        
                        //GuardarImagen(adjunto, IDusuario, eGeneral, operacion);
                        if (adjunto.RutaArchivo.Contains(eGeneral.RutaGdocAdjuntos) || adjunto.RutaArchivo.Contains(eGeneral.RutaGdocPDF))
                        {
                            if (adjunto.RutaArchivo.Contains(eGeneral.RutaGdocPDF))
                                System.IO.File.Copy(adjunto.RutaArchivo, string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal), true);
                            else
                                System.IO.File.Copy(adjunto.RutaArchivo, string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal), true);
                           
                           adjunto.IDUsuario = IDusuario;
                           adjunto.NombreOriginal = adjunto.NombreOriginal;
                           adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                           adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                           adjunto.FechaRegistro = System.DateTime.Now;

                           adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;


                        }
                        else
                        {
                            byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);

                            adjunto.IDUsuario = IDusuario;
                            adjunto.NombreOriginal = adjunto.NombreOriginal;
                            adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                            adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                            adjunto.FechaRegistro = System.DateTime.Now;

                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;

                            if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                            {
                                File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                            }
                            else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                            {
                                File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                            }
                            else
                            {
                                using (MemoryStream stream = new MemoryStream(fileBytes))
                                {
                                    Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                        }

                        //GRABAR ADJUNTO
                        dAdjunto.GrabarAdjunto(adjunto);
                        //GRABAR DOCUMENTO ADJUNTO
                        eDocumentoAdjunto = new DocumentoAdjunto();
                        eDocumentoAdjunto.IDOperacion = operacion.IDOperacion;
                        eDocumentoAdjunto.IDAdjunto = adjunto.IDAdjunto;
                        eDocumentoAdjunto.EstadoDoctoAdjunto = adjunto.EstadoAdjunto;
                        dDocumentoAdjunto.GrabarDocumentoAdjunto(eDocumentoAdjunto);
                    }
                }
                //GRABA USUARIOS PARTICIPANTES
                foreach (var participante in listEUsuarioGrupo)
                {
                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))
                    {
                        //Grabar solo Usuarios
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                        eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                        eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                        eUsuarioParticipante.ReenvioOperacion = "S";
                        eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                        //listEusuarioParticipante.Add(eUsuarioParticipante);
                        if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                            listEusuarioParticipante.Add(eUsuarioParticipante);

                        if (participante.TipoParticipante == Constantes.TipoParticipante.RemitenteDE)
                            eUsuarioRemitente.Add(eUsuarioParticipante);
                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE && operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                        {
                            GrabarMensajeAlerta("017", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);

                            var correo = dUsuario.ListarUsuario().Where(x => x.IDUsuario == participante.IDUsuarioGrupo).FirstOrDefault().Personal.EmailTrabrajo;
                            EnviarCorreo(correo, operacion.TituloOperacion, "017", operacion.NumeroOperacion, operacion.TipoOperacion, participante.IDUsuarioGrupo);
                        }
                    }
                    else {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo{ IDGrupo = participante.IDUsuarioGrupo};
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante = new UsuarioParticipante();
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                listEusuarioParticipante.Add(eUsuarioParticipante);

                            if (participante.TipoParticipante == Constantes.TipoParticipante.RemitenteDE)
                                eUsuarioRemitente.Add(eUsuarioParticipante);
                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE &&  operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                            {
                                GrabarMensajeAlerta("017", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);
                                var correo = dUsuario.ListarUsuario().Where(x => x.IDUsuario == participante.IDUsuarioGrupo).FirstOrDefault().Personal.EmailTrabrajo;
                                EnviarCorreo(correo, operacion.TituloOperacion, "017", operacion.NumeroOperacion,operacion.TipoOperacion,usuario.IDUsuario);
                            }
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);

                //GRABA LOG OPERACION
                GrabarLogOperacion("011", operacion, IDusuario);
                if(operacion.EstadoOperacion==1)
                    GrabarLogOperacion("017", operacion, IDusuario);

                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short EditarDocumentoElectronico(Operacion operacion, List<Adjunto> listDocumentosAdjuntos, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, Int64 IDusuario)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var eMensajeAlerta = new MensajeAlerta();
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                //EDITAR OPERACION
                dOperacion.EditarOperacion(operacion);
                //EDITAR DOCUMENTO ELECTRONICO
                dDocumentoElectronicoOperacion.Editar(eDocumentoElectronicoOperacion);

                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);

                if (listDocumentosAdjuntos != null)
                {
                    //EDITAR DOCUMENTO ADJUNTO Y ADJUNTO
                    //DESHABILITAR ADJUNTOS
                    //adjuntos guardados por operacion
                    var listAdjuntosGuardados = dAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion);
                    //adjuntos que voy a grabar
                    var lisAdjuntosActuales = listDocumentosAdjuntos.Select(x => x.NombreOriginal).ToList();
                    //adjuntos que se van a anular
                    IEnumerable<Adjunto> adjuntosAnulados = listAdjuntosGuardados.Where(x => !lisAdjuntosActuales.Contains(x.NombreOriginal) && x.EstadoAdjunto == 1);
                    //ANULA ADJUNTOS ELIMINADOS
                    foreach (var item in adjuntosAnulados)
                    {
                        eDocumentoAdjunto = new DocumentoAdjunto();
                        eDocumentoAdjunto = dDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDAdjunto == item.IDAdjunto).FirstOrDefault();
                        item.EstadoAdjunto = 2;
                        eDocumentoAdjunto.EstadoDoctoAdjunto = 2;
                        dAdjunto.EditarAdjunto(item);
                        dDocumentoAdjunto.EditarDocumentoAdjunto(eDocumentoAdjunto);
                    }

                    foreach (var adjunto in listDocumentosAdjuntos)
                    {
                        var adjuntoencontrado = dAdjunto.ListarAdjunto().Where(x => x.NombreOriginal == adjunto.NombreOriginal && x.EstadoAdjunto==2).FirstOrDefault();                      
                        if (adjuntoencontrado != null)
                        {
                            eDocumentoAdjunto = dDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDAdjunto == adjuntoencontrado.IDAdjunto).FirstOrDefault();
                            adjuntoencontrado.EstadoAdjunto = 1;
                            eDocumentoAdjunto.EstadoDoctoAdjunto = 1;
                            dAdjunto.EditarAdjunto(adjuntoencontrado);
                            dDocumentoAdjunto.EditarDocumentoAdjunto(eDocumentoAdjunto);
                            eDocumentoAdjunto = new DocumentoAdjunto();
                        }
                        else if (adjunto.EstadoAdjunto != 1 && adjunto.EstadoAdjunto != 2)
                        {
                            byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                            adjunto.IDUsuario = IDusuario;
                            adjunto.NombreOriginal = adjunto.NombreOriginal;
                            adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                            adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                            adjunto.FechaRegistro = System.DateTime.Now;
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;

                            if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                                File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                            else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                                File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                            else
                            {
                                using (MemoryStream stream = new MemoryStream(fileBytes))
                                {
                                    Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            //GRABAR ADJUNTO
                            dAdjunto.GrabarAdjunto(adjunto);

                            //GRABAR DOCUMENTO ADJUNTO
                            eDocumentoAdjunto = new DocumentoAdjunto();
                            eDocumentoAdjunto.IDOperacion = operacion.IDOperacion;
                            eDocumentoAdjunto.IDAdjunto = adjunto.IDAdjunto;
                            eDocumentoAdjunto.EstadoDoctoAdjunto = adjunto.EstadoAdjunto;
                            dDocumentoAdjunto.GrabarDocumentoAdjunto(eDocumentoAdjunto);
                        }
                        
                    }
                }
                //EDITAR USUARIOS PARTICIPANTES
                
                //DESHABILITAR USUARIOS PARTICIPANTES
                var uparticipantesguardados = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante==1);              
                var participantesactuales = listEUsuarioGrupo.Select(x => x.IDUsuarioGrupo).ToList();
                IEnumerable<EUsuarioParticipante> nuevos = uparticipantesguardados.Where(x => !participantesactuales.Contains(x.IDUsuarioGrupo));
                foreach (var item in nuevos)
                {
                    if (item.EstadoUsuarioParticipante != 0)
                    {
                        item.EstadoUsuarioParticipante = 2;
                        dUsuarioParticipante.Editar(item);
                    }
                }
                foreach (var participante in listEUsuarioGrupo)
                {
                    var usuarioencontrado = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDUsuario == participante.IDUsuarioGrupo && x.IDOperacion==operacion.IDOperacion).FirstOrDefault();

                    if(usuarioencontrado!=null){
                        usuarioencontrado.EstadoUsuarioParticipante = 1;
                        dUsuarioParticipante.Editar(usuarioencontrado);
                    }
                    else
                    {
                        if (participante.EstadoUsuarioParticipante != 1 && participante.EstadoUsuarioParticipante != 2)
                        {
                            var eUsuarioParticipante = new UsuarioParticipante();
                            if (participante.Tipo.Equals(Usuario))
                            {
                                //Grabar solo Usuarios
                                eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                                eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                eUsuarioParticipante.ReenvioOperacion = "S";
                                eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                //listEusuarioParticipante.Add(eUsuarioParticipante);
                                if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                                    listEusuarioParticipante.Add(eUsuarioParticipante);

                                if (participante.TipoParticipante == Constantes.TipoParticipante.RemitenteDE)
                                    eUsuarioRemitente.Add(eUsuarioParticipante);
                            }
                            else
                            {
                                //Buscar usuarios por grupo
                                var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                                var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                                foreach (var usuario in listUsuarioGrupo)
                                {
                                    eUsuarioParticipante = new UsuarioParticipante();
                                    eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                                    eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                    eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                    eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                    eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                    eUsuarioParticipante.ReenvioOperacion = "S";
                                    eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                    if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                        listEusuarioParticipante.Add(eUsuarioParticipante);
                                    if (participante.TipoParticipante == Constantes.TipoParticipante.RemitenteDE)
                                        eUsuarioRemitente.Add(eUsuarioParticipante);
                                }
                            }
                        }
                    }
                }

                //SI SE ENVIA GRABA ALERTA
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                {
                    var uparticipantes = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);
                    foreach (var item in uparticipantes)
                    {
                        //GRABAR MENSAJE ALERTA
                        if (item.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            GrabarMensajeAlerta("017", operacion, item.IDUsuario, eUsuarioRemitente);

                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);

                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }       
        public short GrabarDocumentoDigital(Operacion operacion, List<DocumentoDigitalOperacion> listDocumentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<IndexacionDocumento> listIndexacion, Int64 IDusuario)
        {
            try
            {
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var listEindexacionDocumento = new List<IndexacionDocumento>();
                var eMensajeAlerta = new MensajeAlerta();
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                //GRABAR ADJUNTO
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos)) {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                foreach (var documentoOperacion in listDocumentoDigitalOperacion)
                {
                    byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(documentoOperacion.RutaFisica);
                    //documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoOperacion.NombreOriginal);
                    
                    var extension = documentoOperacion.NombreOriginal;
                    var n = extension.LastIndexOf(".");
                    var ext=extension.Substring(n);

                    operacion.NombreFinal = operacion.NumeroOperacion + ext;
                    //GRABAR OPERACION
                    dOperacion.Grabar(operacion);

                    documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocPDF, operacion.NombreFinal);
                    documentoOperacion.IDOperacion = operacion.IDOperacion;
                    documentoOperacion.NombreFisico = string.Empty;
                    documentoOperacion.TamanoDocto = documentoOperacion.TamanoDocto;
                    documentoOperacion.DerivarDocto = documentoOperacion.DerivarDocto;
                    if (string.IsNullOrEmpty(documentoOperacion.TipoArchivo) || !documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                    {
                        File.WriteAllBytes(documentoOperacion.RutaFisica, fileBytes);
                    }
                    else if (documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                    {
                        File.WriteAllText(documentoOperacion.RutaFisica, Encoding.UTF8.GetString(fileBytes));
                    }
                    else
                    {
                        using (MemoryStream stream = new MemoryStream(fileBytes))
                        {
                            Image.FromStream(stream).Save(documentoOperacion.RutaFisica, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }


                //GRABAR DOCUMENTODIGITALOPERACION
                dDocumentoDigitalOperacion.GrabarDocumentoDigitalOperacion(listDocumentoDigitalOperacion);

                //GRABAR USUARIOPARTICIPANTE
                foreach (var participante in listEUsuarioGrupo)
                {
                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))//Grabar solo Usuarios
                    {
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                        eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                        eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                        eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                        eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                        if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                            listEusuarioParticipante.Add(eUsuarioParticipante);

                        if (participante.TipoParticipante == Constantes.TipoParticipante.EmisorDD)
                            eUsuarioRemitente.Add(eUsuarioParticipante);
                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            GrabarMensajeAlerta("007", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);
                    }
                    else
                    {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante = new UsuarioParticipante();
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                            eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                                listEusuarioParticipante.Add(eUsuarioParticipante);
                            if (participante.TipoParticipante == Constantes.TipoParticipante.EmisorDD)
                                eUsuarioRemitente.Add(eUsuarioParticipante);
                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                                GrabarMensajeAlerta("007", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);


                //GRABAR INDEXACION
                if (listIndexacion!=null)
                {
                    foreach (var referencia in listIndexacion)
                    {
                        var eIndexacionDocumento = new IndexacionDocumento();

                        eIndexacionDocumento.DescripcionIndice = referencia.DescripcionIndice;
                        eIndexacionDocumento.EstadoIndice = 1;
                        eIndexacionDocumento.IDOperacion = operacion.IDOperacion;
                        listEindexacionDocumento.Add(eIndexacionDocumento);
                    }
                    dIndexacionDocumento.GrabarIndexacion(listEindexacionDocumento);
                }
                

                //GRABA LOG OPERACION
                GrabarLogOperacion("001",operacion, IDusuario);
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                    GrabarLogOperacion("007",operacion, IDusuario);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short EditarDocumentoDigital(Operacion operacion, List<DocumentoDigitalOperacion> listDocumentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<IndexacionDocumento> listIndexacion, Int64 IDusuario)
        {
            try
            {
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);

                var listEindexacionDocumento = new List<IndexacionDocumento>();
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var eMensajeAlerta = new MensajeAlerta();
                //dDocumentoElectronicoOperacion.Editar(eDocumentoElectronicoOperacion);
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                //EDITAR DOCUMENTO DIGITAL OPERACION
                var docDigiOper = dDocumentoDigitalOperacion.ListarDocumentoDigitalOperacion().Where(x => x.IDOperacion == operacion.IDOperacion).FirstOrDefault();
                //var participantesactuales = listEUsuarioGrupo.Select(x => x.IDUsuarioGrupo).ToList();
                //IEnumerable<EUsuarioParticipante> nuevos = uparticipantesguardados.Where(x => !participantesactuales.Contains(x.IDUsuarioGrupo));
                //foreach (var item in nuevos)
                //{
                //    if (item.EstadoUsuarioParticipante != 0)
                //    {
                //        item.EstadoUsuarioParticipante = 2;
                //        dUsuarioParticipante.Editar(item);
                //    }
                //}
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                if (listDocumentoDigitalOperacion != null)
                {
                    foreach (var documentoOperacion in listDocumentoDigitalOperacion)
                    {

                        var documentodigitalguardado = dDocumentoDigitalOperacion.ListarDocumentoDigitalOperacion().Where(x => x.IDOperacion == operacion.IDOperacion).FirstOrDefault();


                        var extension = documentoOperacion.NombreOriginal;
                        var n = extension.LastIndexOf(".");
                        var ext = extension.Substring(n);

                        operacion.NombreFinal = operacion.NumeroOperacion + ext;
                        //GRABAR OPERACION
                        dOperacion.EditarOperacion(operacion);
                        if (documentodigitalguardado.NombreOriginal != documentoOperacion.NombreOriginal)
                        {
                            byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(documentoOperacion.RutaFisica);

                            documentoOperacion.IDDoctoDigitalOperacion = docDigiOper.IDDoctoDigitalOperacion;
                            documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocPDF, operacion.NombreFinal);
                            documentoOperacion.IDOperacion = operacion.IDOperacion;
                            documentoOperacion.NombreFisico = string.Empty;
                            documentoOperacion.TamanoDocto = documentoOperacion.TamanoDocto;
                            documentoOperacion.DerivarDocto = documentoOperacion.DerivarDocto;
                            if (string.IsNullOrEmpty(documentoOperacion.TipoArchivo) || !documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                            {
                                File.WriteAllBytes(documentoOperacion.RutaFisica, fileBytes);
                            }
                            else if (documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                            {
                                File.WriteAllText(documentoOperacion.RutaFisica, Encoding.UTF8.GetString(fileBytes));
                            }
                            else
                            {
                                using (MemoryStream stream = new MemoryStream(fileBytes))
                                {
                                    Image.FromStream(stream).Save(documentoOperacion.RutaFisica, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            //EDITAR DOCUMENTODIGITALOPERACION
                            dDocumentoDigitalOperacion.EditarDocumentoDigitalOperacion(documentoOperacion);

                        }
                        
                    }
                }
                

                //EDITAR USUARIOS PARTICIPANTES

                //DESHABILITAR USUARIOS PARTICIPANTES
                var uparticipantesguardados = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);
                var participantesactuales = listEUsuarioGrupo.Select(x => x.IDUsuarioGrupo).ToList();
                IEnumerable<EUsuarioParticipante> nuevos = uparticipantesguardados.Where(x => !participantesactuales.Contains(x.IDUsuarioGrupo));
                foreach (var item in nuevos)
                {
                    if (item.EstadoUsuarioParticipante != 0)
                    {
                        item.EstadoUsuarioParticipante = 2;
                        dUsuarioParticipante.Editar(item);
                    }
                }
                foreach (var participante in listEUsuarioGrupo)
                {
                    var usuarioencontrado = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDUsuario == participante.IDUsuarioGrupo && x.IDOperacion == operacion.IDOperacion).FirstOrDefault();
                    if (usuarioencontrado != null)
                    {
                        usuarioencontrado.EstadoUsuarioParticipante = 1;
                        dUsuarioParticipante.Editar(usuarioencontrado);
                        
                    }
                    else
                    {
                        if (participante.EstadoUsuarioParticipante != 1 && participante.EstadoUsuarioParticipante != 2)
                        {
                            var eUsuarioParticipante = new UsuarioParticipante();
                            if (participante.Tipo.Equals(Usuario))
                            {
                                //Grabar solo Usuarios
                                eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                                eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                eUsuarioParticipante.ReenvioOperacion = "S";
                                eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                //listEusuarioParticipante.Add(eUsuarioParticipante);
                                if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                                    listEusuarioParticipante.Add(eUsuarioParticipante);
                                if (participante.TipoParticipante == Constantes.TipoParticipante.EmisorDD)
                                    eUsuarioRemitente.Add(eUsuarioParticipante);

                            }
                            else
                            {
                                //Buscar usuarios por grupo
                                var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                                var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                                foreach (var usuario in listUsuarioGrupo)
                                {
                                    eUsuarioParticipante = new UsuarioParticipante();
                                    eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                                    eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                    eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                    eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                    eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                    eUsuarioParticipante.ReenvioOperacion = "S";
                                    eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                    if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                        listEusuarioParticipante.Add(eUsuarioParticipante);
                                    if (participante.TipoParticipante == Constantes.TipoParticipante.EmisorDD)
                                        eUsuarioRemitente.Add(eUsuarioParticipante);
                                }
                            }
                        }
                    }
                }

                
                //SI SE ENVIA GRABA ALERTA
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                {
                    var uparticipantesalertas = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);

                    foreach (var item in uparticipantesalertas)
                    {
                        //GRABAR MENSAJE ALERTA
                        if (item.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            GrabarMensajeAlerta("007", operacion, item.IDUsuario, eUsuarioRemitente);
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);

                //EDITAR INDEXACION
                var referenciasguardadas = dIndexacionDocumento.ListarIndexacionDocumento().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoIndice == 1);
                var referenciasactuales = listIndexacion.Select(x => x.DescripcionIndice).ToList();
                IEnumerable<IndexacionDocumento> referenciasanuladas = referenciasguardadas.Where(x => !referenciasactuales.Contains(x.DescripcionIndice));
                //ANULA REFERENCIAS
                foreach (var item in referenciasanuladas)
                {
                    if (item.EstadoIndice != 0)
                    {
                        item.EstadoIndice = 2;
                        dIndexacionDocumento.EditarIndexacion(item);
                    }
                }
                //GRABAR INDEXACION
                if (listIndexacion != null)
                {
                    
                    foreach (var referencia in listIndexacion)
                    {
                        var referenciaencontrada = dIndexacionDocumento.ListarIndexacionDocumento().Where(x => x.DescripcionIndice == referencia.DescripcionIndice && x.EstadoIndice==2).FirstOrDefault();

                        if (referenciaencontrada != null )
                        {
                            referenciaencontrada.EstadoIndice = 1;
                            dIndexacionDocumento.EditarIndexacion(referenciaencontrada);
                            
                        }
                        else if(referencia.EstadoIndice != 2 && referencia.EstadoIndice != 1)
                        {
                            var eIndexacionDocumento = new IndexacionDocumento();

                            eIndexacionDocumento.DescripcionIndice = referencia.DescripcionIndice;
                            eIndexacionDocumento.EstadoIndice = 1;
                            eIndexacionDocumento.IDOperacion = operacion.IDOperacion;
                            listEindexacionDocumento.Add(eIndexacionDocumento);
                        }

                    }
                    dIndexacionDocumento.GrabarIndexacion(listEindexacionDocumento);
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarMesaVirtual(Operacion operacion, List<Adjunto> listAdjuntos, List<EUsuarioGrupo> listEUsuarioGrupo, Int64 IDusuario)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var eMensajeAlerta = new MensajeAlerta();
                dOperacion.Grabar(operacion);

                var listDocumentoAdjunto = new List<DocumentoAdjunto>();
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }

                //COPIAR ADJUNTO Y GRABAR
                if (listAdjuntos != null)
                {
                    foreach (var adjunto in listAdjuntos)
                    {
                        byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                        adjunto.IDUsuario = IDusuario;
                        adjunto.NombreOriginal = adjunto.NombreOriginal;
                        //documentoAdjunto.RutaArchivo = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoAdjunto.NombreOriginal);
                        adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                        adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                        adjunto.FechaRegistro = System.DateTime.Now;
                        adjunto.EstadoAdjunto = 1;
                        if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                        {
                            File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                        }
                        else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                        {
                            File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream(fileBytes))
                            {
                                Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }

                        dAdjunto.GrabarAdjunto(adjunto);

                        //GRABAR DOCUMENTO ADJUNTO
                        eDocumentoAdjunto = new DocumentoAdjunto();
                        eDocumentoAdjunto.IDOperacion = operacion.IDOperacion;
                        eDocumentoAdjunto.IDAdjunto = adjunto.IDAdjunto;
                        eDocumentoAdjunto.EstadoDoctoAdjunto = 1;
                        dDocumentoAdjunto.GrabarDocumentoAdjunto(eDocumentoAdjunto);
                    }
                }
                //Falta Terminar
                foreach (var participante in listEUsuarioGrupo)
                {

                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))//Grabar solo Usuarios
                    {

                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.MesaVirtual;
                        eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                        eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                        eUsuarioParticipante.ReenvioOperacion = "S";
                        eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                        if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                            listEusuarioParticipante.Add(eUsuarioParticipante);
                        if (participante.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV)
                            eUsuarioRemitente.Add(eUsuarioParticipante);
                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            GrabarMensajeAlerta("044", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);
                    }
                    else
                    {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante = new UsuarioParticipante();
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.MesaVirtual;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                listEusuarioParticipante.Add(eUsuarioParticipante);
                            if (participante.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV)
                                eUsuarioRemitente.Add(eUsuarioParticipante);
                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                                GrabarMensajeAlerta("044", operacion, eUsuarioParticipante.IDUsuario, eUsuarioRemitente);
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);

                //GRABA LOG OPERACION
                GrabarLogOperacion("030", operacion, IDusuario);
                if (operacion.EstadoOperacion == 1)
                    GrabarLogOperacion("044", operacion, IDusuario);
                return 1;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public short EditarMesaVirtual(Operacion operacion, List<Adjunto> listDocumentosAdjuntos, List<EUsuarioGrupo> listEUsuarioGrupo, Int64 IDusuario)
        {
            try
            {
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eMensajeAlerta = new MensajeAlerta();
                var eUsuarioRemitente = new List<UsuarioParticipante>();

                dOperacion.EditarOperacion(operacion);

                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);

                if (listDocumentosAdjuntos != null)
                {
                    //EDITAR DOCUMENTO ADJUNTO Y ADJUNTO
                    //DESHABILITAR ADJUNTOS
                    //adjuntos guardados por operacion
                    var listAdjuntosGuardados = dAdjunto.ListarAdjunto().Where(x => x.DocumentoAdjunto.IDOperacion == operacion.IDOperacion);
                    //adjuntos que voy a grabar
                    var lisAdjuntosActuales = listDocumentosAdjuntos.Select(x => x.NombreOriginal).ToList();
                    //adjuntos que se van a anular
                    IEnumerable<Adjunto> adjuntosAnulados = listAdjuntosGuardados.Where(x => !lisAdjuntosActuales.Contains(x.NombreOriginal) && x.EstadoAdjunto == 1);
                    //ANULA ADJUNTOS ELIMINADOS
                    foreach (var item in adjuntosAnulados)
                    {
                        eDocumentoAdjunto = new DocumentoAdjunto();
                        eDocumentoAdjunto = dDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDAdjunto == item.IDAdjunto).FirstOrDefault();
                        item.EstadoAdjunto = 2;
                        eDocumentoAdjunto.EstadoDoctoAdjunto = 2;
                        dAdjunto.EditarAdjunto(item);
                        dDocumentoAdjunto.EditarDocumentoAdjunto(eDocumentoAdjunto);
                    }
                    foreach (var adjunto in listDocumentosAdjuntos)
                    {
                        var adjuntoencontrado = dAdjunto.ListarAdjunto().Where(x => x.NombreOriginal == adjunto.NombreOriginal && x.EstadoAdjunto == 2).FirstOrDefault();
                        if (adjuntoencontrado != null)
                        {
                            eDocumentoAdjunto = dDocumentoAdjunto.ListarDocumentoAdjunto().Where(x => x.IDAdjunto == adjuntoencontrado.IDAdjunto).FirstOrDefault();
                            adjuntoencontrado.EstadoAdjunto = 1;
                            eDocumentoAdjunto.EstadoDoctoAdjunto = 1;
                            dAdjunto.EditarAdjunto(adjuntoencontrado);
                            dDocumentoAdjunto.EditarDocumentoAdjunto(eDocumentoAdjunto);
                            eDocumentoAdjunto = new DocumentoAdjunto();
                        }
                        else if (adjunto.EstadoAdjunto != 1 && adjunto.EstadoAdjunto != 2)
                        {
                            byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                            adjunto.IDUsuario = IDusuario;
                            adjunto.NombreOriginal = adjunto.NombreOriginal;
                            adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                            adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                            adjunto.FechaRegistro = System.DateTime.Now;
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;

                            if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                                File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                            else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                                File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                            else
                            {
                                using (MemoryStream stream = new MemoryStream(fileBytes))
                                {
                                    Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            //GRABAR ADJUNTO
                            dAdjunto.GrabarAdjunto(adjunto);

                            //GRABAR DOCUMENTO ADJUNTO
                            eDocumentoAdjunto = new DocumentoAdjunto();
                            eDocumentoAdjunto.IDOperacion = operacion.IDOperacion;
                            eDocumentoAdjunto.IDAdjunto = adjunto.IDAdjunto;
                            eDocumentoAdjunto.EstadoDoctoAdjunto = adjunto.EstadoAdjunto;
                            dDocumentoAdjunto.GrabarDocumentoAdjunto(eDocumentoAdjunto);
                        }

                    }
                }

                //EDITAR USUARIOS PARTICIPANTES

                //DESHABILITAR USUARIOS PARTICIPANTES
                var uparticipantesguardados = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);
                var participantesactuales = listEUsuarioGrupo.Select(x => x.IDUsuarioGrupo).ToList();
                IEnumerable<EUsuarioParticipante> nuevos = uparticipantesguardados.Where(x => !participantesactuales.Contains(x.IDUsuarioGrupo));
                foreach (var item in nuevos)
                {
                    if (item.EstadoUsuarioParticipante != 0)
                    {
                        item.EstadoUsuarioParticipante = 2;
                        dUsuarioParticipante.Editar(item);
                    }
                }
                foreach (var participante in listEUsuarioGrupo)
                {
                    var usuarioencontrado = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDUsuario == participante.IDUsuarioGrupo && x.IDOperacion == operacion.IDOperacion).FirstOrDefault();
                    if (usuarioencontrado != null)
                    {
                        usuarioencontrado.EstadoUsuarioParticipante = 1;
                        dUsuarioParticipante.Editar(usuarioencontrado);
                    }
                    else
                    {
                        if (participante.EstadoUsuarioParticipante != 1 && participante.EstadoUsuarioParticipante != 2)
                        {
                            var eUsuarioParticipante = new UsuarioParticipante();
                            if (participante.Tipo.Equals(Usuario))
                            {
                                //Grabar solo Usuarios
                                eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                                eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                eUsuarioParticipante.ReenvioOperacion = "S";
                                eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                //listEusuarioParticipante.Add(eUsuarioParticipante);
                                if (listEusuarioParticipante.Count(x => x.IDUsuario == eUsuarioParticipante.IDUsuario) == 0)
                                    listEusuarioParticipante.Add(eUsuarioParticipante);
                                if (participante.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV)
                                    eUsuarioRemitente.Add(eUsuarioParticipante);
                            }
                            else
                            {
                                //Buscar usuarios por grupo
                                var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                                var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                                foreach (var usuario in listUsuarioGrupo)
                                {
                                    eUsuarioParticipante = new UsuarioParticipante();
                                    eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                                    eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                                    eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                                    eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                                    eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                                    eUsuarioParticipante.ReenvioOperacion = "S";
                                    eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                                    if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                        listEusuarioParticipante.Add(eUsuarioParticipante);
                                    if (participante.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV)
                                        eUsuarioRemitente.Add(eUsuarioParticipante);
                                }
                            }
                        }
                    }
                }

                var uparticipantesalerta = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);

                //SI SE ENVIA GRABA ALERTA
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                {
                    foreach (var item in uparticipantesalerta)
                    {
                        //GRABAR MENSAJE ALERTA
                        if (item.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            GrabarMensajeAlerta("007", operacion, item.IDUsuario, eUsuarioRemitente);
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }     
        public List<EOperacion> ListarOperacionDigital(UsuarioParticipante eUsuarioParticipante)
        {
            try
            {
                return dOperacion.ListarOperacionDigital(eUsuarioParticipante);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarOperacionElectronico(UsuarioParticipante eUsuarioParticipante)
        {
            try
            {
                return dOperacion.ListarOperacionElectronico(eUsuarioParticipante);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarMesaVirtual(UsuarioParticipante eUsuarioParticipante)
        {
            try
            {
                return dOperacion.ListarMesaVirtual(eUsuarioParticipante);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarMesaTrabajoVirtual(UsuarioParticipante eUsuarioParticipante)
        {
            try
            {
                return dOperacion.ListarMesaTrabajoVirtual(eUsuarioParticipante);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short AnularDocumentoDigital(Operacion operacion)
        {
            try
            {
                var eMensajeAlerta = new MensajeAlerta();
                var eUsuarioRemitente = new List<UsuarioParticipante>();
                var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                //ANULAR OPERACION
                 dOperacion.EliminarOperacion(operacion);
                //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                 foreach (var usuario in listUsuarioParticipante)
                 {
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.EmisorDD)
                         eUsuarioRemitente.Add(usuario);
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)
                         GrabarMensajeAlerta("009", operacion, usuario.IDUsuario, eUsuarioRemitente);
                 }
                 return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short AnularMesaVirtual(Operacion operacion)
        {
             try
             {
                 var eMensajeAlerta = new MensajeAlerta();
                 var eUsuarioRemitente = new List<UsuarioParticipante>();
                 var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                 //ANULAR OPERACION
                 dOperacion.EliminarOperacion(operacion);
                 //EDITA FECHA CIERRE
                 operacion.FechaCierre = System.DateTime.Now;
                 dOperacion.EditarOperacion(operacion);
                 //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                 foreach (var usuario in listUsuarioParticipante)
                 {
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.OrganizadorMV)
                         eUsuarioRemitente.Add(usuario);
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV)
                         GrabarMensajeAlerta("038", operacion, usuario.IDUsuario, eUsuarioRemitente);
                 }
                 return 1;
             }
             catch (Exception)
             {

                 throw;
             }
         }
        public short AnularDocumentoElectronico(Operacion operacion)
        {
            try
            {
                var eMensajeAlerta = new MensajeAlerta();
                var eUsuarioRemitente = new List<UsuarioParticipante>();
                var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                //ANULAR OPERACION
                dOperacion.EliminarOperacion(operacion);
                //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                foreach (var usuario in listUsuarioParticipante)
                {
                    if (usuario.TipoParticipante == Constantes.TipoParticipante.RemitenteDE)
                        eUsuarioRemitente.Add(usuario);
                    if (usuario.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE)
                        GrabarMensajeAlerta("040", operacion, usuario.IDUsuario, eUsuarioRemitente);
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public String NumeroOperacion(Int64 IDUsuario, string tipooperacion)
        {
            try
            {
                return dOperacion.NumeroOperacion(IDUsuario, tipooperacion);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Metodos
        protected void EnviarCorreo(string correoDestinatario,string Titulo, string codigoEvento,string numerooperacion,string tipooperacion,long idusuario)
        {
            /*-------------------------MENSAJE DE CORREO----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            //Direccion de correo electronico a la que queremos enviar el mensaje
            //mmsg.To.Add("andersonberrocal94@gmail.com");
            mmsg.To.Add(correoDestinatario);

            //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

            //Asunto
            var ope = string.Empty;
            if (tipooperacion == Constantes.TipoOperacion.DocumentoElectronico)
                ope = " DOC-E ";
            else if (tipooperacion == Constantes.TipoOperacion.DocumentoElectronico)
                ope = " DOC-D ";
            else
                ope = " GRUPO-V ";

            var destinatario = dUsuario.ListarUsuario().Where(x => x.IDUsuario == idusuario).FirstOrDefault().NombreUsuario;
            mmsg.Subject = "Notificación DocWeb :" + ope + numerooperacion;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Busca Descripcion del Evento
            var evento = dConcepto.ListarConcepto().Where(x => x.TipoConcepto == "008" && x.CodiConcepto == codigoEvento).FirstOrDefault().DescripcionConcepto;
            
            //Direccion de correo electronico que queremos que reciba una copia del mensaje

            //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

            //Cuerpo del Mensaje
            var body = "Sr.(ita).  " + destinatario + 
                        Environment.NewLine +
                        Environment.NewLine +
                        evento +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Asunto: " + Titulo +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Pulse Click en el siguiente link " + 
                        "http://192.168.100.29:90" + 
                        " para ingresar al sistema DocWeb, usando sus credenciales de red."+
                        Environment.NewLine +
                        Environment.NewLine +
                        "Att." +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Administrador – DocWeb" +
                        Environment.NewLine +
                        "FEPCMAC" +
                        Environment.NewLine +
                        "Lima, Perú";
            mmsg.Body = body;
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

            cliente.Port = 587;
            cliente.EnableSsl = true;
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

        }
        protected void GrabarLogOperacion(string codigoevento, Operacion operacion, Int64 IDusuario)
        {
            try
            {
                var logoperacion = new LogOperacion();

                logoperacion.FechaEvento = System.DateTime.Now;
                logoperacion.IDOperacion = operacion.IDOperacion;
                logoperacion.CodigoEvento = codigoevento;
                logoperacion.IDUsuario = IDusuario;
                logoperacion.CodigoConexion = "";
                logoperacion.TerminalConexion = "";

                dLogOperacion.GrabarLogOperacion(logoperacion);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        protected void GrabarMensajeAlerta(string codigoevento, Operacion operacion, Int64 IDusuario,List<UsuarioParticipante> uRemitente)
        {
            try
            {
                var mensajeAlerta = new MensajeAlerta();
                var eUsuario = new List<string>();

                foreach (var item in uRemitente)
	            {
                    eUsuario.Add(dUsuario.ListarUsuario().Where(x => x.IDUsuario == item.IDUsuario).FirstOrDefault().NombreUsuario);
		 
	            }
                mensajeAlerta.IDOperacion = operacion.IDOperacion;
                mensajeAlerta.FechaAlerta = System.DateTime.Now;
                mensajeAlerta.TipoAlerta = 1;
                mensajeAlerta.EstadoMensajeAlerta = 1;
                mensajeAlerta.CodigoEvento = codigoevento;
                mensajeAlerta.IDUsuario = IDusuario;
                mensajeAlerta.Remitente = string.Join(",", eUsuario.ToArray());

                dMensajeAlerta.GrabarMensajeAlerta(mensajeAlerta);
                //Envia Correo
                var correo = dUsuario.ListarUsuario().Where(x => x.IDUsuario == IDusuario).FirstOrDefault().Personal.EmailTrabrajo;
                EnviarCorreo(correo, operacion.TituloOperacion, codigoevento);
            }
            catch (Exception)
            {

                throw;
            }

        }
        protected void GuardarImagen(Adjunto adjunto,Int64 IDusuario,General eGeneral, Operacion operacion)
        {
            try
            {
                byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                adjunto.IDUsuario = IDusuario;
                adjunto.NombreOriginal = adjunto.NombreOriginal;
                //documentoAdjunto.RutaArchivo = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoAdjunto.NombreOriginal);
                adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                adjunto.FechaRegistro = System.DateTime.Now;

                adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;

                if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                {
                    File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                }
                else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                {
                    File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                }
                else
                {
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #endregion
    }
}
