using System;
using System.Linq;
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
    }
}
