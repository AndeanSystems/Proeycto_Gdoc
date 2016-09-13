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
