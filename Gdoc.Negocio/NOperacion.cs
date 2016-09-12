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
        protected DIndexacionDocumento dIndexacionDocumento = new DIndexacionDocumento();
        protected DGeneral dGeneral = new DGeneral();

        
        protected string Usuario = "U";
        protected string Grupo = "G";
        #endregion
        public void Dispose()
        {
            dOperacion = null;
            dUsuarioParticipante = null;
            dUsuarioGrupo = null;
            dGeneral = null;
        }

        public short Grabar(Operacion operacion, DocumentoElectronicoOperacion eDocumentoElectronicoOperacion, List<EUsuarioGrupo> listEUsuarioGrupo)
        {
            try
            {

                var listEusuarioParticipante = new List<UsuarioParticipante>();
                dOperacion.Grabar(operacion);


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
                        eUsuarioParticipante.TipoOperacion = "03";
                        eUsuarioParticipante.TipoParticipante = "03";//FALTA CORREGIR
                        eUsuarioParticipante.ReenvioOperacion = "S";
                        eUsuarioParticipante.EstadoUsuarioParticipante = 1;
                        listEusuarioParticipante.Add(eUsuarioParticipante);
                    }
                    else {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo{ IDGrupo = participante.IDUsuarioGrupo};
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoParticipante = "03";//FALTA CORREGIR
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = 1;
                            dUsuarioParticipante.Grabar(listEusuarioParticipante);
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
                    byte[] imageBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(documentoOperacion.RutaFisica);
                    documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoOperacion.NombreOriginal);
                    documentoOperacion.IDOperacion = operacion.IDOperacion;
                    documentoOperacion.NombreFisico = string.Empty;
                    using (MemoryStream stream = new MemoryStream(imageBytes))
                    {
                        Image.FromStream(stream).Save(documentoOperacion.RutaFisica, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                        eUsuarioParticipante.TipoOperacion = "02";
                        eUsuarioParticipante.TipoParticipante = "08";
                        eUsuarioParticipante.FechaNotificacion = System.DateTime.Now;
                        eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                        eUsuarioParticipante.EstadoUsuarioParticipante = 1;//FALTA
                        listEusuarioParticipante.Add(eUsuarioParticipante);
                    }
                    else
                    {
                        //Buscar usuarios por grupo
                        var eUsuarioGrupo = new UsuarioGrupo { IDGrupo = participante.IDUsuarioGrupo };
                        var listUsuarioGrupo = dUsuarioGrupo.listarUsuarioGrupo(eUsuarioGrupo);
                        foreach (var usuario in listUsuarioGrupo)
                        {
                            eUsuarioParticipante.IDUsuario = usuario.IDUsuario;
                            eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                            eUsuarioParticipante.TipoParticipante = "08";//FALTA CORREGIR
                            eUsuarioParticipante.ReenvioOperacion = "S";//FALTA
                            eUsuarioParticipante.EstadoUsuarioParticipante = 1;//FALTA
                            dUsuarioParticipante.Grabar(listEusuarioParticipante);
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
        public List<EOperacion> ListarOperacion()
        {
            try
            {
                return dOperacion.ListarOperacion();
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

    }
}
