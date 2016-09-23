using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DMesaVirtualComentario
    {
        public List<MesaVirtualComentario> ListarMesaVirtualComentario()
        {
            var listMesaVirtualComentario = new List<MesaVirtualComentario>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.MesaVirtualComentarios.ToList();

                    var list2 = (from comentario in db.MesaVirtualComentarios

                                 join usuario in db.Usuarios
                                 on comentario.IDUsuario equals usuario.IDUsuario

                                 select new { comentario,usuario}).ToList();


                    list2.ForEach(x => listMesaVirtualComentario.Add(new MesaVirtualComentario
                    {
                        IDComentarioMesaVirtual=x.comentario.IDComentarioMesaVirtual,
                        ComentarioMesaVirtual=x.comentario.ComentarioMesaVirtual,
                        FechaPublicacion=x.comentario.FechaPublicacion,
                        EstadoComentario=x.comentario.EstadoComentario,
                        IDOperacion=x.comentario.IDOperacion,
                        IDUsuario=x.comentario.IDUsuario,

                        Usuario=new Usuario{
                            NombreUsuario=x.usuario.NombreUsuario,
                        },

                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listMesaVirtualComentario;
        }
        public short GrabarMesaVirtualComentario(MesaVirtualComentario mesaVirtualComentario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.MesaVirtualComentarios.Add(mesaVirtualComentario);
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
