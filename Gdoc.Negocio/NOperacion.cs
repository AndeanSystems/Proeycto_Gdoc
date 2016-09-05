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
                //Falta Terminar
                foreach (var participante in listEUsuarioGrupo)
                {
                    var eUsuarioParticipante = new UsuarioParticipante();
                    if (participante.Tipo.Equals(Usuario))//Grabar solo Usuarios
                    {
                        eUsuarioParticipante.IDUsuario = participante.IDUsuarioGrupo;
                        eUsuarioParticipante.IDOperacion = operacion.IDOperacion;
                        eUsuarioParticipante.TipoParticipante = 2;
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
                            eUsuarioParticipante.TipoParticipante = 2;
                            eUsuarioParticipante.ReenvioOperacion = "S";
                            eUsuarioParticipante.EstadoUsuarioParticipante = 1;
                            dUsuarioParticipante.Grabar(listEusuarioParticipante);
                        }
                    }
                }
                dUsuarioParticipante.Grabar(listEusuarioParticipante);
                dDocumentoElectronicoOperacion.Grabar(eDocumentoElectronicoOperacion);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
