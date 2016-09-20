﻿using Gdoc.Dao;
using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio.Utils;
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

        public short Grabar(Operacion operacion,List<Adjunto> listDocumentosAdjuntos, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo,Int64 IDusuario)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                //GRABAR OPERACION
                dOperacion.Grabar(operacion);

                //GRABAR DOCUMENTO ELECTRONICO OPERACION
                eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);

                //var lista = dUsuarioParticipante.ListarUsuarioParticipante();
                var listDocumentoAdjunto = new List<DocumentoAdjunto>();
                var eDocumentoAdjunto = new DocumentoAdjunto();

                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                //COPIAR ADJUNTO Y GRABAR
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
                        //listDocumentoAdjunto.Add(eDocumentoAdjunto);
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
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);

                //GRABA log operacion
                GrabarLogOperacion("011", operacion, IDusuario);
                GrabarLogOperacion("017", operacion, IDusuario);

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
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var listEindexacionDocumento = new List<IndexacionDocumento>();
                //Grabar Operacion
                dOperacion.Grabar(operacion);

                //Traer las ruta de la tabla general
                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos)) {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                foreach (var documentoOperacion in listDocumentoDigitalOperacion)
                {
                    byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(documentoOperacion.RutaFisica);
                    documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoOperacion.NombreOriginal);
                    documentoOperacion.IDOperacion = operacion.IDOperacion;
                    documentoOperacion.NombreFisico = string.Empty;
                    documentoOperacion.TamanoDocto = documentoOperacion.TamanoDocto;
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


                //Grabar documentoDigitalOperacion
                dDocumentoDigitalOperacion.GrabarDocumentoDigitalOperacion(listDocumentoDigitalOperacion);

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
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                            eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            listEusuarioParticipante.Add(eUsuarioParticipante);
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);


                //grabar referencias
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
                

                //GRABA log operacion
                GrabarLogOperacion("001",operacion, IDusuario);
                GrabarLogOperacion("007",operacion, IDusuario);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarMesaVirtual(Operacion operacion, List<EUsuarioGrupo> listEUsuarioGrupo)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                dOperacion.Grabar(operacion);

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
                            eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.MesaVirtual;
                            eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
                            eUsuarioParticipante.FechaNotificacion = operacion.FechaEnvio;
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            if (listEusuarioParticipante.Count(x => x.IDUsuario == usuario.IDUsuario) == 0)
                                listEusuarioParticipante.Add(eUsuarioParticipante);
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
        public short EditarOperacion(Operacion operacion,DocumentoElectronicoOperacion eDocumentoElectronicoOperacion)
        {
            try
            {
                dOperacion.EditarOperacion(operacion);
                dDocumentoElectronicoOperacion.Editar(eDocumentoElectronicoOperacion);
                //FALTA EDITAR DOCUMENTO ADJUNTO
                //FALTA EDITAR USUARIOS PARTICIPANTES
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Operacion EliminarOperacion(Operacion operacion)
        {
            try
            {
                return dOperacion.EliminarOperacion(operacion);
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
