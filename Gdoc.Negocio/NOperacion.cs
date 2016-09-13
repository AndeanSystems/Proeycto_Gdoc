using Gdoc.Dao;
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
        protected DIndexacionDocumento dIndexacionDocumento = new DIndexacionDocumento();
        protected DGeneral dGeneral = new DGeneral();

        
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
        }

        public short Grabar(Operacion operacion, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo, List<EUsuarioGrupo> listEDestinatario = null)
        {
            try
            {
                var listEusuarioParticipante = new List<UsuarioParticipante>();
                dOperacion.Grabar(operacion);
                //var lista = dUsuarioParticipante.ListarUsuarioParticipante();

                eDocumentoElectronicoOperacion.IDOperacion = operacion.IDOperacion;
                dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);
                //Falta Terminar
                foreach (var participante in listEUsuarioGrupo)
                {

                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))//Grabar solo Usuarios
                    {
                        
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoElectronico;
                        eUsuarioParticipante.TipoParticipante = participante.TipoParticipante;
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
        public short GrabarDocumentoDigital(Operacion operacion, List<DocumentoDigitalOperacion> listDocumentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo,List<IndexacionDocumento> listIndexacion)
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
                    documentoOperacion.Comentario = documentoOperacion.Comentario;
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

                //Falta Terminar
                foreach (var participante in listEUsuarioGrupo)
                {
                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))//Grabar solo Usuarios
                    {
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoOperacion = Constantes.TipoOperacion.DocumentoDigital;
                        eUsuarioParticipante.TipoParticipante = operacion.TipoOperacion;
                        eUsuarioParticipante.FechaNotificacion = DateAgregarLaborales(5, System.DateTime.Now);
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
                            eUsuarioParticipante.TipoParticipante = operacion.TipoOperacion;
                            eUsuarioParticipante.FechaNotificacion = DateAgregarLaborales(5, System.DateTime.Now);
                            eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                            eUsuarioParticipante.EstadoUsuarioParticipante = Constantes.EstadoParticipante.Activo;
                            listEusuarioParticipante.Add(eUsuarioParticipante);
                        }
                    }
                }
                //--
                foreach (var referencia in listIndexacion)
                {
                    var eIndexacionDocumento = new IndexacionDocumento();

                    eIndexacionDocumento.DescripcionIndice = referencia.DescripcionIndice;
                    eIndexacionDocumento.EstadoIndice="1";
                    eIndexacionDocumento.IDOperacion = operacion.IDOperacion;
                    listEindexacionDocumento.Add(eIndexacionDocumento);
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);
                dIndexacionDocumento.GrabarIndexacion(listEindexacionDocumento);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarOperacionDigital()
        {
            try
            {
                return dOperacion.ListarOperacionDigital();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EOperacion> ListarOperacionElectronico()
        {
            try
            {
                return dOperacion.ListarOperacionElectronico();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Operacion EditarOperacion(Operacion operacion)
        {
            try
            {
                return dOperacion.EditarOperacion(operacion);
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
        #endregion
    }
}
