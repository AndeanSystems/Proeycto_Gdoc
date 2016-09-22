using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DLogOperacion
    {
        public List<ELogOperacion> ListarLogOperacion()
        {
            var listLogOperacion = new List<ELogOperacion>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.LogOperacions.ToList();

                    var list2 = (from logoperacion in db.LogOperacions
                                 join usuario in db.Usuarios
                                 on logoperacion.IDUsuario equals usuario.IDUsuario

                                 join operacion in db.Operacions
                                 on logoperacion.IDOperacion equals operacion.IDOperacion

                                 join evento in db.Conceptoes
                                 on logoperacion.CodigoEvento equals evento.CodiConcepto

                                 where evento.TipoConcepto.Equals("008")

                                 select new { logoperacion, usuario, evento, operacion }).ToList();


                    list2.ForEach(x => listLogOperacion.Add(new ELogOperacion
                    {
                        IDLogOperacion = x.logoperacion.IDLogOperacion,
                        FechaEvento = x.logoperacion.FechaEvento,
                        IDOperacion = x.logoperacion.IDOperacion,
                        CodigoEvento = x.logoperacion.CodigoEvento,
                        IDUsuario = x.logoperacion.IDUsuario,
                        CodigoConexion = x.logoperacion.CodigoConexion,
                        TerminalConexion = x.logoperacion.TerminalConexion,

                        Operacion = new Operacion{
                            NumeroOperacion=x.operacion.NumeroOperacion,
                            TipoOperacion=x.operacion.TipoOperacion,
                        },
                        Usuario = new Usuario{NombreUsuario=x.usuario.NombreUsuario},
                        Evento= new Concepto{DescripcionConcepto=x.evento.DescripcionConcepto},

                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listLogOperacion;
        }
        public LogOperacion GrabarLogOperacion(LogOperacion logoperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.LogOperacions.Add(logoperacion);
                    db.SaveChanges();
                }
                return logoperacion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
