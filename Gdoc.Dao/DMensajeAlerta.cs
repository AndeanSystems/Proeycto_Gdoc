using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DMensajeAlerta
    {
        public List<EMensajeAlerta> ListarMensajeAlerta()
        {
            var listMensajeAlerta = new List<EMensajeAlerta>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.MensajeAlertas.ToList();

                    var list2 = (from mensajealerta in db.MensajeAlertas
                                
                                join operacion in db.Operacions
                                on mensajealerta.IDOperacion equals operacion.IDOperacion

                                 join usuario in db.Usuarios
                                 on mensajealerta.IDUsuario equals usuario.IDUsuario

                                join usuarioparticipante in db.UsuarioParticipantes
                                on operacion.IDOperacion equals usuarioparticipante.IDOperacion



                                 join tipooperacion in db.Conceptoes
                                 on operacion.TipoOperacion equals tipooperacion.CodiConcepto

                                join tipodocumento in db.Conceptoes
                                on operacion.TipoDocumento equals tipodocumento.CodiConcepto

                                where tipodocumento.TipoConcepto.Equals("012")
                                      && tipooperacion.TipoConcepto.Equals("003") 

                                select new { mensajealerta, usuarioparticipante, tipooperacion, tipodocumento,usuario,operacion }).ToList();

                    list2.ForEach(x => listMensajeAlerta.Add(new EMensajeAlerta
                    {
                        IDMensajeAlerta = x.mensajealerta.IDMensajeAlerta,
                        IDOperacion = x.mensajealerta.IDOperacion,
                        FechaAlerta = x.mensajealerta.FechaAlerta,
                        TipoAlerta = x.mensajealerta.TipoAlerta,
                        CodigoEvento = x.mensajealerta.CodigoEvento,
                        EstadoMensajeAlerta = x.mensajealerta.EstadoMensajeAlerta,
                        IDUsuario = x.mensajealerta.IDUsuario,

                        TipoOperacion = new Concepto
                        {
                            DescripcionConcepto = x.tipooperacion.DescripcionConcepto,
                        },
                        TipoDocumento=new Concepto
                        {
                            DescripcionConcepto=x.tipodocumento.DescripcionConcepto,
                        },
                        Operacion =new Operacion
                        {
                            NumeroOperacion=x.operacion.NumeroOperacion,
                        },
                        Usuario = new Usuario
                        {
                            NombreUsuario=x.usuario.NombreUsuario,
                        }
                        
                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listMensajeAlerta;
        }
    }
}
