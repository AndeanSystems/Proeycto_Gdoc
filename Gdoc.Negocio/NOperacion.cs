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
        public short Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo,Int64 IDusuario )
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                //var lista = dUsuarioParticipante.ListarUsuarioParticipante();
                var listDocumentoAdjunto = new List<DocumentoAdjunto>();
                var eDocumentoAdjunto = new DocumentoAdjunto();
                var eMensajeAlerta = new MensajeAlerta();
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);

                //GRABAR OPERACION
                dOperacion.Grabar(operacion);

                //GRABAR DOCUMENTO ELECTRONICO OPERACION
                eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);

                //COPIAR ADJUNTO Y GRABAR 
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                if(listDocumentosAdjuntos!=null)
                {
                    foreach (var adjunto in listDocumentosAdjuntos)
                    {
                        byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                        adjunto.IDUsuario = IDusuario;
                        adjunto.NombreOriginal = adjunto.NombreOriginal;
                        //documentoAdjunto.RutaArchivo = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoAdjunto.NombreOriginal);
                        adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                        adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                        adjunto.FechaRegistro = System.DateTime.Now;

                        if(operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;
                        else
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Inactivo;

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

                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE && operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                        {
                            eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                            eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                            eMensajeAlerta.TipoAlerta = 1;
                            eMensajeAlerta.CodigoEvento = "017";
                            eMensajeAlerta.EstadoMensajeAlerta = 1;
                            eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;

                            dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
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

                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE &&  operacion.EstadoOperacion==Estados.EstadoOperacion.Activo)
                            {
                                eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                                eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                                eMensajeAlerta.TipoAlerta = 1;
                                eMensajeAlerta.EstadoMensajeAlerta = 1;
                                eMensajeAlerta.CodigoEvento = "017";
                                eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;

                                dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
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
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                dOperacion.EditarOperacion(operacion);

                dDocumentoElectronicoOperacion.Editar(eDocumentoElectronicoOperacion);
                var eDocumentoAdjunto = new DocumentoAdjunto();


                //ANULAR DOCUMENTOS ADJUNTOS y ADJUNTOS
                //foreach (var item in listDocumentosAdjuntos)
                //{
                //    item.EstadoAdjunto = Estados.EstadoAdjunto.Inactivo;
                //    dAdjunto.AnularAdjunto(item);
                //    dDocumentoAdjunto.AnularDocumentoAdjunto(item);
                //}
                //FALTA EDITAR DOCUMENTO ADJUNTO
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                if (listDocumentosAdjuntos != null)
                {
                    foreach (var adjunto in listDocumentosAdjuntos)
                    {
                        byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                        adjunto.IDUsuario = IDusuario;
                        adjunto.NombreOriginal = adjunto.NombreOriginal;
                        //documentoAdjunto.RutaArchivo = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoAdjunto.NombreOriginal);
                        adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                        adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                        adjunto.FechaRegistro = System.DateTime.Now;

                        if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Activo;
                        else
                            adjunto.EstadoAdjunto = Estados.EstadoAdjunto.Inactivo;

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
                    var usuarioencontrado = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDUsuario == participante.IDUsuarioGrupo).FirstOrDefault();

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
                                }
                            }
                        }
                    }
                    
                    
                }

                var uparticipantesguardados2 = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion && x.EstadoUsuarioParticipante == 1);

                //SI SE ENVIA GRABA ALERTA
                if (operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                {
                    foreach (var item in uparticipantesguardados2)
                    {
                        //GRABAR MENSAJE ALERTA
                        if (item.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                            eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                            eMensajeAlerta.TipoAlerta = 1;
                            eMensajeAlerta.CodigoEvento = "017";
                            eMensajeAlerta.EstadoMensajeAlerta = 1;
                            eMensajeAlerta.IDUsuario = item.IDUsuario;

                            dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                        }
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
                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                            eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                            eMensajeAlerta.TipoAlerta = 1;
                            eMensajeAlerta.CodigoEvento = "007";
                            eMensajeAlerta.EstadoMensajeAlerta = 1;
                            eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;
                            dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                        }
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

                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            {
                                eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                                eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                                eMensajeAlerta.TipoAlerta = 1;
                                eMensajeAlerta.CodigoEvento = "007";
                                eMensajeAlerta.EstadoMensajeAlerta = 1;
                                eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;
                                dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                            }
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
                dOperacion.EditarOperacion(operacion);
                //dDocumentoElectronicoOperacion.Editar(eDocumentoElectronicoOperacion);
                var eDocumentoAdjunto = new DocumentoAdjunto();
                //FALTA EDITAR DOCUMENTO ADJUNTO

                //FALTA EDITAR USUARIOS PARTICIPANTES
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

                        //GRABAR MENSAJE ALERTA
                        if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                        {
                            eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                            eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                            eMensajeAlerta.TipoAlerta = 1;
                            eMensajeAlerta.CodigoEvento = "044";
                            eMensajeAlerta.EstadoMensajeAlerta = 1;
                            eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;

                            dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                        }
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

                            //GRABAR MENSAJE ALERTA
                            if (eUsuarioParticipante.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV && operacion.EstadoOperacion == Estados.EstadoOperacion.Activo)
                            {
                                eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                                eMensajeAlerta.FechaAlerta = operacion.FechaEnvio;
                                eMensajeAlerta.TipoAlerta = 1;
                                eMensajeAlerta.CodigoEvento = "044";
                                eMensajeAlerta.EstadoMensajeAlerta = 1;
                                eMensajeAlerta.IDUsuario = eUsuarioParticipante.IDUsuario;

                                dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                            }
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
                var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                //ANULAR OPERACION
                 dOperacion.EliminarOperacion(operacion);
                //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                 foreach (var usuario in listUsuarioParticipante)
                 {
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.DestinatarioDD)
                     {
                         eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                         eMensajeAlerta.FechaAlerta = System.DateTime.Now;
                         eMensajeAlerta.TipoAlerta = 1;
                         eMensajeAlerta.CodigoEvento = "009";
                         eMensajeAlerta.EstadoMensajeAlerta = 1;
                         eMensajeAlerta.IDUsuario = usuario.IDUsuario;

                         dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                     }
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
                 var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                 //ANULAR OPERACION
                 dOperacion.EliminarOperacion(operacion);
                 //EDITA FECHA CIERRE
                 operacion.FechaCierre = System.DateTime.Now;
                 dOperacion.EditarOperacion(operacion);
                 //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                 foreach (var usuario in listUsuarioParticipante)
                 {
                     if (usuario.TipoParticipante == Constantes.TipoParticipante.ColaboradorMV)
                     {
                         eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                         eMensajeAlerta.FechaAlerta = System.DateTime.Now;
                         eMensajeAlerta.TipoAlerta = 1;
                         eMensajeAlerta.CodigoEvento = "038";
                         eMensajeAlerta.EstadoMensajeAlerta = 1;
                         eMensajeAlerta.IDUsuario = usuario.IDUsuario;

                         dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                     }
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
                var listUsuarioParticipante = dUsuarioParticipante.ListarUsuarioParticipante().Where(x => x.IDOperacion == operacion.IDOperacion).ToList();
                //ANULAR OPERACION
                dOperacion.EliminarOperacion(operacion);
                //GRABAR MENSAJE ALERTA A TODOS LOS PARTICIPANTES
                foreach (var usuario in listUsuarioParticipante)
                {
                    if (usuario.TipoParticipante == Constantes.TipoParticipante.DestinatarioDE)
                    {
                        eMensajeAlerta.IDOperacion = operacion.IDOperacion;
                        eMensajeAlerta.FechaAlerta = System.DateTime.Now;
                        eMensajeAlerta.TipoAlerta = 1;
                        eMensajeAlerta.CodigoEvento = "040";
                        eMensajeAlerta.EstadoMensajeAlerta = 1;
                        eMensajeAlerta.IDUsuario = usuario.IDUsuario;

                        dMensajeAlerta.GrabarMensajeAlerta(eMensajeAlerta);
                    }
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Metodos

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
        #endregion
    }
}
