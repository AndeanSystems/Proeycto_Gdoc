using Gdoc.Dao;
using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
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

        
        protected string Usuario = "U";
        protected string Grupo = "G";
        #endregion
        public void Dispose()
        {
            dOperacion = null;
            dUsuarioParticipante = null;
            dUsuarioGrupo = null;
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
        public short GrabarDocumentoDigital(Operacion operacion, DocumentoDigitalOperacion documentoDigitalOperacion, List<EUsuarioGrupo> listEUsuarioGrupo,List<IndexacionDocumento> listIndexacion)
        {
            try
            {

                var listEusuarioParticipante = new List<UsuarioParticipante>();
                var listEindexacionDocumento = new List<IndexacionDocumento>();

                dOperacion.Grabar(operacion);

                documentoDigitalOperacion.IDOperacion = operacion.IDOperacion;

                //documentoDigitalOperacion.NombreOriginal = "EN DURO";//FALTA
                documentoDigitalOperacion.RutaFisica = "EN DURO";//FALTA
                documentoDigitalOperacion.TamanoDocto = 0;//FALTA
                documentoDigitalOperacion.NombreFisico = "EN DURO";//FALTA


                dDocumentoDigitalOperacion.GrabarDocumentoDigitalOperacion(documentoDigitalOperacion);

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
