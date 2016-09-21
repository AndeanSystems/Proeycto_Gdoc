using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gdoc.Dao
{
    public class DUsuarioGrupo
    {
        public List<UsuarioGrupo> listarUsuarioGrupo(UsuarioGrupo eUsuarioGrupo) {
            try
            {
                using (var db = new DataBaseContext())
                {
                    return db.UsuarioGrupoes.Where(x => x.IDGrupo == eUsuarioGrupo.IDGrupo).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<UsuarioGrupo> listarUsuarioG( )
        {
            var listUsuarioGrupo = new List<UsuarioGrupo>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.UsuarioGrupoes.ToList();


                    list.ForEach(x => listUsuarioGrupo.Add(new UsuarioGrupo
                    {
                        IDUsuarioGrupo=x.IDUsuarioGrupo,
                        IDUsuario=x.IDUsuario,
                        IDGrupo=x.IDGrupo,
                        UsuarioRegistro=x.UsuarioRegistro,
                        FechaRegistro = x.FechaRegistro,
                        EstadoUsuarioGrupo=x.EstadoUsuarioGrupo,

                        Usuario = new Usuario
                        {
                            NombreUsuario=x.Usuario.NombreUsuario,
                        }
                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listUsuarioGrupo;
        }
        public short GrabarUsuarioGrupo(List<UsuarioGrupo> listUsuarioGrupo)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.UsuarioGrupoes.AddRange(listUsuarioGrupo);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
