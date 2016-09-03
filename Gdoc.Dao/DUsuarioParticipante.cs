using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Dao
{
    public class DUsuarioParticipante
    {
        public short Grabar(List<UsuarioParticipante> listUsuarioParticipante) {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.UsuarioParticipantes.AddRange(listUsuarioParticipante);
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
