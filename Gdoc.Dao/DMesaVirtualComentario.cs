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
        public List<EMesaVirtualComentario> ListarMesaVirtualComentario()
        {
            var listMesaVirtualComentario = new List<EMesaVirtualComentario>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var destinatarios = new List<String>();
                    var list = db.MesaVirtualComentarios.ToList();

                    var list2 = (from comentario in db.MesaVirtualComentarios

                                 //join alerta in db.MensajeAlertas
                                 //on comentario.IDComentarioMesaVirtual equals alerta.IDComentarioMesaVirtual 

                                 join usuario in db.Usuarios
                                 on comentario.IDUsuario equals usuario.IDUsuario

                                 select new { comentario, usuario }).ToList();


                    var listDestinatariosAlertas = (from alerta in db.MensajeAlertas

                                                    join comentario in db.MesaVirtualComentarios
                                                    on alerta.IDComentarioMesaVirtual equals comentario.IDComentarioMesaVirtual

                                                    join usuario in db.Usuarios
                                                    on alerta.IDUsuario equals usuario.IDUsuario

                                                    select new { alerta, comentario, usuario }).ToList();


                    foreach (var x in list2)
                    {
                        foreach (var item in listDestinatariosAlertas.Where(y => y.comentario.IDOperacion == x.comentario.IDOperacion 
                            && y.comentario.IDComentarioMesaVirtual == x.comentario.IDComentarioMesaVirtual && y.alerta.TipoAlerta == 4))
                        {
                            destinatarios.Add(item.usuario.NombreUsuario);
                        }
                        listMesaVirtualComentario.Add(new EMesaVirtualComentario
                        {
                            IDComentarioMesaVirtual = x.comentario.IDComentarioMesaVirtual,
                            ComentarioMesaVirtual = x.comentario.ComentarioMesaVirtual,
                            FechaPublicacion = x.comentario.FechaPublicacion,
                            EstadoComentario = x.comentario.EstadoComentario,
                            IDOperacion = x.comentario.IDOperacion,
                            IDUsuario = x.comentario.IDUsuario,

                            Usuario = new Usuario
                            {
                                NombreUsuario = x.usuario.NombreUsuario,
                            },

                            //Destinatarios = string.Join(", ", x.alerta.Usuario.NombreUsuario.ToArray()),
                            Destinatarios = string.Join(", ", destinatarios.ToArray()),
                        });
                        destinatarios = new List<String>();
                    }
                    //list2.ForEach(x => listMesaVirtualComentario.Add(new MesaVirtualComentario
                    //{
                    //    IDComentarioMesaVirtual = x.comentario.IDComentarioMesaVirtual,
                    //    ComentarioMesaVirtual = x.comentario.ComentarioMesaVirtual,
                    //    FechaPublicacion = x.comentario.FechaPublicacion,
                    //    EstadoComentario = x.comentario.EstadoComentario,
                    //    IDOperacion = x.comentario.IDOperacion,
                    //    IDUsuario = x.comentario.IDUsuario,

                    //    Usuario = new Usuario
                    //    {
                    //        NombreUsuario = x.usuario.NombreUsuario,
                    //    },

                    //}));
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
