using Gdoc.Entity.Extension;
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
        public List<EUsuarioParticipante> ListarUsuarioParticipante()
        {
            var listUsuarioParticipante= new List<EUsuarioParticipante>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.UsuarioParticipantes.ToList();


                    list.ForEach(x => listUsuarioParticipante.Add(new EUsuarioParticipante
                    {
                        IDUsuarioParticipante = x.IDUsuarioParticipante,
                        IDUsuario= x.IDUsuario,
                        IDOperacion= x.IDOperacion,
                        TipoOperacion= x.TipoOperacion,
                        TipoParticipante = x.TipoParticipante,
                        EstadoUsuarioParticipante=x.EstadoUsuarioParticipante,
                        Tipo="U",
                        IDUsuarioGrupo=x.Usuario.IDUsuario,
                        Nombre=x.Usuario.NombreUsuario,
                        


                        Usuario = new Usuario
                        {
                            NombreUsuario = x.Usuario.NombreUsuario,
                        }
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
        public short Editar(UsuarioParticipante UsuarioParticipante)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad =db.UsuarioParticipantes.Find(UsuarioParticipante.IDUsuarioParticipante);
                    entidad.EstadoUsuarioParticipante = UsuarioParticipante.EstadoUsuarioParticipante;
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
