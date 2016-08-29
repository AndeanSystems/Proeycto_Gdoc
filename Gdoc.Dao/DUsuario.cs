using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DUsuario
    {
        public Usuario ValidarLogin(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    return db.Usuarios.Where(x => x.NombreUsuario == usuario.NombreUsuario && x.ClaveUsuario == usuario.ClaveUsuario).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Usuario> ListarUsuario()
        {
            var listUsuario = new List<Usuario>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Usuarios.ToList();

                    var list3 = db.Usuarios.Join(db.Personals,
                                    u => u.IDPersonal,
                                    p => p.IDPersonal,
                                    (u, p) => new { Usuario = u, Personal = p }).ToList();

                    list.ForEach(x => listUsuario.Add(new Usuario
                    {
                        IDUsuario = x.IDUsuario,
                        NombreUsuario = x.NombreUsuario,
                        FirmaElectronica=x.FirmaElectronica,
                        EstadoUsuario=x.EstadoUsuario,
                        FechaRegistro=x.FechaRegistro,
                        FechaUltimoAcceso=x.FechaUltimoAcceso,
                        FechaModifica=x.FechaModifica,
                        IntentoErradoClave=x.IntentoErradoClave,
                        IntentoerradoFirma=x.IntentoerradoFirma,
                        TerminalUsuario=x.TerminalUsuario,
                        UsuarioRegistro=x.UsuarioRegistro,
                        CodigoConexion=x.CodigoConexion,
                        IDPersonal=x.IDPersonal,
                        CodigoRol=x.CodigoRol,
                        CodigoTipoUsua=x.CodigoTipoUsua,
                        ExpiraClave=x.ExpiraClave,
                        ExpiraFirma=x.ExpiraFirma,
                        FechaExpiraClave=x.FechaExpiraClave,
                        FechaExpiraFirma=x.FechaExpiraFirma
                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listUsuario;
        }

    }
}
