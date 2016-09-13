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
        public List<UsuarioParticipante> ListarUsuarioParticipante()
        {
            var listUsuarioParticipante= new List<UsuarioParticipante>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.UsuarioParticipantes.ToList();


                    list.ForEach(x => listUsuarioParticipante.Add(new UsuarioParticipante
                    {
                        IDUsuarioParticipante = x.IDUsuarioParticipante,
                        IDUsuario= x.IDUsuario,
                        IDOperacion= x.IDOperacion,
                        TipoOperacion= x.TipoOperacion,
                        TipoParticipante = x.TipoParticipante,

                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listUsuarioParticipante;
        }
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
